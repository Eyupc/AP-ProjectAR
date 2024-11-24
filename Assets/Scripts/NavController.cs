using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavController : MonoBehaviour
{
    public GameObject homeScreenCanvas;

    void Start()
    {
        homeScreenCanvas.SetActive(false);

    }

    public void ShowHomeScreen()
    {

        homeScreenCanvas.SetActive(true);
    }

    // Function to show the AR Scene
    public void ShowARScene()
    {
        homeScreenCanvas.SetActive(false);
    }

    public void ShowMap()
    {
        Debug.Log("Map functionality not implemented yet.");
    }

}
