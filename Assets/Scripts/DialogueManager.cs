using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;
using UnityEngine.Events;

public class DialogueManager : MonoBehaviour
{
    private static DialogueManager Instance;

    public ScheduleManager sm;

    private DialogueVariables dialogueVariables;

    private const string SPEAKER_TAG = "Speaker";
    private const string CLUE_TAG = "clue";

    /*
    private const string PORTRAIT_TAG = "portrait";

    private const string LAYOUT_TAG = "layout";
    */

    // variable for the load_globals.ink JSON
    [Header("Load Globals JSON")]
    [SerializeField] private TextAsset loadGlobalsJSON;

    [Header("Dialogue UI")]

    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private TextMeshProUGUI displayNameText;
    private Story currentStory;
    private bool dialogueIsPlaying;

    [Header("Choices UI")]

    [SerializeField] private GameObject[] choices;
    private TextMeshProUGUI[] choicesText;

    [Header("Events")]

    public UnityEvent dialogueHasEnded;
    

    private void Awake() {
        if (Instance != null) {
            Debug.LogWarning("Singleton aint single no more");
        }
        Instance = this;

         dialogueVariables = new DialogueVariables(loadGlobalsJSON);
    }

    private void Start() {
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);

        choicesText = new TextMeshProUGUI[choices.Length];
        int index = 0;
        foreach(GameObject choice in choices) {
            choicesText[index] = choice.GetComponentInChildren<TextMeshProUGUI>();
            index++;
        }

        if (dialogueHasEnded == null)
            dialogueHasEnded = new UnityEvent();
    }

    private void Update() {
        if (!dialogueIsPlaying)
            return;
        
        if (currentStory.currentChoices.Count == 0 && Input.GetMouseButtonDown(0)) {
            ContinueStory();
        }
    }

    public static DialogueManager GetInstance() {
        return Instance;
    }

    public void EnterDialogueMode(TextAsset inkJSON) {
        currentStory = new Story(inkJSON.text);
        dialogueIsPlaying = true;
        dialoguePanel.SetActive(true);

        displayNameText.text = "";

        dialogueVariables.StartListening(currentStory);

        currentStory.BindExternalFunction("pullUpComputer", () => {
            sm.ShowScheduleCanvas();
        });

        ContinueStory();
    }

    private void ExitDialogueMode() {

        dialogueVariables.StopListening(currentStory);

        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        dialogueText.text = "";
        dialogueHasEnded.Invoke();
    }

    private void ContinueStory() {
        if (currentStory.canContinue) {
            string nextLine = currentStory.Continue();

            if (nextLine.Equals("") && !currentStory.canContinue) {
                ExitDialogueMode();
            }
            else {
                dialogueText.text = nextLine;
                DisplayChoices();

                HandleTags(currentStory.currentTags);
            }
        }
        else {
            ExitDialogueMode();
        }
    }

    private void HandleTags(List<string> currentTags) {
        foreach (string tag in currentTags) {
            string[] splitTag = tag.Split(':');
            if (splitTag.Length != 2) {
                Debug.LogError("Tag could not be appropriately parsed: " +tag);
            }
            string tagKey = splitTag[0].Trim();
            string tagValue = splitTag[1].Trim();


            // handles tags
            // maybe add a tag for clues, that calls a method in schedulemanager to toggle them in the notebook
            switch (tagKey) {  
                case SPEAKER_TAG:
                    if (tagValue == "na") { // if the speaker is listed as NA, remove the profile image and set the display name to nothing.
                        displayNameText.text = "";
                        break;
                    }

                    displayNameText.text = tagValue;
                    break;
                case CLUE_TAG: // Calls method to add clue to notes in ScheduleManager
                    ScheduleManager.GetInstance().AddClueToNotes(tagValue);
                    break;
                default:
                    Debug.LogWarning("Tag came in but is not being handled: " +tag);
                    break;
            }
        }
    }

    private void DisplayChoices() {
        List<Choice> currentChoices = currentStory.currentChoices;

        if (currentChoices.Count > choices.Length) {
            Debug.LogError("More choices than the UI could handle");
        }

        int index = 0;
        foreach(Choice choice in currentChoices) {
            choices[index].gameObject.SetActive(true);
            choicesText[index].text = choice.text;
            index++;
        }

        for (int i = index; i < choices.Length; i++) {
            choices[i].gameObject.SetActive(false);
        }

    }

    public void MakeChoice(int choiceIndex) {
        Debug.Log(choiceIndex);
        currentStory.ChooseChoiceIndex(choiceIndex);
        ContinueStory();
    }

    public Ink.Runtime.Object GetVariableState(string variableName) {
        Ink.Runtime.Object variableValue = null;
        dialogueVariables.variables.TryGetValue(variableName, out variableValue);

        if (variableValue == null) {
            Debug.LogWarning("Ink variable was found to be null: " +variableName);
        }
        return variableValue;
    }
}
