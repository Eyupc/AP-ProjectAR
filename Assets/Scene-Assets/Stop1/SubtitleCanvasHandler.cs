using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubtitleCanvasHandler : MonoBehaviour
{
    public Action OnCloseClicked;
    private GameObject avatar;
    private MonoBehaviour scriptToActivate;

    void Start()
    {
        avatar = GameObject.FindWithTag("Stop1_Player");
        if (avatar != null)
        {
            scriptToActivate = avatar.GetComponent<PlayPoem>();
            if (scriptToActivate != null)
            {
                scriptToActivate.enabled = false;
            }
            else 
            {
                Debug.LogError("Poem script not found.");
            }
        }
        else
        {
            Debug.LogError("Avatar instance not found in the scene.");
        }
    }

    public void OnCloseClick()
    {
        Debug.Log(scriptToActivate);
        Debug.Log(avatar.gameObject.name);
        if (scriptToActivate != null)
        {
            Debug.Log("the script will now be executed");
            scriptToActivate.enabled = true;
        }
        else {
            Debug.Log("the script cannot be executed");
        }
        //this.OnCloseClicked.Invoke();
        this.gameObject.SetActive(false);
    }

    void Update()
    {

    }
}
