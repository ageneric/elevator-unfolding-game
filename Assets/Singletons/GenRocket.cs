using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenRocket : MonoBehaviour
{
    public static GenRocket Instance { get; private set; }

    public string keyName = "Rocket";
    public int owned = 0;

    public void Awake() {
        if (Instance is null) {
            Instance = this;
        }
    }

    public float Cost() {
        return Mathf.Pow(10, 2 + owned);
    }

    public bool CanPurchase() {
        if (Game.Player.ascend && Game.Player.height >= Cost()) {
            if (GenCable.Instance.owned >= 1) {
                return true;
            }
        }

        return false;
    }

    public void Purchase() {
        if (CanPurchase()) {
            Game.Player.height -= Cost();
            GenCable.Instance.owned -= 1;
            owned++;
        }
    }
}
