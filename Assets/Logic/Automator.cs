using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Automator : MonoBehaviour
{
    public float interval = 0.5f;
    public static bool automatorEnabled = false;  // if enabled by player; still must be purchased
    public Toggle automatorToggle;
    private float timeSinceTick;

    private void Start() {
        timeSinceTick = 0f;
    }

    private void FixedUpdate() {
        if (Upgrade.automatorCable && automatorEnabled && !Game.Player.pause && Game.Player.ascend) {
            timeSinceTick = Mathf.Min(interval * 8, timeSinceTick + Time.deltaTime);
            TickAutomator();
        }
        else {
            timeSinceTick = 0f;
        }
    }

    void TickAutomator() {
        // Automatically buy Cable with an interval of 0.5 - 1s if affordable
        // timeSinceTick may be negative after purchases; this is an intentional delay
        if (timeSinceTick > interval) {
            if (Game.Player.height > GenCable.Player.Cost() + Game.Player.rate/6f) {
                while (GenCable.Player.CanPurchase()) {
                    // Inline copy of GenCable.Purchase()
                    float cost = GenCable.Player.Cost();
                    Game.Player.height -= cost;
                    Game.Player.lostRunHeight += cost;
                    GenCable.Player.owned++;
                    timeSinceTick -= interval / 2;
                }
                timeSinceTick -= interval;
            }
            if (timeSinceTick < interval * -4f) {
                timeSinceTick = interval * -4f;
            }
        }
    }

    public void ToggleAutomator() {
        automatorEnabled = !automatorEnabled;
    }
}
