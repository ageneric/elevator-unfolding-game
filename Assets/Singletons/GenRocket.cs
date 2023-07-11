using UnityEngine;

public class GenRocket : MonoBehaviour
{
    public static GenRocket Player { get; private set; }

    public string keyName = "Rocket";
    public int owned = 0;

    public void Awake() {
        if (Player is null) {
            Player = this;
        }
    }

    public float Cost() {
        if (Upgrade.automatorFactory) {
            if (owned >= 3) {
                // Produce nicer numbers by zero-ing insignificant final digits
                return 50 * (Mathf.Pow(9, owned) - Mathf.Pow(9, owned - 2));
            }
            else {
                return 50 * Mathf.Pow(9, owned);
            }
        }
        else {
            return 50 * Mathf.Pow(10, owned);
        }
    }

    public bool CanPurchase() {
        if (Game.Player.ascend && Game.Player.height >= Cost()) {
            if (GenCable.Player.owned >= 1) {
                return true;
            }
        }
        return false;
    }

    public void Purchase() {
        if (CanPurchase()) {
            float cost = Cost();
            Game.Player.height -= cost;
            Game.Player.lostRunHeight += cost;
            GenCable.Player.owned = Mathf.Max(0, GenCable.Player.owned - 1);
            owned++;
        }
    }

    public float RocketPower() {
        float levelMultiplier = 1f + Upgrade.levelRocketsMultiplier * 0.05f * (owned + GenCable.Player.owned);
        if (Game.Player.height < 100) {
            return 1.5f * levelMultiplier;
        }
        else {
            return 1.067f * Mathf.Pow(Game.Player.height, 0.3125f) * levelMultiplier;
        }
    }
}
