using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThereCanBeOnlyOne : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Triggered by the scene change event to prevent errors (i.e. there cant be 2 event systems)
    public void SilenceMe() {
        gameObject.SetActive(false);
    }
}
