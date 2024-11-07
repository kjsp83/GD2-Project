using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using Ink.Runtime;

public class ScheduleManager : MonoBehaviour
{
    public UnityEvent<Timeslot, Destination> progressDay;
    public UnityEvent endDay;

    public int day;
    public enum Timeslot {
        Planning,
        Morning,
        Noon,
        Afternoon,
        Evening,
        Count
    }

    public enum Destination {
        Empty,
        RallyCenter,
        Radio,
        Apartment
    }

    public Timeslot currentTime;

    public Destination[] scheduleForToday;
    private int currentIndex;
    public TMP_Dropdown[] dropdowns;
    public GameManager manager;

    public Animator fader;
    public Animator playerAnimator;

    public GameObject ScheduleCanvas;

    [Header("Timeslot Stuff")]

    [SerializeField] private TextAsset introJSON;
    [SerializeField] private TextAsset[] morningJSONS;
    [SerializeField] private TextAsset noonJSON;
    [SerializeField] private TextAsset afternoonJSON;
    [SerializeField] private TextAsset eveningJSON;

    // Start is called before the first frame update
    void Start()
    {
        if (progressDay == null) {
            progressDay = new UnityEvent<Timeslot, Destination>();
        }

        if (endDay == null) {
            endDay = new UnityEvent();
        }

        currentIndex = 0;
        day = 0;
        scheduleForToday = new Destination[4];

        DialogueManager.GetInstance().EnterDialogueMode(introJSON);
    }

    public void ShowScheduleCanvas() {
        ScheduleCanvas.SetActive(true);
    }

    public void AttemptToProgressDay() {
        if (currentTime == Timeslot.Planning) {
            foreach(Destination d in scheduleForToday) {
                if (d == Destination.Empty)
                return;
            }

            ScheduleCanvas.SetActive(false);
            UpdateTime();

            StartCoroutine(ProgressDayCoroutine());
        }
        else if (currentTime == Timeslot.Evening) {
            currentIndex = 0;
            currentTime = Timeslot.Planning;
            ClearSchedule();

            StartCoroutine(ProgressDayCoroutine());
        }
        else {
            currentIndex++;
            UpdateTime();
            Debug.Log(currentIndex);
            StartCoroutine(ProgressDayCoroutine());
        }
    }

    private IEnumerator ProgressDayCoroutine() {
        Debug.Log("A");
        playerAnimator.SetTrigger("WalkOut");
        yield return new WaitForSeconds(1f);

        fader.SetTrigger("TriggerFade");
        yield return new WaitForSeconds(1f);

        progressDay.Invoke(currentTime, scheduleForToday[currentIndex]);

        yield return new WaitForSeconds(1f);

        fader.SetTrigger("TriggerFade");

        yield return new WaitForSeconds(1f);
        
        playerAnimator.SetTrigger("WalkIn");

        yield return new WaitForSeconds(1f);

        if (currentIndex != 0 || (day == 0 && currentIndex == 0))
            BeginInteraction();
        else {
            ScheduleCanvas.SetActive(true);
            day++;
        }
    }

    private void BeginInteraction() {
        Debug.Log(currentIndex);
        switch (currentTime) {
            case Timeslot.Morning:
                Debug.Log((int)scheduleForToday[currentIndex - 1]);
                TextAsset morningQ = morningJSONS[(int)scheduleForToday[currentIndex]];
                DialogueManager.GetInstance().EnterDialogueMode(morningQ);
                break;
            case Timeslot.Noon:
                DialogueManager.GetInstance().EnterDialogueMode(noonJSON);
                break;
            case Timeslot.Afternoon:
                DialogueManager.GetInstance().EnterDialogueMode(afternoonJSON);
                break;
            case Timeslot.Evening:
                DialogueManager.GetInstance().EnterDialogueMode(eveningJSON);
                break;
        }
    }

    /*
    public void FillTimeslot(string timeAndChoice) {
        string[] split = timeAndChoice.Split (","[0]);
        int t = int.Parse(split[0]);
        string choice = split[1];

        Destination fill = (Destination) Destination.Parse(typeof(Destination), choice);
        scheduleForToday[t] = fill;
    }
    */

    public void UpdateScheduleValue(int i) {
        scheduleForToday[i] = GetStringDestination(dropdowns[i].captionText.text);
    }

    private Destination GetStringDestination(string x) {
        switch (x) {
            case "Rally Center":
                return Destination.RallyCenter;
            case "ESP Radio":
                return Destination.Radio;
            case "Apartment":
                return Destination.Apartment;
            default:
                return Destination.Empty;
        }
    }

    public void ClearSchedule() {
        for (int i = 0; i < scheduleForToday.Length; i++) {
            scheduleForToday[i] = Destination.Empty;
        }
    }

    public void UpdateTime() {
        int x = (int)currentTime;
        x = (x + 1) % 5;
        currentTime = (Timeslot)x;
    }


}