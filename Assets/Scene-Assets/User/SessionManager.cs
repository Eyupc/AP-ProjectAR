using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SessionManager : MonoBehaviour
{

    public TMP_Dropdown CharacterDropdown, languageDropdown, muteDropdown;
    private string male, female, dutch, arabic, muted, unmuted;

    void Start()
    {
        InitializeDropdowns();
        UserSystemManager.LoadData();
        LoadSettings();
        SetVolume();
    }

    private void ChangeLanguage() {
        if (UserSystemManager.Language == Language.Dutch) {
            male = "Man";
            female = "Vrouw";
            dutch = "Nederlands";
            arabic = "Arabisch";
            muted = "Gedempt";
            unmuted = "Ongedempt";
        } else {
            male = "رجل";
            female = "امرأة";
            dutch = "هولندي";
            arabic = "العربية";
            muted = "صامت";
            unmuted = "غير مكتوم";
        }
    }

    private void ChangeDropdowns() {
        ChangeLanguage();

        CharacterDropdown.captionText.text = UserSystemManager.Character == Character.Man ? male : female;
        languageDropdown.captionText.text = UserSystemManager.Language == Language.Dutch ? dutch : arabic;
        muteDropdown.captionText.text = UserSystemManager.AudioState == AudioState.Unmuted ? unmuted : muted;

        CharacterDropdown.options[0].text = female;
        CharacterDropdown.options[1].text = male;

        languageDropdown.options[0].text = dutch;
        languageDropdown.options[1].text = arabic;
        
        muteDropdown.options[0].text = unmuted;
        muteDropdown.options[1].text = muted;

    }

    private void InitializeDropdowns()
    {
        ChangeLanguage();
        CharacterDropdown.ClearOptions();
        CharacterDropdown.AddOptions(new List<string> {
            female, male
        });

        languageDropdown.ClearOptions();
        languageDropdown.AddOptions(new List<string> {
            dutch, arabic
        });

        muteDropdown.ClearOptions();
        muteDropdown.AddOptions(new List<string> {
            unmuted, muted
        });

        CharacterDropdown.onValueChanged.AddListener(OnCharacterChanged);
        languageDropdown.onValueChanged.AddListener(OnLanguageChanged);
        muteDropdown.onValueChanged.AddListener(OnMuteChanged);

        ChangeDropdowns();
    }

    private void OnCharacterChanged(int value)
    {
        UserSystemManager.Character = (Character)value;
        UserSystemManager.SaveData();
        ChangeDropdowns();
    }

    private void OnLanguageChanged(int value)
    {
        UserSystemManager.Language = (Language)value;
        UserSystemManager.SaveData();
        ChangeDropdowns();
        LanguageManager.LanguageChanged();
    }

    private void OnMuteChanged(int value)
    {
        UserSystemManager.AudioState = (AudioState)value;
        UserSystemManager.SaveData();
        SetVolume();
        ChangeDropdowns();
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
