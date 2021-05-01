using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Unfolding : MonoBehaviour
{
    float lastHeightMessage = 0;
    bool hasUnlockedFall = false;
    bool hasUnlockedBasement = false;
    bool hasUnlockedLab = false;
    bool hasCommentedDrill = false;

    public MessageLog messageLog;
    public GameObject prepareFallButton;
    public GameObject currency;
    public GameObject wingUnlock;
    public GameObject switchTab;
    public GameObject basementLoc;
    public GameObject secretLabLoc;

    public GameObject elevator;

    void Start() {
        prepareFallButton.SetActive(false);
        wingUnlock.SetActive(false);
        currency.SetActive(false);
        switchTab.SetActive(false);

        basementLoc.SetActive(false);
        secretLabLoc.SetActive(false);

        // Stop Unity from making the sprite black.
        elevator.SetActive(false);
        elevator.SetActive(true);

        messageLog.AddMessage("Testing Elevator 1. Please proceed.");
    }

    // Update is called once per frame
    void Update()
    {
        if (lastHeightMessage < 1 && Game.Player.height > 0) {
            lastHeightMessage = 1;
            currency.SetActive(true);
        }

        else if (lastHeightMessage < 10 && Game.Player.height >= 10) {
            lastHeightMessage = 10;
            wingUnlock.SetActive(true);
        }

        else if (!hasUnlockedFall && Game.Player.height >= 30) {
            hasUnlockedFall = true;
            prepareFallButton.SetActive(true);
            messageLog.AddMessage("We haven't found a way to get it back down. That won't be a problem, right?");
        }

        else if (lastHeightMessage < 50 && Game.Player.height >= 50) {
            lastHeightMessage = 50;
            messageLog.AddMessage("How about we cut the elevator cables? Don't worry, there's no-one inside.");
        }

        else if (!hasUnlockedBasement && lastHeightMessage < 70 && Game.Player.height >= 70) {
            lastHeightMessage = 70;
            messageLog.AddMessage("Click 'Cut Cables!', which will send the elevator down by gravity.");
        }

        else if (!hasCommentedDrill && Upgrade.upgradeDrillUnlock) {
            hasCommentedDrill = true;
            messageLog.AddMessage("Looks powerful. I'll attach the drill when we're at the top.");
        }

        else if (!hasUnlockedBasement && Game.Player.height <= -5) {
            hasUnlockedBasement = true;
            messageLog.AddMessage("Ouch! We fell into the... basement? I didn't know this was here.");
            switchTab.SetActive(true);
            basementLoc.SetActive(true);
        }

        else if (!hasUnlockedLab && Game.Player.height <= -50) {
            hasUnlockedLab = true;
            messageLog.AddMessage("This is some kind of secret lab! Adding Jet Engine blueprint.");
            secretLabLoc.SetActive(true);
        }
    }
}
