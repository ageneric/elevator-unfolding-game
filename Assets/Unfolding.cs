using UnityEngine;
using UnityEngine.UI;

public class Unfolding : MonoBehaviour
{
    // Manages the unfolding and story aspect of the game by
    // Revealing game elements as they are unlocked and showing radio messages
    float lastHeightMessage = 0;
    bool hasUsedFall = false;
    bool hasUnlockedBasement = false;
    bool hasUnlockedLab = false;
    bool hasCommentedDrill = false;
    bool hasReachedMillSpeed = false;
    bool hasBrokenLightSpeed = false;
    bool hasMaxedTether = false;

    public Upgrade upgradeScript;
    public MessageLog messageLog;
    public GameObject prepareFallButton;
    public GameObject currency;
    public GameObject wingUnlock;
    public GameObject switchTab;
    public GameObject basementLoc;
    public GameObject secretLabLoc;
    public GameObject elevator;
    public GameObject secretLabSwitchTab;
    public GameObject tetherHint;
    public GameObject tetherUnlock;
    public GameObject tetherMax;
    public GameObject tetherDisplayCost;
    public GameObject speedSlider;

    public Text tetherText1;
    public Text tetherText2;

    public static string langElevator = "elevator";

    public string[] messages = {
        "_Replace these messages in Editor",
        "_Height1",
        "_Height2",
        "_Message",
        "_Cut",
        "_Basement reached",
        "_Lab reached",
        "_Height",
        "_Commented drill"
    };

    void Start() {
        prepareFallButton.SetActive(false);
        wingUnlock.SetActive(false);
        currency.SetActive(false);
        switchTab.SetActive(false);
        speedSlider.SetActive(false);

        basementLoc.SetActive(false);
        secretLabLoc.SetActive(false);
        tetherHint.SetActive(false);
        tetherUnlock.SetActive(false);
        Load();
        
        // Stop Unity from making the sprite black.
        elevator.SetActive(false);
        elevator.SetActive(true);
    }

    public void Load() {
        if (Game.Player.boreDepth >= 5) {
            lastHeightMessage = 90;
            currency.SetActive(true);
            wingUnlock.SetActive(true);
            prepareFallButton.SetActive(true);
            if (GenTether.Player.owned > 0) {
                tetherUnlock.SetActive(true);
                speedSlider.SetActive(true);
                hasReachedMillSpeed = true;
            }
            Update();
            messageLog.AddMessage("Whirring and buzzing sounds are heard.");
            messageLog.AddMessage("(You loaded a save.)");
        }
        else {
            AddMessageFromIndex(0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (lastHeightMessage < 1 && Game.Player.height > 0) {
            lastHeightMessage = 1;
            currency.SetActive(true);
            AddMessageFromIndex(1);
            AddMessageFromIndex(2);
        }
        if (lastHeightMessage < 6 && Game.Player.height >= 6) {
            lastHeightMessage = 6;
            wingUnlock.SetActive(true);
        }
        if (lastHeightMessage < 30 && Game.Player.height >= 30) {
            lastHeightMessage = 30;
            prepareFallButton.SetActive(true);
            AddMessageFromIndex(3);
        }

        if (!hasUsedFall && Game.Player.pause) {
            hasUsedFall = true;
            AddMessageFromIndex(4);
        }

        if (!hasUnlockedBasement && lastHeightMessage < 90 && Game.Player.height >= 90) {
            lastHeightMessage = 90;
            AddMessageFromIndex(7);
        }

        if (!hasCommentedDrill && Upgrade.upgradeDrillUnlock) {
            hasCommentedDrill = true;
            AddMessageFromIndex(8);
        }

        if (!hasUnlockedBasement && Game.Player.boreDepth >= 5) {
            hasUnlockedBasement = true;
            AddMessageFromIndex(5);
            switchTab.SetActive(true);
            basementLoc.SetActive(true);
        }

        if (!hasUnlockedLab && Game.Player.boreDepth >= 50) {
            hasUnlockedLab = true;
            upgradeScript.ShowLabUpgrades();
            AddMessageFromIndex(6);
            secretLabLoc.SetActive(true);
            secretLabSwitchTab.SetActive(true);
        }

        if (!hasReachedMillSpeed && Game.Player.rate >= 15000 && Game.Player.rate < 90000) {
            tetherHint.SetActive(true);
            speedSlider.SetActive(true);
        }
        if (!hasReachedMillSpeed && Game.Player.rate >= 90000) {
            tetherUnlock.SetActive(true);
            tetherHint.SetActive(false);
            AddMessageFromIndex(10);
            hasReachedMillSpeed = true;
            speedSlider.SetActive(true);
        }

        if (!hasBrokenLightSpeed && (Game.Player.rate >= 300000000 || Game.Player.noRelativityEffect)) {
            AddMessageFromIndex(11);
            messageLog.AddMessage("Thank you for playing; this is the end of content.");
            hasBrokenLightSpeed = true;
            speedSlider.SetActive(false);

            tetherText1.text = "Photon Cores";
            tetherText2.text = tetherText2.text.Replace("Gravity", "Photon");
        }

        if (!hasMaxedTether && GenTether.Player.owned > 60) {
            tetherMax.SetActive(true);
        }

        if (!hasMaxedTether && GenTether.Player.owned >= GenTether.Player.maxOwned) {
            tetherMax.SetActive(false);
            tetherDisplayCost.SetActive(false);
            hasMaxedTether = true;
        }
    }

    void AddMessageFromIndex(int messageIndex) {
        messageLog.AddMessage(string.Format(messages[messageIndex], langElevator));
    }
}
