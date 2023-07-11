using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class StatusPopup : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject popup;
    public Text popupText;

    // Start is called before the first frame update
    void Start()
    {
        popup.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData) {
        if (Game.Player.ascend && !Game.Player.pause) {
            popupText.text = "Flight, " + Helper.DeltaReading(Helper.Gravity()) + " by gravity";
        }
        else {
            popupText.text = "Freefall, " + Helper.DeltaReading(Helper.Gravity(), "m/s^2") + " gravity";
        }
        popup.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData) {
        popup.SetActive(false);
    }

    public void OnDisable() {
        popup.SetActive(false);
    }
}
