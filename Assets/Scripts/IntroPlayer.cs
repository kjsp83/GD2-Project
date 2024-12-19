using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class IntroPlayer : MonoBehaviour
{
    public GameObject journalButton;
    public GameObject videoObject;
    public VideoPlayer videoPlayer;

    // Start is called before the first frame update
    void Start()
    {
        videoPlayer.isLooping = false;
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (( videoPlayer.frame) > 0 && (videoPlayer.isPlaying == false)) {
            journalButton.SetActive(true);
            videoObject.SetActive(false);
        }
        */
    }
}
