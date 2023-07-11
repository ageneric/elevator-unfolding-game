using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fall : MonoBehaviour
{
    public Upgrade upgradeScript;
    public MessageLog messageLog;
    public CameraController cameraControllerScript;
    public GameObject fallingUI;
    public GameObject buttonPrepareFall;
    public GameObject buttonStageStart;
    public GameObject buttonStageCancel;
    public GameObject buttonResetFall;
    private Button interactButtonPrepareFall;
    private float lastDepth;
    private bool firstDrillRun;
    public GameObject drillAnimObject;
    public GameObject rocketAnimObject;
    public ParticleSystem hitGroundParticle;
    public Transform particleTransform;
    private Vector3 drillAnimStartPosition;

    public void FixedUpdate() {
        if (Game.Player.ascend) return;  // See Generator.cs for ascending logic

        if (Game.Player.height <= 0 && !Game.Player.pause) {
            FallStatisticGain();
            Game.Player.pause = true;
        }

        TickFalling();
        Game.Player.bonusGameSpeedTime = 0;
    }

    private void Start() {
        lastDepth = 0f;
        firstDrillRun = true;
        interactButtonPrepareFall = buttonPrepareFall.GetComponent<Button>();
        drillAnimStartPosition = drillAnimObject.GetComponent<Transform>().localPosition;

        buttonPrepareFall.SetActive(false);
        buttonStageStart.SetActive(false);
        buttonStageCancel.SetActive(false);
        buttonResetFall.SetActive(false);
        FallUpdateBuyables(false);
        Load();
    }

    public void Load() {
        if (Game.Player.pause && Game.Player.ascend) {
            PrepareFall();
        }
        else if (!Game.Player.pause && !Game.Player.ascend) {
            FallUpdateBuyables(true);
            buttonStageCancel.SetActive(true);
        }
        else if (Game.Player.pause && !Game.Player.ascend) {
            FallUpdateBuyables(true);
            buttonResetFall.SetActive(true);
        }
    }

    public void TickFalling() {
        float dTime = Time.deltaTime;
        float time = Mathf.Max(Time.time - Game.Player.fallTime, 0);
        float rate = 0;

        if (Game.Player.height > -lastDepth) {
            // Free fall under gravity.
            rate = Helper.Gravity() * time * (1 + GenWing.Player.owned);
            rate -= Mathf.Pow(Mathf.Max(1, Game.Player.height - 99), 0.3125f) * GenRocket.Player.owned * (1 + GenWing.Player.owned);
            rate -= Mathf.Pow(Mathf.Max(0, Game.Player.height), 0.75f) * 0.1f * Mathf.Max(0, time); // Safety around falls taking too long
            if (GenTether.Player.owned > 0) {
                rate = rate * (GenTether.Player.owned*4 + 1); // Tether effectiveness
            }
            Game.Player.height = Mathf.Clamp(Game.Player.height + rate * dTime, -lastDepth, 1e30f);
        }
        else if (Game.Player.pause && Game.Player.height > -Game.Player.boreDepth) {
            // Visual deceleration (does not affect mechanics - only requirement is to
            // interpolate between 0m or lastDepth and the pre-calculated bore depth).
            rate -= Game.Player.maxRunHeight / (10 + Mathf.Sqrt(Mathf.Abs(Game.Player.height)));
            Game.Player.height = Mathf.Clamp(Game.Player.height + rate * dTime, -Game.Player.boreDepth, 0f);
        }
        else {
            // Idle deceleration or stopping. The drill will slowly add additional tunnel depth.
            if (Upgrade.upgradeDrillUnlock && !firstDrillRun) {
                float fallPressure = 0.015f * Game.Player.maxRunHeight / (6 + time);
                rate = Mathf.Clamp(0.015f - Mathf.Pow(fallPressure, 0.9f), 0f, 0.5f);
            }
            else {
                rate = 0;
            }
            Game.Player.boreDepth -= rate * dTime;
            Game.Player.height = -Game.Player.boreDepth;
        }

        Game.Player.rate = rate;
    }

    public void FallStatisticGain() {
        float time = Mathf.Max(Time.time - Game.Player.fallTime, 0);

        float fallPressure = 2f * Game.Player.maxRunHeight / Mathf.Pow(6f + time, 0.9375f);
        if (!Upgrade.upgradeDrillUnlock && fallPressure > 0) {
            fallPressure = Mathf.Pow(fallPressure, 0.8125f);
        }
        else {
            fallPressure = Mathf.Pow(fallPressure, 0.890625f);
        }
        if (fallPressure + Game.Player.boreDepth > 100f) {
            float toMantlePressure = Mathf.Max(0f, 100f - Game.Player.boreDepth);
            fallPressure = toMantlePressure + Mathf.Pow(fallPressure - toMantlePressure,
                0.96f - 0.05f*Mathf.Clamp(Mathf.Log10(Mathf.Max(100f, fallPressure)) - 2f, 0f, 14.2f));
        }
        lastDepth = Game.Player.boreDepth;
        Game.Player.boreDepth += fallPressure;

        if (Game.Player.boreDepth < 5) {
            Game.Player.boreDepth = 5;  // Fail-safe: get the player to Basement
        }
        buttonStageCancel.SetActive(false);
        buttonResetFall.SetActive(true);
        if (!Upgrade.upgradeWingCost || !Upgrade.upgradeRocketUnlock) {
            upgradeScript.SwitchTabUpgrade();
        }

        if (Game.Player.boreDepth > 15) {
            messageLog.AddMessage("Crash! Impact with " + Helper.CurrencyReading(fallPressure, " units") + " of force.");
        }
        cameraControllerScript.PlayExplosionSound();

        ParticleSystem particle = Instantiate(hitGroundParticle, particleTransform);
        StartCoroutine(ManageParticleSystem(particle));
    }

    public IEnumerator ManageParticleSystem(ParticleSystem particle) {
        float scaleHeight = -0.512f;
        Transform transform = particle.gameObject.GetComponent<Transform>();
        Vector3 setPosition = new Vector3(0, 0, 0);

        while (particle.isPlaying) {
            setPosition.y = scaleHeight * Game.Player.height;
            transform.localPosition = setPosition;
            yield return new WaitForEndOfFrame();
        }
        Destroy(particle);
        Debug.Log("Cleared 'on-impact' particle system");
    }

    private void Update() {
        if (Game.Player.ascend && Game.Player.height >= 50) {
            interactButtonPrepareFall.interactable = true;
        }
        else {
            interactButtonPrepareFall.interactable = false;
        }
        if (Game.Player.pause && Game.Player.ascend && Game.Player.height < 50) {
            CancelFall();
        }
    }

    // Actions called by the fall control buttons.
    public void PrepareFall() {
        buttonPrepareFall.SetActive(false);
        buttonStageStart.SetActive(true);
        buttonStageCancel.SetActive(true);

        Game.Player.pause = true;
        FallUpdateBuyables(true);
    }

    public void CancelFall() {
        buttonPrepareFall.SetActive(true);
        buttonStageStart.SetActive(false);
        buttonStageCancel.SetActive(false);

        Game.Player.ascend = true;
        Game.Player.pause = false;
        FallUpdateBuyables(false);
    }

    public void StartFall() {
        buttonPrepareFall.SetActive(false);
        buttonStageStart.SetActive(false);

        Game.Player.SetStartFall();
    }

    public void StartReset() {
        buttonPrepareFall.SetActive(true);
        buttonResetFall.SetActive(false);
        upgradeScript.SwitchTabGenerator();
        FallUpdateBuyables(false);
        Game.Player.SetStartRun();
    }

    public void FallUpdateBuyables(bool enableFallGraphics) {
        if (Upgrade.upgradeDrillUnlock) {
            firstDrillRun = false;
            // Rewind the animation so end remains correct then disable object
            // This animation modifies the transform so it needs to be reset manually
            drillAnimObject.GetComponent<Transform>().localPosition = drillAnimStartPosition;
            drillAnimObject.SetActive(enableFallGraphics);
        }
        if (Generator.GenOwnedById(3) > 0) {
            if (enableFallGraphics)
                rocketAnimObject.transform.localScale = new Vector3(1f, -1f, 1f);
            else
                rocketAnimObject.transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }


    public void DevSpeed() {
        GenCable.Player.owned += 5;
        Game.Player.height += 50;
        GenTether.Player.owned = 1;
    }
}
