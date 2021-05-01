using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackBuilding : MonoBehaviour
{
    public float floorHeight;
    public float imageHeight;
    public float offset;
    public float buildingTop;
    private Vector3 buildingPosition;

    private void Start() {
        buildingPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float relativeHeight;
        if (Game.Player.height / floorHeight > buildingTop) {
            relativeHeight = (-Game.Player.height + offset) / floorHeight + buildingTop;
        }
        else {
            relativeHeight = (-Game.Player.height % floorHeight + offset) / floorHeight;
        }

        if (Game.Player.height / floorHeight < buildingTop + 20) {
            buildingPosition.y = relativeHeight * imageHeight;
            transform.position = buildingPosition;
        }
    }
}
