using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleScreenButton : MonoBehaviour
{
    [SerializeField] private string startScene;
    public Image fade;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void StartGame() {
        StartCoroutine(DoFade());
    }

     public IEnumerator DoFade()
    {
        // CanvasGroup canvasGroup = FindObjectOfType<CanvasGroup>();
        Camera camera = FindObjectOfType<Camera>();
        // canvasGroup.interactable = false;
        float t = 0.0f;
        float duration = 1.0f;
        var tempColor = fade.color;
        while (tempColor.a < 1f)
        {
            if (t < 1)
            {
                t += Time.deltaTime / duration;
            }
            tempColor.a += Time.deltaTime / 2;
            fade.color = tempColor;
            yield return null;
        }
        yield return null;
        SceneManager.LoadScene(startScene);
    }
}
