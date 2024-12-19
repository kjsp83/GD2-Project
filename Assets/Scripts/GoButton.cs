using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoButton : MonoBehaviour
{

    public BinderController binder;


    public void ButtonPress() {
        StartCoroutine(ButtonEffects());
    }

    IEnumerator ButtonEffects() {
        binder.triggerBinder();

        yield return new WaitForSeconds(0.5f);

        SceneChanger.GetInstance().LoadNextScene();
    }
}
