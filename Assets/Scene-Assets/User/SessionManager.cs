using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SessionManager : MonoBehaviour
{

    public TMP_Dropdown CharacterDropdown, languageDropdown, muteDropdown;


    // Start is called before the first frame update
    void Start()
    {
        UserSystemManager.LoadData();
        SetVolume();
    }

    private void SetVolume() {
        AudioListener.volume = UserSystemManager.getMute();
        Debug.Log("volume: " + UserSystemManager.getMute());
    }

    // Update is called once per frame
    public void LoadSettings()
    {
        int gender = UserSystemManager.getCharacterGender();
        int language = UserSystemManager.getLanguage();
        int muted = UserSystemManager.getMute();

        CharacterDropdown.value = gender;
        languageDropdown.value = language;
        muteDropdown.value = muted;

    }

    public void SaveSettings() {
        int gender = CharacterDropdown.value;
        int language = languageDropdown.value;
        int muted = muteDropdown.value;

        UserSystemManager.setCharacterGender(gender);
        UserSystemManager.setLanguage(language);
        UserSystemManager.setMute(muted);

        UserSystemManager.SaveData();
        SetVolume();
    }
}
