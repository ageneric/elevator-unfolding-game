using UnityEngine;

public class BackBuilding : MonoBehaviour
{
    // Implements the building movement (based on height).
    public float floorHeight;
    public float imageHeight;
    public float offset;
    public float buildingTop;
    private Vector3 buildingPosition;

    private void Start() {
        buildingPosition = transform.position;
    }

    private void Update() {
        float relativeHeight;
        if (Game.Player.height / floorHeight > buildingTop) {
            relativeHeight = (-Game.Player.height + offset) / floorHeight + buildingTop;
        }
        else {
            relativeHeight = (-Game.Player.height % floorHeight + offset) / floorHeight;
        }

        if (Game.Player.height < -100f) {
            buildingPosition.y = -20f;
            transform.position = buildingPosition;
        }
        else if (buildingPosition.y != relativeHeight * imageHeight) {
            buildingPosition.y = relativeHeight * imageHeight;
            transform.position = buildingPosition;
        }
    }
}
