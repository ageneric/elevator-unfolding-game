using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Upgrade : MonoBehaviour
{
    public MessageLog messageLog;
    public GameObject tabGenerator;
    public GameObject tabUpgrade;
    public GameObject tabAutomator;
    public GameObject factoryMiniUI;
    public GameObject rocketUnlock;
    public Toggle automatorToggle;
    public Text nextProgressText;
    public Text wingCostLabelText;
    public GameObject[] upgradeButtons;
    public Text levelRocketsCostText;

    public static bool upgradeCablesMultiplier = false;
    public static bool upgradeDrillUnlock = false;
    public static bool upgradeRocketUnlock = false;
    public static bool upgradeWingCost = false;
    public static bool automatorCable = false;
    public static bool automatorFactory = false;
    public static int levelRocketsMultiplier = 0;
    public float levelRocketsMultiplierCost = 250f;

    // Start is called before the first frame update
    void Start() {
        tabGenerator.SetActive(true);
        tabUpgrade.SetActive(false);
        tabAutomator.SetActive(false);
        Load();
    }

    public void Load() {
        if (Game.Player.boreDepth >= 5) {
            BuyDrillUnlock();
        }
        if (Game.Player.boreDepth >= 50) {
            BuyCablesMultiplier();
            BuyRocketUnlock();
            if (Game.Player.boreDepth >= 75) {
                BuyWingCostUnlock();
            }
            if (automatorCable) {
                DoBuyCablesAutomator();
                Automator.automatorEnabled = false;
            }
            if (automatorFactory) {
                BuyFactory();
            }
            if (levelRocketsMultiplier > 0) {
                int targetLevel = levelRocketsMultiplier;
                levelRocketsMultiplier = 0;
                for (int i = 0; i < targetLevel; i++) {  // Re-buy so UI is up to date
                    BuyRocketsMultiplier();
                }
            }
        }
        else {
            rocketUnlock.SetActive(false);
            upgradeButtons[2].SetActive(false);
            upgradeButtons[3].SetActive(false);
        }
    }

    private void Update() {
        if (upgradeWingCost && GenWing.Player.owned < 10) {
            wingCostLabelText.text = "Spend Altitude";
        }
        else {
            wingCostLabelText.text = "Spend Altitude + 1 Cable";
        }

        upgradeButtons[6].GetComponent<Button>().interactable = levelRocketsMultiplier < 3 &&
            Game.Player.boreDepth >= levelRocketsMultiplierCost;
    }

    public void ShowLabUpgrades() {
        upgradeButtons[2].SetActive(true);
        upgradeButtons[3].SetActive(true);
        nextProgressText.gameObject.SetActive(false);
    }

    public void SwitchTabGenerator() {
        tabGenerator.SetActive(true);
        tabUpgrade.SetActive(false);
        tabAutomator.SetActive(false);
        factoryMiniUI.SetActive(true);
    }

    public void SwitchTabUpgrade() {
        tabGenerator.SetActive(false);
        tabUpgrade.SetActive(true);
        tabAutomator.SetActive(false);
        factoryMiniUI.SetActive(false);
    }

    public void SwitchTabAutomator() {
        tabGenerator.SetActive(false);
        tabUpgrade.SetActive(false);
        tabAutomator.SetActive(true);
        factoryMiniUI.SetActive(true);
    }

    public void BuyDrillUnlock() {
        if (Game.Player.boreDepth >= 5) {
            upgradeDrillUnlock = true;
            DisableUpgradeButtonByIndex(0);
        }
        else {
            messageLog.AddMessage("Reach -5m to unlock.");
        }
    }

    public void BuyCablesMultiplier() {
        if (Game.Player.boreDepth >= 15) {
            upgradeCablesMultiplier = true;
            DisableUpgradeButtonByIndex(1);
        }
        else {
            messageLog.AddMessage("Reach -15m to unlock.");
        }
    }

    public void BuyRocketUnlock() {
        if (Game.Player.boreDepth >= 50) {
            upgradeRocketUnlock = true;
            rocketUnlock.SetActive(true);
            DisableUpgradeButtonByIndex(2);
        }
        else {
            messageLog.AddMessage("Reach -50m to unlock.");
        }
    }

    public void BuyWingCostUnlock() {
        if (Game.Player.boreDepth >= 75) {
            upgradeWingCost = true;
            DisableUpgradeButtonByIndex(3);
        }
        else {
            messageLog.AddMessage("Reach -75m to unlock.");
        }
    }

    public void BuyCablesAutomator() {
        if (Game.Player.rate >= 300) {
            DoBuyCablesAutomator();
        }
        else {
            messageLog.AddMessage("Ascend at a speed of 300m/s to unlock.");
        }
    }

    private void DoBuyCablesAutomator() {
        automatorCable = true;
        GenCable.Player.freeCableCount += 1;
        DisableUpgradeButtonByIndex(4);
        automatorToggle.interactable = true;
    }

    public void BuyFactory() {
        if (Game.Player.boreDepth >= 140) {
            automatorFactory = true;
            upgradeButtons[5].SetActive(false);
            upgradeButtons[7].SetActive(true);
        }
        else {
            messageLog.AddMessage("Reach -140m to unlock.");
        }
    }

    public void BuyRocketsMultiplier() {
        if (Game.Player.boreDepth >= levelRocketsMultiplierCost) {
            if (levelRocketsMultiplier < 3) {
                levelRocketsMultiplier++;
                levelRocketsMultiplierCost *= 5 - levelRocketsMultiplier;
                levelRocketsCostText.text = Helper.CostReading(-levelRocketsMultiplierCost);
                upgradeButtons[6].GetComponentInChildren<Text>().text += "+";
            }
            if (levelRocketsMultiplier >= 3) {
                levelRocketsCostText.text = "MAX";
                DisableUpgradeButtonByIndex(6);
            }
        }
        else {
            messageLog.AddMessage("Insufficient depth to unlock.");
        }
    }

    public void DisableUpgradeButtonByIndex(int index) {
        upgradeButtons[index].GetComponent<Button>().interactable = false;
    }
}
