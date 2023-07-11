using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICurrency : MonoBehaviour
{
    public Text heightAmount;
    public Text rateAmount;
    public Text deepCurrencyAmount;
    public Text wingBonusAmount;
    public Text rocketBonusAmount;
    public Text tetherBonusAmount;
    public Text floorNameText;
    public Text speedSliderText;
    public Slider speedSlider;
    public GameObject gravityPanel;

    // Update is called once per frame
    void Update()
    {
        heightAmount.text = Helper.CurrencyReading(Game.Player.height);
        rateAmount.text = Helper.DeltaReading(Game.Player.rate);
        if (Game.Player.bonusGameSpeedTime > 0.66f)
        {
            heightAmount.text += " [+" + Helper.ShortHeight(Game.Player.bonusGameSpeedTime * Game.Player.rate) + "]";
            if (heightAmount.text.Length > 18)
            {
                heightAmount.text = Helper.CurrencyReading(Game.Player.height) + " [+"
                    + Helper.CostReading(Game.Player.bonusGameSpeedTime, "s") + "]";
            }
            rateAmount.text += " [x8]";
        }

        if (Game.Player.boreDepth != 0) {
            deepCurrencyAmount.text = Helper.CurrencyReading(Game.Player.boreDepth, "m");
        }
        wingBonusAmount.text = "+" + (Mathf.RoundToInt(Mathf.Pow(GenWing.Player.owned, GenTether.Player.TetherPower()) * 10) * 10).ToString() + "%";
        rocketBonusAmount.text = "x" + Helper.FormatValue(GenRocket.Player.RocketPower(),
            Helper.MathfRound, true);
        tetherBonusAmount.text = "^" + string.Format("{0:F2}", GenTether.Player.TetherPower());
        floorNameText.text = Game.Player.FloorName();

        if (Game.Player.height >= 100 || (!Game.Player.ascend && Game.Player.height > 0)) {
            if (!gravityPanel.activeSelf)
                gravityPanel.SetActive(true);
        }
        else if (gravityPanel.activeSelf) {
            gravityPanel.SetActive(false);
        }

        if (Game.Player.rate > 30000f) {
            speedSlider.value = Mathf.Clamp(Game.Player.rate / Helper.lightSpeed, 0f, 1f);
            speedSliderText.text = Helper.LongPercent(speedSlider.value, "% c");
        }
        else if (speedSlider.value > 0) {
            speedSlider.value = 0f;
            speedSliderText.text = "0% c";
        }
    }
}
