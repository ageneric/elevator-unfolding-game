using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackSky : MonoBehaviour
{
    public GameObject currentSky;
    public GameObject transitionSky;
    public float scaleBy = 10f;

    public Color[] skyColors;
    public float[] transitionLines = new float[] { -10, 10, 20, 30, 60, 100, 500 };

    private Vector2 movePosition;
    private SpriteRenderer skySprite;
    private SpriteRenderer transitionSkySprite;

    private void Start() {
        movePosition = new Vector2(0, scaleBy);
        skySprite = currentSky.GetComponent<SpriteRenderer>();
        transitionSkySprite = transitionSky.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        float apparentHeight = Mathf.Sqrt(Mathf.Max(0, Game.Player.height));

        int lineIndex;
        for (lineIndex = 0; lineIndex < transitionLines.Length - 1; lineIndex++) {
            if (transitionLines[lineIndex] > apparentHeight) {
                break;
            }
        }

        if (lineIndex >= transitionLines.Length) {
            Debug.LogWarning("Index out of range."); 
            lineIndex = transitionLines.Length - 1;
        }
        float lineHeight = transitionLines[lineIndex];
        float proportion;

        if (lineHeight - apparentHeight < 5) {
            proportion = (lineHeight - apparentHeight) / 5;

            // Set the colours to the previous and new palettes.
            skySprite.color = skyColors[lineIndex];
            transitionSkySprite.color = skyColors[lineIndex + 1];
        }
        else {
            proportion = 1;

            skySprite.color = skyColors[lineIndex];
        }

        movePosition.y = scaleBy * proportion;
        transitionSky.transform.position = movePosition;
    }
}
