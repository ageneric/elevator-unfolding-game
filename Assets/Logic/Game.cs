using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public static Game Player { get; private set; }

    public float height;
    public float rate;
    public bool ascend;
    public bool pause;

    public float maxRunHeight;
    public float fallTime;
    public float deepCurrency;

    public List<string> upgrades;

    [RuntimeInitializeOnLoadMethod]
    public void Awake() {
        if (Player is null) {
            Player = this;
            SetStartGame();
        }
        else {
            Debug.LogWarning("Attempted to create multiple Main instances.");
        }
    }

    public string FloorName() {
        if (height >= 70000) { return "Orbit"; }
        else if (height >= 8000) { return "Thermosphere"; }
        else if (height >= 5000) { return "Mesosphere"; }
        else if (height >= 1200) { return "Stratosphere"; }
        else if (height >= 400) { return "Atmosphere"; }

        else if (height >= 105) {
            return "Balcony";
        }
        else if (height >= 5) {
            int floorNumber = Mathf.FloorToInt(height / 5);
            return floorNumber.ToString();
        }
        else if (height <= -5) {
            return "Basement";
        }
        else if (height <= -50) {
            return "Secret Lab";
        }
        else {
            return "Ground";
        }
    }

    public void SetStartGame() {
        upgrades = new List<string>();
        deepCurrency = 0;
        SetStartRun();
    }

    public void SetStartRun() {
        ascend = true;
        pause = false;
        height = 0;
        rate = 0;
        maxRunHeight = 0;

        GenCable.Instance.owned = 0;
        GenRocket.Instance.owned = 0;
        GenWing.Instance.owned = 0;
    }

    public void SetStartFall() {
        ascend = false;
        pause = false;
        fallTime = Time.time;
    }
}
