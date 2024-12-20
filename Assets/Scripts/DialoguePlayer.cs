using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialoguePlayer : MonoBehaviour
{
    // Array of the conversations that the player can have at this location, in chronological order
    [SerializeField] private TextAsset[] sceneDialogues;

    private bool hasPlayed = false;
    [SerializeField] private bool playOnStart = false;

    // Start is called before the first frame update
    void Start()
    {
        if (playOnStart) {
            Debug.Log("go");
            DialogueManager.GetInstance().EnterDialogueMode(sceneDialogues[ScheduleManager.GetInstance().GetCurrentTime()]);
            hasPlayed = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space") && !hasPlayed) {
            Debug.Log("go");
            DialogueManager.GetInstance().EnterDialogueMode(sceneDialogues[ScheduleManager.GetInstance().GetCurrentTime()]);
            hasPlayed = true;
        }
    }


}
