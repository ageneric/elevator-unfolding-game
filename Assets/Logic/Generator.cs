using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{
    public MessageLog messageLog;

    private void FixedUpdate() {
        if (Game.Player.pause) return;
        else if (!Game.Player.ascend) return;  // See Fall.cs

        float dTime = Time.deltaTime;
        float rate;

        rate = 0;
        rate += GenCable.Instance.owned * Helper.CableMultiplier() * (1 + GenWing.Instance.owned);
        rate += 10 * Mathf.Pow(Game.Player.height, 0.25f) * GenRocket.Instance.owned * (1 + GenWing.Instance.owned);

        Game.Player.rate = rate;
        Game.Player.height = Mathf.Clamp(Game.Player.height + rate * dTime, 0f, 1e30f);
        Game.Player.maxRunHeight = Mathf.Max(Game.Player.maxRunHeight, Game.Player.height);
    }

    public static int GenOwnedById(int id) {
        switch (id) {
            case 1: return GenCable.Instance.owned;
            case 2: return GenWing.Instance.owned;
            case 3: return GenRocket.Instance.owned;
            default:
                Debug.LogError("GenById (owned) " + id + " not found.");
                return 0;
        }
    }

    public static float GenCostById(int id) {
        switch (id) {
            case 1: return GenCable.Instance.Cost();
            case 2: return GenWing.Instance.Cost();
            case 3: return GenRocket.Instance.Cost();
            default:
                Debug.LogError("GenById (cost) " + id + " not found.");
                return 0;
        }
    }

    public static bool GenCanPurchase(int id) {
        switch (id) {
            case 1: return GenCable.Instance.CanPurchase();
            case 2: return GenWing.Instance.CanPurchase();
            case 3: return GenRocket.Instance.CanPurchase();
            default:
                Debug.LogError("GenById (cost) " + id + " not found.");
                return false;
        }
    }
}
