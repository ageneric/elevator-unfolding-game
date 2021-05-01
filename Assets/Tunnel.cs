using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tunnel : MonoBehaviour
{
    public float layerHeight;
    public float offset = 0f;
    private Vector3 m_scale;
    private float relativeHeight;

    private void Start() {
        m_scale = transform.localScale;
    }

    private void Update() {
        relativeHeight = -Game.Player.height / layerHeight + offset;

        if (relativeHeight > 0) {
            m_scale.y = relativeHeight;
            transform.localScale = m_scale;
        }
        else {
            m_scale.y = 0f;
            transform.localScale = m_scale;
        }
    }
}
