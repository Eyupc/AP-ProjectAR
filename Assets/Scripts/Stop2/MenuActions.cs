using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuActions : MonoBehaviour
{
    public GameObject menuCanvas;
    public void ShowMenu()
    {
        Debug.LogWarning("Menu is now visible.");
        if (menuCanvas != null)
        {
            menuCanvas.SetActive(true);
        }
        else
        {
            Debug.LogError($"{nameof(MenuActions)}: Menu Canvas is not assigned!");
        }
    }

    public void CloseMenu()
    {
        if (menuCanvas != null)
        {
            menuCanvas.SetActive(false);
        }
        else
        {
            Debug.LogError($"{nameof(MenuActions)}: Menu Canvas is not assigned!");
        }
    }
}
