using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageLog : MonoBehaviour
{
    public Text[] dialogueText;

    private List<string> currentMessages = new List<string>();
    private string lastMessage = "";
    private int timesRepeatedMessage = 0;


    public void AddMessage(string message) {
        // Debug.Log(message);
        // Count the number of consecutive repeated messages, editing the previous repeat.
        if (lastMessage == message) {
            timesRepeatedMessage++;
            message += " x" + (timesRepeatedMessage + 1).ToString();
            currentMessages[currentMessages.Count - 1] = message;
        }
        else {
            currentMessages.Add(message);
            timesRepeatedMessage = 0;
            lastMessage = message;
        }

        if (currentMessages.Count > dialogueText.Length) {
            currentMessages.RemoveAt(0);
        }
    }

    // Update is called once per frame
    void Update() {
        for (int i = 0; i < currentMessages.Count; i++) {
            dialogueText[i].text = currentMessages[i];
        }
    }
}
