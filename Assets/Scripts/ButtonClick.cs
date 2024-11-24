using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonClick : MonoBehaviour
{
    public GameObject homeCanvas;
    public GameObject cameraCanvas;
    public GameObject buttonCanvas;

    public void ShowUI(string target)
    {
        homeCanvas.SetActive(false);
        cameraCanvas.SetActive(false);
        Debug.Log("A button was clicked.");
        Debug.Log(target);

        switch (target)
        {
            case "Home":
                homeCanvas.SetActive(true);
                //cameraCanvas.SetActive(false);
                Debug.Log("Home canvas is now visible.");
                break;

            case "Camera":
                cameraCanvas.SetActive(true);
                //homeCanvas.SetActive(false);
                Debug.Log("Camera canvas is now visible.");
                break;

            default:
                Debug.LogWarning("Invalid target: " + target);
                break;
        }
    }
    //public GameObject targetCanvas;
    //public void ToggleCanvasVisibility()
    //{
    //    if (targetCanvas != null)
    //    {
    //        targetCanvas.SetActive(false);
    //        Debug.LogWarning(targetCanvas+" is clicked");
    //    }
    //    else
    //    {
    //        Debug.LogWarning("Er is geen canvas gekoppeld aan dit script!");
    //    }
    //}
}