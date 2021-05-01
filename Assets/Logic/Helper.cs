using UnityEngine;

public static class Helper
{
    public static string CurrencyReading(float amount, string unit = "m") {
        return Mathf.Floor(amount).ToString() + unit;
    }

    public static string CostReading(float amount, string unit = "m") {
        return Mathf.Ceil(amount).ToString() + unit;
    }

    public static string DeltaReading(float rate, string unit = "m") {
        if (rate > 0) {
            return "+" + Mathf.Round(rate).ToString() + unit;
        }
        else if (rate < 0) {
            return "-" + Mathf.Round(Mathf.Abs(rate)).ToString() + unit;
        }
        else {
            return "...";
        }
    }

    public static float Gravity() {
        float time = Time.time - Game.Player.fallTime;
        return -9.8f * time;
    }

    public static float CableMultiplier() {
        if (Upgrade.upgradeCablesMultiplier) {
            return 1f + 0.15f * (GenCable.Instance.owned - 1f);
        }
        else {
            return 1;
        }
    }
}
