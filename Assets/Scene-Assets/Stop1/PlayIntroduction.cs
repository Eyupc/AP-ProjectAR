using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayIntroduction : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip audioClip;
    [SerializeField] private Transform subtitlePosition;
    [SerializeField] private float textHeight = 2f;
    [SerializeField] private List<SubtitleLine> subtitles = new List<SubtitleLine>();
    [SerializeField] private TMP_FontAsset garamondFont;
    [SerializeField] private GameObject prefabToShow;
    //[SerializeField] private MonoBehaviour scriptToActivate;
    private float audioDelay = 3f;

    private GameObject instantiatedPrefab;
    private TextMeshPro subtitleText;
    private float audioStartTime;
    private bool isPlaying;

    void Start()
    {
        //if (scriptToActivate != null)
        //{
        //    scriptToActivate.enabled = false;
        //}

        CreateSubtitleText();
        subtitleText.text = "";
        Invoke(nameof(PlayAudioWithSubtitles), audioDelay);
        Debug.Log("The introduction starts");
    }

    void CreateSubtitleText()
    {
        GameObject textObj = new GameObject("SubtitleText");
        textObj.transform.SetParent(transform);

        textObj.transform.localPosition = Vector3.up * textHeight;

        subtitleText = textObj.AddComponent<TextMeshPro>();
        subtitleText.alignment = TextAlignmentOptions.Center;
        subtitleText.fontSize = 0.8f;
        subtitleText.color = Color.white;
        subtitleText.font = garamondFont;

        RectTransform rectTransform = subtitleText.rectTransform;
        rectTransform.sizeDelta = new Vector2(5f, 3f);
        subtitleText.enableWordWrapping = true;
        subtitleText.overflowMode = TextOverflowModes.Ellipsis;

        subtitleText.gameObject.AddComponent<FaceCamera>();
    }

    void Update()
    {
        if (isPlaying && audioSource.isPlaying)
        {
            UpdateSubtitles();
        }
        else if (isPlaying && !audioSource.isPlaying)
        {
            isPlaying = false;
            subtitleText.text = "";

            if (instantiatedPrefab == null && prefabToShow != null)
            {
                instantiatedPrefab = Instantiate(prefabToShow);
            }
        }
    }

    public void PlayAudioWithSubtitles()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.clip = audioClip;
            audioSource.Play();
            audioStartTime = Time.time;
            isPlaying = true;
        }
    }

    void UpdateSubtitles()
    {
        float currentTime = Time.time - audioStartTime;

        SubtitleLine currentLine = subtitles.Find(s =>
            currentTime >= s.startTime && currentTime <= s.endTime);

        if (currentLine != null)
        {
            subtitleText.text = currentLine.text;
        }
        else
        {
            subtitleText.text = "";
        }
        if (!audioSource.isPlaying)
        {
            isPlaying = false;
            subtitleText.text = "";
        }
    }
}
