using UnityEngine;

public class Floor : MonoBehaviour
{
    public float layerHeight = 1f;  // parallax effect - apparent distance from the camera relative to foreground
    public float offset = 0f;
    private Vector3 m_position;
    private float relativeHeight;

    private void Start() {
        m_position = transform.position;
    }

    private void Update() {
        relativeHeight = Mathf.Min(10, -Game.Player.height / layerHeight + offset);

        if ((relativeHeight > -25 && relativeHeight < 20)
                || (m_position.y >= -25 && m_position.y < 20)) {
            m_position.y = relativeHeight;
            transform.position = m_position;
        }
    }
}
