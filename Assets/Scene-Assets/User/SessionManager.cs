using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SessionManager : MonoBehaviour
{

    public TMP_Dropdown CharacterDropdown, languageDropdown, muteDropdown;


    void Start()
    {
        InitializeDropdowns();
        UserSystemManager.LoadData();
        LoadSettings();
        SetVolume();
    }

    private void InitializeDropdowns()
    {
        CharacterDropdown.ClearOptions();
        CharacterDropdown.AddOptions(new List<string> {
            Character.Man.ToString(),
            Character.Woman.ToString()
        });

        languageDropdown.ClearOptions();
        languageDropdown.AddOptions(new List<string> {
            Language.Dutch.ToString(),
            Language.Arabic.ToString()
        });

        muteDropdown.ClearOptions();
        muteDropdown.AddOptions(new List<string> {
            AudioState.Unmuted.ToString(),
            AudioState.Muted.ToString()
        });

        CharacterDropdown.onValueChanged.AddListener(OnCharacterChanged);
        languageDropdown.onValueChanged.AddListener(OnLanguageChanged);
        muteDropdown.onValueChanged.AddListener(OnMuteChanged);
    }

    private void OnCharacterChanged(int value)
    {
        UserSystemManager.Character = (Character)value;
        UserSystemManager.SaveData();
    }

    private void OnLanguageChanged(int value)
    {
        UserSystemManager.Language = (Language)value;
        UserSystemManager.SaveData();
    }

    private void OnMuteChanged(int value)
    {
        UserSystemManager.AudioState = (AudioState)value;
        UserSystemManager.SaveData();
        SetVolume();
    }

    public void LoadSettings()
    {
        CharacterDropdown.value = (int)UserSystemManager.Character;
        languageDropdown.value = (int)UserSystemManager.Language;
        muteDropdown.value = (int)UserSystemManager.AudioState;
    }

    public void SaveSettings()
    {
        UserSystemManager.Character = (Character)CharacterDropdown.value;
        UserSystemManager.Language = (Language)languageDropdown.value;
        UserSystemManager.AudioState = (AudioState)muteDropdown.value;

        UserSystemManager.SaveData();
        SetVolume();
    }

    private void SetVolume()
    {
        AudioListener.volume = UserSystemManager.AudioState == AudioState.Unmuted ? 1 : 0;
    }
}
