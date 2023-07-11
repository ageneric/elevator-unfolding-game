using System;
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
    public float boreDepth;
    public float lostRunHeight;

    public double saveTime;
    public float bonusGameSpeedTime;

    public List<string> upgrades;
    public List<int> savedResourceInts;

    public bool noRelativityEffect;

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
        if (height >= 14062199845800) { return "Heliosphere"; }
        else if (height >= 152091867000) { return "Solar System"; }
        else if (height >= 600000000 && Mathf.Abs(rate) >= Helper.lightSpeed) { return "Hyperspace"; }
        else if (height >= 363104000) { return "Lunar Orbit"; }
        else if (height >= 100000000) { return "Deepest Spc."; }
        else if (height >= 10000000) { return "Deeper Space"; }
        else if (height >= 700000) { return "Deep Space"; }
        else if (height >= 100000) { return "Satellite"; }
        else if (height >= 80000) { return "Thermosphere"; }
        else if (height >= 50000) { return "Mesosphere"; }
        else if (height >= 12000) { return "Stratosphere"; }
        else if (height >= 1000) { return "Troposphere"; }
        else if (height >= 100) { return "Balcony"; }
        else if (height >= 5) {
            int floorNumber = Mathf.FloorToInt(height / 5);
            return floorNumber.ToString();
        }
        else if (height <= -Helper.planetRadius) { return "Other Side"; }
        else if (height <= -10000) { return "The Mantle"; }
        else if (height <= -1000) { return "Dark Tunnel"; }
        else if (height <= -100) { return "The Tunnel"; }
        else if (height <= -50) { return "Secret Lab"; }
        else if (height <= -5) { return "Basement"; }
        else {
            return "Ground";
        }
    }

    public void SetStartGame() {
        upgrades = new List<string>();
        savedResourceInts = new List<int>();
        boreDepth = 0;
        noRelativityEffect = false;
        SetStartRun();
    }

    public void SetStartRun() {
        ascend = true;
        pause = false;
        height = 0;
        rate = 0;
        maxRunHeight = 0;
        lostRunHeight = 0;

        GenCable.Player.owned = 0;
        GenRocket.Player.owned = 0;
        GenWing.Player.owned = 0;
    }

    public void SetStartFall() {
        ascend = false;
        pause = false;
        fallTime = Time.time;
    }

    public string GetJSONString(int factoryGenericInt) {  // parameter: slot for private data used in Factory.cs
        try {
            DateTime time = DateTime.Now;
            Game.Player.saveTime = (time - DateTime.MinValue).TotalMinutes;
        }
        catch {}

        savedResourceInts = new List<int>() {
            GenCable.Player.owned, GenWing.Player.owned, GenRocket.Player.owned, GenTether.Player.owned,
            // Pack upgrades into naive BitArray.
            Convert.ToInt32(Upgrade.automatorCable) + (Convert.ToInt32(Upgrade.automatorFactory) << 1) + (Convert.ToInt32(Automator.automatorEnabled) << 2),
            Upgrade.levelRocketsMultiplier, factoryGenericInt
        };
        return JsonUtility.ToJson(this);
    }

    public GameRecord GetRecordFromJSONString(string jsonString) {
        try {
            return JsonUtility.FromJson<GameRecord>(jsonString);
        }
        catch {
            Debug.LogWarning("Failed to deserialise JSON: " + jsonString);
            return null;
        }
    }

    public void LoadRecord(GameRecord record) {
        height = record.height;
        rate = record.rate;
        ascend = record.ascend;
        pause = record.pause;
        maxRunHeight = record.maxRunHeight;
        boreDepth = record.boreDepth;
        lostRunHeight = record.lostRunHeight;
        noRelativityEffect = record.noRelativityEffect;

        fallTime = Time.time - record.fallTime;  // Reset the zero for Time.time

        try {
            // Unpack savedResourceInts
            GenCable.Player.owned = record.savedResourceInts[0];
            GenWing.Player.owned = record.savedResourceInts[1];
            GenRocket.Player.owned = record.savedResourceInts[2];
            GenTether.Player.owned = record.savedResourceInts[3];
            // Unpack upgrades (naive BitArray)
            Upgrade.automatorCable = (record.savedResourceInts[4] % 2 == 1);
            Upgrade.automatorFactory = (record.savedResourceInts[4] % 4 >= 2);
            // Automator.automatorEnabled = (record.savedResourceInts[4] % 8 >= 4);
            Upgrade.levelRocketsMultiplier = record.savedResourceInts[5];
        }
        catch {}

        // Enable a boosted production interval for returning players
        try {
            double timeSinceSave = (DateTime.Now - DateTime.MinValue).TotalMinutes - record.saveTime;
            if (record.rate > 0f && height > 0.01f && ascend && timeSinceSave > 0f) {
                bonusGameSpeedTime += 5f;
                if (record.rate > 1f)
                {
                    bonusGameSpeedTime += 5f * Mathf.Round(Mathf.Min((float)timeSinceSave, Mathf.Log10(Mathf.Max(1f, maxRunHeight))));
                }
            }
        }
        catch {
            if (record.rate > 0f && height > 0.01f && ascend)
            {
                bonusGameSpeedTime += 5f;
                if (record.rate >= 1f && maxRunHeight >= 99999f)
                {
                    bonusGameSpeedTime += 25f;
                }
            }
        }
    }
}

[System.Serializable]
public class GameRecord {
    public float height;
    public float rate;
    public bool ascend;
    public bool pause;

    public float maxRunHeight;
    public float fallTime;
    public float boreDepth;
    public float lostRunHeight;

    public float bonusGameSpeedTime;
    public double saveTime;

    public List<string> upgrades;
    public List<int> savedResourceInts;
    public bool noRelativityEffect;
}