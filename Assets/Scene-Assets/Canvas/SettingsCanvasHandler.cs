using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsCanvasHandler : MonoBehaviour
{
    [SerializeField] private GameObject settingsIcon;
    [SerializeField] private GameObject settingsCanvas;

    public void onSettingsIconClick()
    {
        settingsCanvas.SetActive(true);
        settingsIcon.SetActive(false);
    }

    public void ResetSession()
    {
        UserSystemManager.ResetSession();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void onSettingsCloseButtonClick()
    {
        settingsCanvas.SetActive(false);
        settingsIcon.SetActive(true);
    }
}
