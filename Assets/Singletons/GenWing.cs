using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenWing : MonoBehaviour
{
    public static GenWing Instance { get; private set; }

    public string keyName = "Wing";
    public int owned = 0;

    public void Awake() {
        if (Instance is null) {
            Instance = this;
        }
    }

    public float Cost() {
        return Mathf.Max(0, 2 * (10 - owned + Mathf.Pow(owned, 3)));
    }

    public bool CanPurchase() {
        if (Game.Player.ascend && Game.Player.height >= Cost()) {
            if (GenCable.Instance.owned >= 2 || (GenRocket.Instance.owned >= 1 && GenCable.Instance.owned >= 1)) {
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
