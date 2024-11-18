using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonClick : MonoBehaviour
{
    public GameObject targetCanvas;

    // Functie om het canvas aan of uit te zetten
    public void ToggleCanvasVisibility()
    {
        if (targetCanvas != null)
        {
            // Wissel tussen aan en uit
            bool isActive = targetCanvas.activeSelf;
            targetCanvas.SetActive(!isActive);
            Debug.LogWarning("boe");
        }
        else
        {
            Debug.LogWarning("Er is geen canvas gekoppeld aan dit script!");
        }
    }
}