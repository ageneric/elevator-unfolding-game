using UnityEngine;
using UnityEngine.UI;

public class Factory : MonoBehaviour
{
    private float nextThreshold = 100f;
    private float lastThreshold = 100f;
    public float baseThreshold = 100f;
    public float cableMachineCost = 150f;
    public int cableMachines = 0;
    public Button maxCableMachine;
    public Text costCableMachine;
    public Text countCableMachine;
    public Text spaceUsed;
    public Slider cableSlider;
    public Text cableProgress;
    public Text sliderFillCount;

    // Start is called before the first frame update
    void Start()
    {
        cableSlider.gameObject.SetActive(false);
        maxCableMachine.interactable = false;
        if (cableMachines > 99) {
            maxCableMachine.GetComponentInChildren<Text>().text = "Spinner";
        }
    }

    public void Load(GameRecord gameRecord) {
        if (gameRecord.savedResourceInts.Count >= 7) {
            BuyCableMachine(gameRecord.savedResourceInts[6]);
        }
        if (cableMachines > 99) {
            maxCableMachine.GetComponentInChildren<Text>().text = "Spinner";
        }
    }

    // Update is called once per frame
    private void Update() {
        costCableMachine.text = Helper.CurrencyReading(Game.Player.boreDepth, "/") + Helper.CostReading(cableMachineCost * (cableMachines + 1));
        if (Game.Player.height >= 0 && cableMachines > 0) {
            cableProgress.text = Helper.CurrencyReading(Game.Player.lostRunHeight, "/") + Helper.CostReading(nextThreshold);
            cableSlider.value = Game.Player.lostRunHeight;
        }
        if (Game.Player.boreDepth > cableMachineCost * (cableMachines + 1) && !maxCableMachine.interactable) {
            maxCableMachine.interactable = true;
        }
    }

    void FixedUpdate()
    {
        if (cableMachines > 0) {
            if (Game.Player.height >= 0 && Game.Player.lostRunHeight >= nextThreshold) {
                GenCable.Player.factoryCableCount += 1;
                GenCable.Player.owned += 1;
                lastThreshold = nextThreshold;
                nextThreshold = CalculateThreshold(GenCable.Player.factoryCableCount);
                DisplayThreshold();
            }
            if (GenCable.Player.factoryCableCount > 0 && Game.Player.height < 0) {
                ResetItems();
            }
        }
    }

    private float CalculateThreshold(int cables) {
        if (cables < 0) {
            return 0f;
        }
        else {
            return baseThreshold * Mathf.Pow(1.7f + 2.5f*Mathf.Pow(0.75f, cableMachines - 1) + 1.5f*Mathf.Pow(0.95f, cableMachines - 1)
                                             + 0.3f/(1 + Mathf.Log(1 + Mathf.Max(0, cableMachines/100))),
                                   cables);
        }
    }

    private void DisplayThreshold() {
        cableSlider.minValue = lastThreshold;
        cableSlider.maxValue = nextThreshold;
        sliderFillCount.text = "+" + GenCable.Player.factoryCableCount.ToString();
    }

    private void ResetItems() {
        GenCable.Player.owned -= GenCable.Player.factoryCableCount;
        GenCable.Player.factoryCableCount = 0;
        lastThreshold = 0f;
        nextThreshold = baseThreshold;
        DisplayThreshold();
    }

    public void BuyCableMachine(int max=1000000) {
        Debug.Log(max);
        if (Game.Player.boreDepth > cableMachineCost * (cableMachines + 1)) {
            if (cableMachines == 0) {
                cableSlider.gameObject.SetActive(true);
            }
            cableMachines = Mathf.Min((int)Game.Player.boreDepth / (int)cableMachineCost, max);
            lastThreshold = CalculateThreshold(GenCable.Player.factoryCableCount - 1);
            nextThreshold = CalculateThreshold(GenCable.Player.factoryCableCount);
            DisplayThreshold();
            maxCableMachine.interactable = Game.Player.boreDepth > (cableMachines + 1) * cableMachineCost;
            spaceUsed.text = Helper.CostReading(cableMachineCost * cableMachines);
            countCableMachine.text = cableMachines.ToString();
            if (cableMachines > 99) {
                maxCableMachine.GetComponentInChildren<Text>().text = "Spinner";
            }
        }
    }
}
