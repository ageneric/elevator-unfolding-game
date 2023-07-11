using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Camera mainCamera;
    public float restCameraSize = 5f;
    public float limitZoom = 1.75f;
    private float currentZoom;
    private float targetZoom;
    private Vector3 mainCameraPosition;

    public AudioSource musicPlayback;
    public AudioClip musicClip1;
    public AudioClip musicClip2;
    public AudioClip explosionSoundEffect;

    void Start() {
        currentZoom = 1f;
        targetZoom = 1f;
        mainCameraPosition = mainCamera.transform.position;
    }

    void Update() {
        // Camera position and scale: zoom / field of view increases with height.
        if (Game.Player.height > 0) {
            targetZoom = Mathf.Clamp(Mathf.Log10(Game.Player.height) - 1f, 1f, limitZoom);
        }
        else {
            targetZoom = 1f;
        }

        if (Mathf.Abs(currentZoom - targetZoom) < 0.0001f || Mathf.Abs(currentZoom - targetZoom) > 0.37f) {
            // Snap to size if close or if loading into a different size.
            // Camera resizing causes text to crackle so should avoid unnecessary resizing.
            currentZoom = targetZoom;
        }

        float deltaTime = Time.deltaTime;
        float damping = Mathf.Clamp01(Mathf.Pow(0.5f, deltaTime) - deltaTime);

        if ((Game.Player.rate > 0 && currentZoom < targetZoom)
            || (Game.Player.rate < 0 && currentZoom > targetZoom)) {
            currentZoom = targetZoom * (1 - damping) + currentZoom * damping;
        }
        else if (Game.Player.height <= 0f) {
            damping = damping * 0.9f;  // closer to 0: quickly snap to zero
            currentZoom = targetZoom * (1 - damping) + currentZoom * damping;
        }
        else if (Game.Player.height < 100f && Game.Player.rate < 10f) {
            damping = damping * 0.1f + 0.9f;  // closer to 1: smooth out height drops
            currentZoom = targetZoom * (1 - damping) + currentZoom * damping;
        }

        if (Mathf.Abs(mainCamera.orthographicSize - currentZoom * restCameraSize) > 0.001f) {
            mainCamera.orthographicSize = currentZoom * restCameraSize;
            mainCameraPosition.x = (currentZoom - 1f) * restCameraSize;
            mainCamera.transform.position = mainCameraPosition;
        }

        // Set the camera controller music component. Playback tails out above 100m.
        if (!musicPlayback.isPlaying) {
            if (Game.Player.height <= 100) {
                musicPlayback.clip = musicClip1;
                musicPlayback.Play();
            }
            else if (musicPlayback.clip == musicClip1 || !Game.Player.ascend) {
                musicPlayback.clip = musicClip2;
                musicPlayback.Play();
            }
        }
    }

    public void SetVolume() {
        if (musicPlayback.volume > 0) {
            musicPlayback.volume = 0f;
        }
        else {
            musicPlayback.volume = 1f;
        }
    }

    public void PlayExplosionSound() {
        musicPlayback.PlayOneShot(explosionSoundEffect, 0.25f);
    }
}
