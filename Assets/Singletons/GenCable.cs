using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenCable : MonoBehaviour
{
    public static GenCable Instance { get; private set; }

    public void Awake() {
        if (Instance is null) {
            Instance = this;
        }
    }

    public string keyName = "Cable";
    public int owned = 0;

    public float Cost() {
        int stackOwned = owned + GenWing.Instance.owned + GenRocket.Instance.owned;

        if (stackOwned == 0) {
            return 0;
        }
        else if (stackOwned == 1) {
            return 6;
        }
        else if (stackOwned < 10) {
            return Mathf.Max(0, 10 - stackOwned) + Mathf.Pow(stackOwned, 2);
        }
        else {
            return Mathf.Pow(2, Mathf.Floor(stackOwned / 5) - 1) * Mathf.Pow(stackOwned, 2);
        }
    }

    public bool CanPurchase() {
        if (Game.Player.ascend && Game.Player.height >= Cost()) {
            return true;
        }

        return false;
    }

    public void Purchase() {
        if (CanPurchase()) {
            Game.Player.height -= Cost();
            owned++;
        }
    }
}
