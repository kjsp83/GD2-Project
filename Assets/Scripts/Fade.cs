using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fade : MonoBehaviour
{
    [SerializeField] private Animator fader;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void TriggerFade() {
        fader.SetTrigger("TriggerFade");
    }
}
