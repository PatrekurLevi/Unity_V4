using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonPlayerCharacter : MonoBehaviour
{
    public float displayTime = 4.0f; // 4 sekúndur á dialogue
    public GameObject dialogBox; // Breyta fyrir Dialogue
    float timerDisplay; // breyta fyrir tíma sem public

    void Start()
    {
        dialogBox.SetActive(false); // Setur dialogueið á false
        timerDisplay = -1.0f;
    }

    void Update()
    {
        if (timerDisplay >= 0) // Ef dialogueið er ekki uppi þá hleypir það því í gegn.
        {
            timerDisplay -= Time.deltaTime;
            if (timerDisplay < 0)
            {
                dialogBox.SetActive(false); // Ef dialogueið fer frá 4 sek niður í 0 setur það aftur í false og felur það.
            }
        }
    }

    public void DisplayDialog()
    {
        timerDisplay = displayTime; // Gefur dialogueinu 4 sekúndur.
        dialogBox.SetActive(true); // Setur dialogueið á true ef playerinn er í augnlínu miðað við raycast og smellir á 'X'.
    }
}