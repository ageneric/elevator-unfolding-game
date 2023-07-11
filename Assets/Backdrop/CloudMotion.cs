using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudMotion : MonoBehaviour
{
    private float horizontalWrapInterval = 40f;
    public float height;
    private Vector3 m_position;
    private float relativeHeight;
    private bool seen = true;

    private void Start() {
        m_position = transform.position;
    }
    
    void Update()
    {   
        relativeHeight = Mathf.Clamp((height - Game.Player.height) / 25f, -30f, 30f);

        if ((relativeHeight > -27 && relativeHeight < 22)
                || (m_position.y >= -27 && m_position.y < 22)) {
            m_position.y = relativeHeight;
            m_position.x -= Time.deltaTime * Mathf.Sin(Time.time / 150f);
            // Wrap around (% not appropriate as it can return negative remainders)
            m_position.x = m_position.x > horizontalWrapInterval ? -horizontalWrapInterval : m_position.x;
            m_position.x = m_position.x < -horizontalWrapInterval ? horizontalWrapInterval : m_position.x;
            transform.position = m_position;
            seen = true;
        }
        else if (seen) {
            m_position.x = (Time.time + height % 81f) % (horizontalWrapInterval*2) - horizontalWrapInterval;
            transform.position = m_position;
            seen = false;
        }
    }
}
