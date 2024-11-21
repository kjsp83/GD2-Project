using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BinderController : MonoBehaviour
{
    [SerializeField] private Animator binderAnim;
    [SerializeField] private bool isOpen = false;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void triggerBinder() {
        if (isOpen) {
            binderAnim.SetBool("binderOpen", false);
            isOpen = false;
        }
        else {
            binderAnim.SetBool("binderOpen", true);
            isOpen = true;
        }
    }

    public void CloseBinder() {
        binderAnim.SetBool("binderOpen", false);
        isOpen = false;
    }
}
