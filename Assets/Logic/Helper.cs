using UnityEngine;
using System;

public static class Helper
{
    const float naturalDisplayLimit = 10 * 1000000f;
    public const float planetRadius = 6370000f;
    public const float lightSpeed = 299792458f;

    public static string CurrencyReading(float amount, string unit = "m") {
        if (amount >= 0) {
            return FormatValue(amount, MathfFloor) + unit;
        }
        else {
            return "-" + FormatValue(Mathf.Abs(amount), MathfFloor) + unit;
        }
    }

    public static string CostReading(float amount, string unit = "m") {
        if (amount >= 0) {
            return FormatValue(amount, MathfCeil) + unit;
        }
        else {
            return "-" + FormatValue(Mathf.Abs(amount), MathfCeil) + unit;
        }
    }

    public static string DeltaReading(float rate, string unit = "m/s") {
        if (rate > 0) {
            return "+" + FormatValue(rate, MathfRound, true) + unit;
        }
        else if (rate < 0) {
            return "-" + FormatValue(Mathf.Abs(rate), MathfRound, true) + unit;
        }
        else {
            return "";
        }
    }

    public static string ShortHeight(float amount) {
        return FormatValue(amount, MathfFloor, false, 10000) + "m";
    }

    public static string LongPercent(float amount, string unit = "%")
    {
        return FormatValue(amount * 100, MathfFloor, true) + unit;
    }

    public static string FormatValue(float positive, Func<float, float> roundBias,
                                     bool decimalPlaces = false, float displayLimit = naturalDisplayLimit, float matchUnit = 0f) {
        if (positive <= 0.95 && decimalPlaces) {
            float roundAmount = roundBias(positive * 100) / 100;
            return string.Format("{0:F2}", roundAmount);
        }
        if (positive <= 9.95 && decimalPlaces) {
            float roundAmount = roundBias(positive * 10) / 10;
            return string.Format("{0:F1}", roundAmount);
        }
        else if (positive < displayLimit && matchUnit < displayLimit) {
            return roundBias(positive).ToString();
        }
        else if (positive < displayLimit * 100 && matchUnit < displayLimit) {
            return roundBias(positive/1000f).ToString() + "k";
        }
        else {
            int exponent = Mathf.FloorToInt(Mathf.Log10(positive));
            float mantissa = positive / Mathf.Pow(10, exponent);
            return string.Format("{0:F3}^E{1}", roundBias(mantissa * 1000) / 1000, exponent);
        }
    }

    // Fix WebGL build not supporting Func<float, float> properly.
    public static float MathfFloor(float f) { return Mathf.Floor(f); }
    public static float MathfCeil(float f) { return Mathf.Ceil(f); }
    public static float MathfRound(float f) { return Mathf.Round(f); }


    public static float Gravity() {
        float d = 1f + Game.Player.height / planetRadius;
        if (Mathf.Abs(d) < 0.98f) d = 0.98f;
        return -9.8f / d;
    }

    public static float CableMultiplier() {
        if (Upgrade.upgradeCablesMultiplier) {
            return 1f + 0.15f * (GenCable.Player.owned - 1f);
        }
        else {
            return 1;
        }
    }
}
