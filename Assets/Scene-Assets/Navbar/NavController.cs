using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavController : MonoBehaviour
{
    public GameObject homeCanvas;
    public GameObject mapCanvas;

    public void ShowUI(string target)
    {
        homeCanvas.SetActive(false);
        if (mapCanvas.GetComponent<WebViewObject>() != null)
            mapCanvas.GetComponent<WebViewObject>().SetVisibility(false);
        mapCanvas.SetActive(false);

        switch (target)
        {
            case "Home":
                homeCanvas.SetActive(true);
                Debug.Log("Home canvas is now visible.");
                break;
            case "Map":
                mapCanvas.SetActive(true);
                mapCanvas.GetComponent<WebViewObject>().SetVisibility(true);
                Debug.Log(" Map is now visible.");
                break;
            default:
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