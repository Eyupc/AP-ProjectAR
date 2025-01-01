using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayDishSound : MonoBehaviour
{
    [SerializeField] private List<AudioClip> audioClips;
    [SerializeField] private DishType dish;
    private AudioSource audioSource;

    public enum DishType
    {
        Kebab,
        Shakriyeh,
        Fattoush,
        Kibbeh
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            Debug.LogError("AudioSource component is missing on this GameObject.");
        }
    }

    public void PlaySound()
    {
        Language language = UserSystemManager.Language;
        Character voiceType = UserSystemManager.Character;
        string clipName = $"{dish}_{language}_{voiceType}";

        AudioClip clipToPlay = GetAudioClipByName(clipName);

        if (clipToPlay != null)
        {
            audioSource.clip = clipToPlay;
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning($"Audio clip not found for name: {clipName}");
        }
    }

    private AudioClip GetAudioClipByName(string clipName)
    {
        foreach (var clips in audioClips)
        {
            if (clips.name == clipName) 
            {
                return clips;
            }
        }

        return null;  
    }
}