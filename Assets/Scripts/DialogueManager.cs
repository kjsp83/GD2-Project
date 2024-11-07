using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;
using UnityEngine.Events;

public class DialogueManager : MonoBehaviour
{
    private static DialogueManager instance;

    public ScheduleManager sm;

    [Header("Dialogue UI")]

    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;
    private Story currentStory;
    private bool dialogueIsPlaying;

    [Header("Choices UI")]

    [SerializeField] private GameObject[] choices;
    private TextMeshProUGUI[] choicesText;

    [Header("Events")]

    public UnityEvent dialogueHasEnded;
    

    private void Awake() {
        if (instance != null) {
            Debug.LogWarning("Singleton aint single no more");
        }
        instance = this;
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
        return instance;
    }

    public void EnterDialogueMode(TextAsset inkJSON) {
        currentStory = new Story(inkJSON.text);
        dialogueIsPlaying = true;
        dialoguePanel.SetActive(true);
        currentStory.BindExternalFunction("pullUpComputer", () => {
            sm.ShowScheduleCanvas();
        });

        ContinueStory();
    }

    private void ExitDialogueMode() {
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
            }
        }
        else {
            ExitDialogueMode();
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
}
