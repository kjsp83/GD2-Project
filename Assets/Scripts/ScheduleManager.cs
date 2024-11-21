using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using Ink.Runtime;

public class ScheduleManager : MonoBehaviour
{
    [SerializeField] private static ScheduleManager Instance;
    public UnityEvent<Destination> progressDay;
    public UnityEvent endDay;

    public enum Destination {
        Empty,
        RallyCenter,
        ESP,
        Apartment,
        Police,
        Mosaic
    }


    public Destination[] scheduleForToday;
    [SerializeField] private int currentIndex;
    public TMP_Dropdown[] dropdowns;

    public GameObject ScheduleCanvas;

    void Awake() {
        if (Instance == null) {
            Instance = this;
        }
        else {
            Destroy(gameObject);
        }
    }

    // Singleton that will contain the current schedule (that can be updated by the player through the menu)
    // and the time. Ink files for each day will be contained in their own respective scenes. 
    void Start()
    {
        if (progressDay == null) {
            progressDay = new UnityEvent<Destination>();
        }

        if (endDay == null) {
            endDay = new UnityEvent();
        }

        currentIndex = 0;
        scheduleForToday = new Destination[5];
    }

    public static ScheduleManager GetInstance() {
        return Instance;
    }

    private void Update() {
        if (Input.GetKeyDown("1")) {
            UpdateTime();
        }
    }

    public void ShowScheduleCanvas() {
        ScheduleCanvas.SetActive(true);
    }

    public string GetNextSceneName() {
        Destination d = scheduleForToday[currentIndex + 1];

        return d.ToString();
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

    public int GetCurrentTime() {
        return currentIndex;
    }

    public void UpdateScheduleValue(int i) {
        scheduleForToday[i + 1] = GetStringDestination(dropdowns[i].captionText.text);
    }

    private Destination GetStringDestination(string x) {
        switch (x) {
            case "Rally Center":
                return Destination.RallyCenter;
            case "ESP Radio":
                return Destination.ESP;
            case "Apartment":
                return Destination.Apartment;
            case "Police":
                return Destination.Police;
            case "Mosaic":
                return Destination.Mosaic;
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
        currentIndex++;

        if (currentIndex > scheduleForToday.Length)
            currentIndex = 0;
    }


}