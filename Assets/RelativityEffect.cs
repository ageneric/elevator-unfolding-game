using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelativityEffect : MonoBehaviour
{
    public SpriteRenderer elevatorSprite;
    public SpriteRenderer layer;
    public Transform elevatorTransform;
    public Transform shakeTransform;
    public CameraController cameraControllerScript;

    public Vector3 basePosition = new Vector3(0f, 0f, 0f);
    private bool active = false;
    private bool animationWait = false;
    private float effect = 1f;

    public void Update()
    {
        if (Game.Player.rate > Helper.lightSpeed && !Game.Player.noRelativityEffect)
        {
            Game.Player.noRelativityEffect = true;
            animationWait = true;
            StartCoroutine("HideLayer");
        }

        if (Helper.lightSpeed / 2 < Game.Player.rate && Game.Player.rate < Helper.lightSpeed && !Game.Player.noRelativityEffect)
        {
            effect = (effect*3f + Mathf.Pow(Mathf.Clamp01(2f * (Helper.lightSpeed - Game.Player.rate) / Helper.lightSpeed), 0.25f)) / 4f;
            elevatorSprite.color = new Color(effect, 1, 1, 1);
            elevatorTransform.localScale = new Vector3(4 * effect, 4 / Mathf.Clamp(effect + 0.01f, 0.01f, 1f), 4);
            shakeTransform.position = new Vector2(Random.Range(1f-effect, effect-1f), Random.Range(1f - effect, effect - 1f));

            layer.color = new Color(Mathf.Clamp01(effect*2), Mathf.Clamp01(effect * 2), Mathf.Clamp01(effect * 2), 1 - effect);
            active = true;
        }
        else if (active && !animationWait)
        {
            effect = 1f;
            elevatorSprite.color = new Color(1, 1, 1, 1);
            elevatorTransform.localScale = new Vector3(4, 4, 1);
            active = false;
            shakeTransform.position = basePosition;
            layer.color = new Color(0, 0, 0, 0);
        }
    }

    IEnumerator HideLayer()
    {
        layer.color = new Color(0, 0, 0, 1);
        effect = 0f;
        cameraControllerScript.PlayExplosionSound();
        yield return new WaitForSeconds(0.25f);

        for (int i=0; i<100; i++)
        {
            yield return new WaitForSeconds(0.03f);
            effect += 0.01f;
            elevatorSprite.color = new Color(effect, 1, 1, 1);
            elevatorTransform.localScale = new Vector3(4 * effect, 4 / Mathf.Clamp(effect + 0.01f, 0.01f, 1f), 4);
            shakeTransform.position = new Vector2(Random.Range(1f - effect, effect - 1f), Random.Range(1f - effect, effect - 1f));

            layer.color = new Color(0,0,0, 1 - effect);
        }

        layer.gameObject.SetActive(false);
        animationWait = false;
    }
}
