using UnityEngine;

public class GenCable : MonoBehaviour
{
    public static GenCable Player { get; private set; }

    public void Awake() {
        if (Player is null) {
            Player = this;
        }
    }

    public string keyName = "Cable";
    public int owned = 0;
    public int freeCableCount = 0;
    public int factoryCableCount = 0;

    public float Cost() {
        int stackOwned = owned + GenWing.Player.owned + GenRocket.Player.owned - freeCableCount - factoryCableCount;
        if (Upgrade.upgradeWingCost) {
            stackOwned -= Mathf.Min(10, GenWing.Player.owned);
        }
        stackOwned = Mathf.Max(0, stackOwned);

        if (stackOwned <= 1)
            return stackOwned * 6;
        else if (stackOwned <= 5)
            return Mathf.Max(0, 10 - stackOwned) + Mathf.Pow(stackOwned, 2);
        else if (stackOwned <= 10)
            return 2 * (Mathf.Max(0, 10 - stackOwned) + Mathf.Pow(stackOwned, 2));
        else
            return Mathf.Pow(2, Mathf.Floor(stackOwned / 5)) * Mathf.Pow(stackOwned, 2);
    }

    public bool CanPurchase() {
        if (Game.Player.ascend && Game.Player.height >= Cost()) {
            return true;
        }
        return false;
    }

    public void Purchase() {
        if (CanPurchase()) {
            float cost = Cost();
            Game.Player.height -= cost;
            Game.Player.lostRunHeight += cost;
            owned++;
        }
    }
}
