using UnityEngine;

public class GeneratorAnim : MonoBehaviour
{
    // Implements the cable and rocket ("jet engine" in game) animations.
    public GameObject cable;
    private Vector3 startPosition;
    private Vector3 cablePosition;

    public GameObject rocketBase;
    public GameObject rocketAnimObject;
    private float stageScale;

    private void Start() {
        startPosition = cable.transform.position;
        cablePosition = startPosition;
        stageScale = 1.5f;
    }

    private void Update() {
        float deltaTime = Time.deltaTime;
        
        if (!Game.Player.ascend) {
            if (cable.transform.position.y < 20) {
                cablePosition.y += deltaTime * 3;
                cable.transform.position = cablePosition;
            }
        }
        else {
            cablePosition.y = startPosition.y;
            cable.transform.position = startPosition;
        }

        if (Game.Player.height >= 100 && !rocketAnimObject.activeSelf) {
            rocketAnimObject.SetActive(true);
        }
        else if (Game.Player.height < 100 && rocketAnimObject.activeSelf) {
            rocketAnimObject.SetActive(false);
        }
        
        if (rocketAnimObject.activeInHierarchy && stageScale < 2.1625f) {
            stageScale = Mathf.Clamp(stageScale + deltaTime, 1f, 2.1625f);
            rocketBase.transform.localScale = new Vector3(stageScale, 0.48f, 1f);
        }
    }
}
