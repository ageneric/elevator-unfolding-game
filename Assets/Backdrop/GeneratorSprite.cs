using UnityEngine;

public class GeneratorSprite : MonoBehaviour
{
    public int generatorID;
    public GameObject sprite;

    private void Update() {
        if (Generator.GenOwnedById(generatorID) >= 1) {
            if (!sprite.activeSelf) {
                sprite.SetActive(true);
            }
        }
        else {
            sprite.SetActive(false);
        }
    }
}
