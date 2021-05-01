using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonPurchase : MonoBehaviour
{
    public int genId;
    private Button button;
    public Text costText;
    public Text countText;

    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
    }

    // Update is called once per frame
    void Update()
    {
        costText.text = Helper.CostReading(Generator.GenCostById(genId));
        button.interactable = Generator.GenCanPurchase(genId);
        countText.text = Generator.GenOwnedById(genId).ToString();
    }
}
