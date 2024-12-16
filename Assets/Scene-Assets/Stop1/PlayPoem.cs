using UnityEngine;
using TMPro; // Make sure to import TextMeshPro
using System.Collections.Generic;

public class PlayPoem : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private Transform subtitlePosition; // Empty GameObject above character
    [SerializeField] private float textHeight = 2f; // Height above character
    [SerializeField] private List<SubtitleLine> subtitles = new List<SubtitleLine>(); // List of subtitle timings
    [SerializeField] private TMP_FontAsset garamondFont; // List of subtitle timings

    private TextMeshPro subtitleText;
    private float audioStartTime;
    private bool isPlaying;

    void Start()
    {
        // Create text object
        CreateSubtitleText();

        // Hide text initially
        subtitleText.text = "";
    }

    void CreateSubtitleText()
    {
        GameObject textObj = new GameObject("SubtitleText");
        textObj.transform.SetParent(transform);

        // Position it above the character
        textObj.transform.localPosition = Vector3.up * textHeight;

        // Add TextMeshPro component
        subtitleText = textObj.AddComponent<TextMeshPro>();

        // Configure the text component
        subtitleText.alignment = TextAlignmentOptions.Center;
        subtitleText.fontSize = 0.8f;
        subtitleText.color = Color.white;
        subtitleText.font = garamondFont;

        // Enable word wrapping and set maximum width
        RectTransform rectTransform = subtitleText.rectTransform;
        rectTransform.sizeDelta = new Vector2(5f, 3f); // Adjust width and height as needed
        subtitleText.enableWordWrapping = true;
        subtitleText.overflowMode = TextOverflowModes.Ellipsis; // Add ellipsis (...) if text is truncated

        // Make text face camera
        subtitleText.gameObject.AddComponent<FaceCamera>();
    }

    void Update()
    {
        // Handle touch input
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

        // Update subtitles if audio is playing
        if (isPlaying && audioSource.isPlaying)
        {
            UpdateSubtitles();
        }
    }

    void PlayAudioWithSubtitles()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
            audioStartTime = Time.time;
            isPlaying = true;
        }
    }

    void UpdateSubtitles()
    {
        float currentTime = Time.time - audioStartTime;

        // Find the current subtitle
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

        // Check if audio has finished
        if (!audioSource.isPlaying)
        {
            isPlaying = false;
            subtitleText.text = "";
        }
    }
}

// Additional script to make text face camera
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