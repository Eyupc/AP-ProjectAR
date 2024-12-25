using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class StopStartCanvasHandler : MonoBehaviour
{

    public Action OnCloseClicked;
    void Start()
    {
    }

    public void OnCloseClick()
    {
        this.OnCloseClicked.Invoke();
        this.gameObject.SetActive(false);
    }

    void Update()
    {

    }
}
