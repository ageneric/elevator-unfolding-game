using UnityEngine;

public class GenTether : MonoBehaviour
{
    public static GenTether Player { get; private set; }

    public void Awake() {
        if (Player is null) {
            Player = this;
        }
    }

    public string keyName = "Tether";
    public int owned = 0;
    public int maxOwned = 100;

    public float Cost() {
        int stackOwned = Mathf.Max(0, owned);
        if (stackOwned < maxOwned) {
            return Mathf.Max(Game.Player.height, (Mathf.Round(Mathf.Pow(stackOwned, 0.5f) * 10f) + 3f) * 100000);    
        }
        else {
            return 0f;
        }
    }

    public bool CanPurchase() {
        if (Game.Player.ascend && Game.Player.height >= Cost() && GenCable.Player.owned > owned && owned < maxOwned) {
            return true;
        }
        return false;
    }

    public void Purchase() {
        if (CanPurchase()) {
            float cost = Cost();
            Game.Player.lostRunHeight += Game.Player.height - Mathf.Sqrt(Game.Player.height);
            Game.Player.height = Mathf.Sqrt(Game.Player.height);

            owned = GenCable.Player.owned;
            GenCable.Player.owned = 0;
            
            if (owned > maxOwned) {
                owned = maxOwned;
            }
        }
    }

    public float TetherPower() {
        /*
        if (owned < 300) {
            if (owned == 60) {
                return 1.5f;
            }
            // maps 0 -> 1, 60 -> ~1.5, 300 -> ~2 with heavy diminishing returns
            return 1.15f - 0.15f/(1 + Mathf.Sqrt(Mathf.Max(0, owned)))
                         + Mathf.Log(1 + Mathf.Log(1 + Mathf.Max(0, (owned - 2)/102f)));
        }
        else {
            return 2f;
        }*/
        if (!Game.Player.noRelativityEffect) {
            if (owned < 100) {
                return Mathf.Pow(1.011f, owned);
            }
            else {
                return 3f;
            }
        }
        else {
            if (owned < 100) {
                return Mathf.Pow(1.0092f, owned);
            }
            else {
                return 2.5f;
            }
        }
    }
}
