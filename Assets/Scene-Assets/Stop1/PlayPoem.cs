using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class PlayPoem : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip audioClip;
    [SerializeField] private Transform subtitlePosition;
    [SerializeField] private float textHeight = 2f;
    [SerializeField] private List<SubtitleLine> subtitles = new List<SubtitleLine>();
    [SerializeField] private TMP_FontAsset garamondFont;
    [SerializeField] public int StopId = 1;

    private TextMeshPro subtitleText;
    private float audioStartTime;
    private bool isPlaying;

    public event System.Action OnPoemEnd = delegate { };
    void Start()
    {
        CreateSubtitleText();
        subtitleText.text = "";
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
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit) && hit.collider.gameObject == gameObject)
                {
                    PlayAudioWithSubtitles();
                }
            }
        }

        if (isPlaying && audioSource.isPlaying)
        {
            UpdateSubtitles();
        }
        else if (isPlaying && !audioSource.isPlaying)
        {
            isPlaying = false;
            subtitleText.text = "";
            Destroy(this.gameObject);
            if (StopId == 1)
            {
                UserSystemManager.CompleteStop(1);
            }
            else if (StopId == 3)
            {
                UserSystemManager.CompleteStop(3);
            }
            OnPoemEnd.Invoke();
        }
    }

    void PlayAudioWithSubtitles()
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
public class FaceCamera : MonoBehaviour
{
    private Transform mainCamera;

    void Start()
    {
        mainCamera = Camera.main.transform;
    }

    void LateUpdate()
    {
        transform.LookAt(transform.position + mainCamera.rotation * Vector3.forward,
            mainCamera.rotation * Vector3.up);
    }
}