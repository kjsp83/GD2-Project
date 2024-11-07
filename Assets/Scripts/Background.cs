using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    private SpriteRenderer bg;

    public Sprite[] backgrounds;
    public GameObject computerProp;

    // Start is called before the first frame update
    void Start()
    {
        bg = GetComponent<SpriteRenderer>();
    }

    public void UpdateBackground(ScheduleManager.Timeslot time, ScheduleManager.Destination place) {
        if (time == ScheduleManager.Timeslot.Planning) {
            computerProp.SetActive(true);
            bg.sprite = backgrounds[0];
        }
        else {
            computerProp.SetActive(false);
            bg.sprite = backgrounds[(int)place];
        }
    }
}
