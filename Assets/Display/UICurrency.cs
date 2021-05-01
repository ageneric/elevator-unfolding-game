using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICurrency : MonoBehaviour
{
    public Text heightAmount;
    public Text rateAmount;
    public Text deepCurrencyAmount;
    public Text floorNameText;

    // Update is called once per frame
    void Update()
    {
        heightAmount.text = Helper.CurrencyReading(Game.Player.height);
        rateAmount.text = Helper.DeltaReading(Game.Player.rate);

        if (Game.Player.deepCurrency != 0) {
            deepCurrencyAmount.text = Helper.CostReading(Game.Player.deepCurrency, "m");
        }
        floorNameText.text = Game.Player.FloorName();
    }
}
