using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

public class WebSave : MonoBehaviour
{
    // Flush changes to IndexDB by calling JS_Filesystem_Sync();
    [DllImport("__Internal")]
    private static extern void JS_FileSystem_Sync();
    private string dataPath;
    private GameRecord gameRecord;
    private float timeSinceSave;
    private bool enableSaving;
    private bool visible = true;

    public GameObject savePanel;
    public GameObject displayValue;
    public GameObject displaySave;
    public Text displayHeightSave;
    public Text displayAdditionalSave;
    public Text ignoreSaveButtonText;
    public Upgrade upgradeScript;
    public Unfolding unfoldingScript;
    public Fall fallScript;
    public Factory factoryScript;
    public GameObject saveIcon;

    public void ReadSave() {
        Debug.Log("r::" + dataPath);

        if (File.Exists(dataPath)) {
            string jsonString;
            try {
                jsonString = File.ReadAllText(dataPath);
                Debug.Log(jsonString);
            }
            catch {
                enableSaving = false;
                StartCoroutine(AnimateIcon());
                return;
            }
            gameRecord = Game.Player.GetRecordFromJSONString(jsonString);
            if (gameRecord != null) {
                timeSinceSave = -3600000f;
                savePanel.SetActive(true);
                if (gameRecord.height >= gameRecord.boreDepth && gameRecord.height < 999999) {
                    displayHeightSave.text = "Continue from " + Helper.ShortHeight(gameRecord.height);
                }
                else {
                    displayHeightSave.text = "Continue depth -" + Helper.ShortHeight(gameRecord.boreDepth);
                }
                displayAdditionalSave.text = Helper.CurrencyReading(gameRecord.height);
            }
        }
    }

    public void LoadSave() {
        Game.Player.LoadRecord(gameRecord);
        upgradeScript.Load();
        unfoldingScript.Load();
        fallScript.Load();
        factoryScript.Load(gameRecord);
        timeSinceSave = 60f;
    }

    public void IgnoreSave() {
        timeSinceSave += 1200010f;
        ignoreSaveButtonText.text = "Confirm?";
    }

    public void PreviewSave(bool state) {
        displayValue.SetActive(!state);
        displaySave.SetActive(state);
    }

    public void WriteSave() {
        Debug.Log("w::" + dataPath);
        
        FileStream stream = File.Open(dataPath, FileMode.Create);
        StreamWriter writer = new StreamWriter(stream);
        string writeString = Game.Player.GetJSONString(factoryScript.cableMachines);
        Debug.Log(writeString);
        writer.Write(writeString);
        writer.Close();
        
        try {
            JS_FileSystem_Sync();
        }
        catch {}

        if (!File.Exists(dataPath)) {
            enableSaving = false;
        }
        StartCoroutine(AnimateIcon());
    }

    void Awake()
    {
        timeSinceSave = 15f;
        enableSaving = true;
        dataPath = Application.persistentDataPath + "/test.json";
        displayValue.SetActive(false);
        ReadSave();
    }

    void Update()
    {
        if (enableSaving) {
            if (timeSinceSave > 15f) {
                timeSinceSave = 0f;
                if (visible) {
                    visible = false;
                    savePanel.SetActive(false);
                    PreviewSave(false);
                }
                WriteSave();
            }
            else {
                timeSinceSave += Time.deltaTime;
            }
        }
    }

    IEnumerator AnimateIcon()
    {
        saveIcon.SetActive(true);
        if (!enableSaving) {
            for (int i=0; i<4; i++) {
                saveIcon.GetComponent<Image>().color = new Color(1f, 0f, 0f, 1f);
                yield return new WaitForSeconds(1);
                saveIcon.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
                yield return new WaitForSeconds(1);
            }
            saveIcon.GetComponent<Image>().color = new Color(1f, 0f, 0f, 1f);
        }
        yield return new WaitForSeconds(1);
        saveIcon.SetActive(false);
    }
}
