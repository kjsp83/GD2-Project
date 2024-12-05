using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    private SpriteRenderer bg;

    [SerializeField] private Sprite[] backgrounds;

    // Start is called before the first frame update
    void Start()
    {
        bg = GetComponent<SpriteRenderer>();
        bg.sprite = backgrounds[ScheduleManager.GetInstance().GetCurrentTime()];
    }

    public void UpdateBackground(ScheduleManager.Destination place) {
        bg.sprite = backgrounds[ScheduleManager.GetInstance().GetCurrentTime()];
    }
}
