using UnityEngine;

public class Generator : MonoBehaviour
{
    // Manages height generation while the elevator is ascending.
    // See Fall.cs for descending logic.
    public MessageLog messageLog;

    private void FixedUpdate() {
        if (!Game.Player.pause && Game.Player.ascend) {
            TickGenerator(Time.deltaTime);
        }
    }

    public void TickGenerator(float dTime) {
        if (Game.Player.bonusGameSpeedTime > 0f || dTime > 5f) {
            // Cap the delta-time processed in one tick to 5 seconds (plus any boosted production because of banked time).
            float dTimeAfterBonus = Mathf.Min(dTime, 5f);
            // Add banked time to increase the delta-time up to a multiple of its real value.
            if (Game.Player.bonusGameSpeedTime > 0f) {
                dTimeAfterBonus = Mathf.Min(dTimeAfterBonus * 9f, dTimeAfterBonus + Game.Player.bonusGameSpeedTime);
            }
            // Subtract used time / add lost time in the delta-time processed to the banked time.
            Game.Player.bonusGameSpeedTime = Mathf.Min(Game.Player.bonusGameSpeedTime + dTime - dTimeAfterBonus, 1200f);
            dTime = dTimeAfterBonus;
        }

        float rate = 0;
        rate += GenCable.Player.owned * Helper.CableMultiplier() * (1 + Mathf.Pow(GenWing.Player.owned, GenTether.Player.TetherPower()));
        rate += GenRocket.Player.RocketPower() * GenRocket.Player.owned * (1 + Mathf.Pow(GenWing.Player.owned, GenTether.Player.TetherPower()));
        
        if (Game.Player.height + rate * dTime >= 100f) {
            if (Game.Player.height + (rate + Helper.Gravity()) * dTime < 100f) {
                Game.Player.rate = rate + Helper.Gravity();
                rate = 0f;  // Elevator stalls at end of railing
                Game.Player.height = 100f;
                Game.Player.lostRunHeight -= (Helper.Gravity() + rate) * dTime;
            }
            else {
                rate += Helper.Gravity();  // Elevator disconnects from railing, add gravity
                Game.Player.rate = rate;
                Game.Player.lostRunHeight -= Helper.Gravity() * dTime;
            }
        }
        else {
            Game.Player.rate = rate;
        }

        Game.Player.height = Mathf.Clamp(Game.Player.height + rate * dTime, 0f, 3e38f);
        Game.Player.maxRunHeight = Mathf.Max(Game.Player.maxRunHeight, Game.Player.height);
    }

    public static int GenOwnedById(int id) {
        switch (id) {
            case 1: return GenCable.Player.owned;
            case 2: return GenWing.Player.owned;
            case 3: return GenRocket.Player.owned;
            case 4: return GenTether.Player.owned;
            default:
                Debug.LogError("GenById (owned) " + id + " not found.");
                return 0;
        }
    }

    public static float GenCostById(int id) {
        switch (id) {
            case 1: return GenCable.Player.Cost();
            case 2: return GenWing.Player.Cost();
            case 3: return GenRocket.Player.Cost();
            case 4: return GenTether.Player.Cost();
            default:
                Debug.LogError("GenById (cost) " + id + " not found.");
                return 0;
        }
    }

    public static bool GenCanPurchase(int id) {
        switch (id) {
            case 1: return GenCable.Player.CanPurchase();
            case 2: return GenWing.Player.CanPurchase();
            case 3: return GenRocket.Player.CanPurchase();
            case 4: return GenTether.Player.CanPurchase();
            default:
                Debug.LogError("GenById (cost) " + id + " not found.");
                return false;
        }
    }
}
