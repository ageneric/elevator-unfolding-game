using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour
{
    public float layerHeight;
    public float offset = 0f;
    private Vector3 m_position;
    private float relativeHeight;

    private void Start() {
        m_position = transform.position;
    }

    private void Update() {
        relativeHeight = Mathf.Min(10, -Game.Player.height / layerHeight + offset);

        if ((relativeHeight > -20 && relativeHeight < 10) || (m_position.y > -20 && m_position.y < 10)) {
            m_position.y = relativeHeight;
            transform.position = m_position;
        }
    }
}
