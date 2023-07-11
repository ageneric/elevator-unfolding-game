using UnityEngine;

public class GenWing : MonoBehaviour
{
    public static GenWing Player { get; private set; }

    public string keyName = "Wing";
    public int owned = 0;

    public void Awake() {
        if (Player is null) {
            Player = this;
        }
    }

    public float Cost() {
        if (owned < 10) {
            return 2 * (Mathf.Max(0, 10 - owned) + Mathf.Pow(owned, 3));
        }
        else {
            return 2 * Mathf.Pow(owned, 3);
        }
    }

    public bool CanPurchase() {
        if (Game.Player.ascend && Game.Player.height >= Cost()) {
            if (GenCable.Player.owned >= 2 
                    || (GenRocket.Player.owned >= 1 && GenCable.Player.owned >= 1)
                    || (Upgrade.upgradeWingCost && owned < 10)) {
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
            if (!(Upgrade.upgradeWingCost && owned < 10)) {
                GenCable.Player.owned = Mathf.Max(0, GenCable.Player.owned - 1);
            }
            owned++;
        }
    }
}
