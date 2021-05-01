using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackCable : MonoBehaviour
{
    public GameObject cable;
    private Vector3 startPosition;
    private Vector3 cablePosition;

    private void Start() {
        startPosition = cable.transform.position;
        cablePosition = startPosition;
    }

    private void Update() {
        if (Generator.GenOwnedById(1) >= 1) {
            if (!cable.activeSelf) {
                cable.SetActive(true);
            }
        }
        else {
            cable.SetActive(false);
        }

        if (!Game.Player.ascend) {
            if (cable.transform.position.y < 20) {
                cablePosition.y = cablePosition.y + Time.deltaTime * 3;
                cable.transform.position = cablePosition;
            }
        }
        else {
            cable.transform.position = startPosition;
        }
    }
}
