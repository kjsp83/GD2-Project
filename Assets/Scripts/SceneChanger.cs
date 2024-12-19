using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    private static SceneChanger Instance;

    [SerializeField] private string currentScene;

    [Header("Fade")]
    [SerializeField] private Animator fader;


    [Header("Scene Change Events")]
    public UnityEvent m_BeginSceneChangeEvent;
    public UnityEvent m_FinishSceneChangeEvent;

    // Start is called before the first frame update
    void Awake()
    {
        if (Instance == null) {
            Instance = this;
        }
        else {
            Destroy(gameObject);
        }

        if (m_BeginSceneChangeEvent == null) {
            m_BeginSceneChangeEvent = new UnityEvent();
        }

        if (m_FinishSceneChangeEvent == null) {
            m_FinishSceneChangeEvent = new UnityEvent();
        }
    }


    public static SceneChanger GetInstance() {
        return Instance;
    }

    public void LoadNextScene() {
        string name = ScheduleManager.GetInstance().GetNextSceneName();
        if (name != "CHOOSE") {
            Debug.Log(name);
            ScheduleManager.GetInstance().DisableOldTimeslot();
            StartCoroutine(LoadNextSceneAsync(name));
        }
        else {
            Debug.LogError("Invalid Scene Name");
        }
    }

    /** 
        Loads the next scene additively, waits until the load is finished, and then
        unloads the previous scene
    
        @param n Name of the next scene to load
    **/
    IEnumerator LoadNextSceneAsync(string n) {
        m_BeginSceneChangeEvent.Invoke();

        yield return new WaitForSeconds(1.5f);

        // Leftover code for audio stuff from the project I pulled this from, may use later
        /*
        StartCoroutine(FadeMixerGroup.StartFade(audioFader, "MusicVolume", audioFadeDuration, 0f)); // Fade out sfx, music, ambience
        // StartCoroutine(FadeMixerGroup.StartFade(audioFader, "SFXVolume", audioFadeDuration, 0f));
        StartCoroutine(FadeMixerGroup.StartFade(audioFader, "AmbienceVolume", audioFadeDuration, 0f));
        */

        // if (!initialSkip) yield return new WaitForSeconds(1.5f);


        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(n, LoadSceneMode.Additive);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        if (currentScene != "-")
            SceneManager.UnloadSceneAsync(currentScene);
        
        currentScene = n;
        Debug.Log("Scene Loaded: " +currentScene);
        // m_sceneChangeEvent.Invoke(n);

        // Debug.Log("MapController: Fading in Music, SFX and Ambience");
        // Start coroutines to fade in sfx and ambience but NOT music
        // Music fade in will be handled by the BaseSceneManagers for character scenes
        // StartCoroutine(FadeMixerGroup.StartFade(audioFader, "SFXVolume", audioFadeDuration, 1f));
        // StartCoroutine(FadeMixerGroup.StartFade(audioFader, "AmbienceVolume", audioFadeDuration, 1f));
        m_FinishSceneChangeEvent.Invoke();
    }
}
