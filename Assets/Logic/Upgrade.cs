using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Upgrade : MonoBehaviour
{
    public MessageLog messageLog;
    public GameObject tabGenerator;
    public GameObject tabUpgrade;
    public GameObject rocketUnlock;
    public GameObject[] upgradeButtons;

    public static bool upgradeRocketUnlock = false;
    public static bool upgradeCablesMultiplier = false;
    public static bool upgradeDrillUnlock = false;


    // Start is called before the first frame update
    void Start()
    {
        tabUpgrade.SetActive(false);
        rocketUnlock.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SwitchTabGenerator() {
        tabGenerator.SetActive(true);
        tabUpgrade.SetActive(false);
    }

    public void SwitchTabUpgrade() {
        tabGenerator.SetActive(false);
        tabUpgrade.SetActive(true);
    }

    public void BuyDrillUnlock() {
        if (Game.Player.deepCurrency >= 5) {
            upgradeDrillUnlock = true;
            upgradeButtons[0].GetComponent<Button>().interactable = false;
        }
        else {
            messageLog.AddMessage("Reach -5m (The Basement) to unlock the Drill blueprint.");
        }
    }

    public void BuyCablesMultiplier() {
        if (Game.Player.deepCurrency >= 15) {
            upgradeCablesMultiplier = true;
            upgradeButtons[1].GetComponent<Button>().interactable = false;
        }
        else {
            messageLog.AddMessage("Reach -15m to unlock.");
        }
    }

    public void BuyRocketUnlock() {
        if (Game.Player.deepCurrency >= 50) {
            upgradeRocketUnlock = true;
            rocketUnlock.SetActive(true);
            upgradeButtons[2].GetComponent<Button>().interactable = false;
        }
        else {
            messageLog.AddMessage("Reach -50m to unlock.");
        }
    }
}
