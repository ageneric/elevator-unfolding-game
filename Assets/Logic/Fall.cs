using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fall : MonoBehaviour
{
    public Upgrade upgradeScript;
    public GameObject fallingUI;
    public GameObject buttonStage1;
    public GameObject buttonStage2;
    public GameObject buttonStage3;
    private Button stage1Interact;
    public GameObject hitLabel;
    public Text hitLabelText;

    public GameObject drillAnimObject;
    public ParticleSystem hitGroundParticle;
    public Transform particleTransform;

    private void Start() {
        stage1Interact = buttonStage1.GetComponent<Button>();

        buttonStage1.SetActive(false);
        buttonStage2.SetActive(false);
        buttonStage3.SetActive(false);
        hitLabel.SetActive(false);
        drillAnimObject.SetActive(false);
    }

    public void PrepareFall() {
        buttonStage1.SetActive(false);
        buttonStage2.SetActive(true);

        Game.Player.pause = true;
        if (Upgrade.upgradeDrillUnlock) {
            drillAnimObject.SetActive(true);
        }
    }

    public void StartFall() {
        buttonStage1.SetActive(false);
        buttonStage2.SetActive(false);

        Game.Player.SetStartFall();
    }

    public void CompleteFall() {
        float time = Time.time - Game.Player.fallTime;

        float fallPressure = 2 * Game.Player.maxRunHeight / (6 + time);
        if (!Upgrade.upgradeDrillUnlock && fallPressure > 0) {
            fallPressure = Mathf.Pow(fallPressure, 0.75f);
            fallPressure = Mathf.Max(0, fallPressure - Game.Player.deepCurrency / 10);
        }
        else {
            fallPressure = Mathf.Max(0, fallPressure - Game.Player.deepCurrency / 100000);
        }

        Game.Player.deepCurrency += fallPressure;

        if (Game.Player.deepCurrency < 5) {
            Game.Player.deepCurrency = 5;  // Fail-safe: get the player to Basement
        }

        Game.Player.pause = true;

        buttonStage3.SetActive(true);
        hitLabelText.text = Helper.CurrencyReading(fallPressure, " kN!");
        hitLabel.SetActive(true);
        upgradeScript.SwitchTabUpgrade();

        ParticleSystem particle = Instantiate(hitGroundParticle, particleTransform);
        Destroy(particle, 3f);
    }

    public void StartReset() {
        buttonStage1.SetActive(true);
        buttonStage3.SetActive(false);
        hitLabel.SetActive(false);
        upgradeScript.SwitchTabGenerator();

        Game.Player.SetStartRun();
        drillAnimObject.SetActive(false);
    }

    public void FixedUpdate() {
        if (!Game.Player.ascend && !Game.Player.pause && Game.Player.height <= 0) {
            CompleteFall();
        }

        TickFallingGenerator();
    }

    void TickFallingGenerator() {
        if (Game.Player.ascend) return;  // See Generator.cs

        float dTime = Time.deltaTime;
        float rate = 0;

        if (Game.Player.height > 0) {
            rate = Helper.Gravity() * (1 + GenWing.Instance.owned);
            rate -= 20 * Mathf.Pow(Game.Player.height, 0.25f) * GenRocket.Instance.owned * (1 + GenWing.Instance.owned);

            Game.Player.height = Mathf.Clamp(Game.Player.height + rate * dTime, 0f, 1e30f);
        }
        else if (Game.Player.pause && Game.Player.height > -Game.Player.deepCurrency) {
            rate -= Game.Player.maxRunHeight / (10 + Mathf.Sqrt(Mathf.Abs(Game.Player.height)));
            Game.Player.height = Mathf.Clamp(Game.Player.height + rate * dTime, -Game.Player.deepCurrency, 0f);
        }
        else {
            rate = 0;
            Game.Player.height = -Game.Player.deepCurrency;
        }

        Game.Player.rate = rate;
    }

    private void Update() {
        if (Game.Player.ascend && Game.Player.height >= 50) {
            stage1Interact.interactable = true;
        }
        else {
            stage1Interact.interactable = false;
        }
    }
}
