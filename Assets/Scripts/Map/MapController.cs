using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;

public class GoogleMapsController : MonoBehaviour
{
    [Header("Google Maps Settings")]
    [SerializeField] private string apiKey = "AIzaSyDWohoPxq2AfGFoUgw91hbaS8g-";
    [SerializeField] private int zoomLevel = 12;
    [SerializeField] private double latitude = 40.7128;
    [SerializeField] private double longitude = -74.0060;
    [SerializeField] private RawImage mapDisplay;

    private const string GOOGLE_MAPS_URL = "https://maps.googleapis.com/maps/api/staticmap";
    private Vector2 lastTouchPosition;
    private bool isDragging = false;
    private float zoomMin = 1;
    private float zoomMax = 20;

    private Vector2 mapSize = new Vector2(640, 640);

    void Start()
    {
        StartCoroutine(LoadMap());
    }

    private IEnumerator LoadMap()
    {
        Debug.Log("TEST");
        string url = $"{GOOGLE_MAPS_URL}?center={latitude},{longitude}&zoom={zoomLevel}&size={(int)mapSize.x}x{(int)mapSize.y}&scale=1&key={apiKey}";
        using (UnityWebRequest www = UnityWebRequestTexture.GetTexture(url))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                Texture2D mapTexture = DownloadHandlerTexture.GetContent(www);
                mapDisplay.texture = mapTexture;
                mapDisplay.color = Color.white;
            }
            else
            {
                Debug.LogError($"Failed to load map: {www.error}");
            }
        }
    }

    void Update()
    {
        HandleTouchInput();
    }

    private void HandleTouchInput()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    isDragging = true;
                    lastTouchPosition = touch.position;
                    break;

                case TouchPhase.Moved:
                    if (isDragging)
                    {
                        Vector2 touchDelta = touch.position - lastTouchPosition;
                        PanMap(touchDelta);
                        lastTouchPosition = touch.position;
                    }
                    break;

                case TouchPhase.Ended:
                    isDragging = false;
                    break;
            }
        }
        else if (Input.touchCount == 2)
        {
            HandlePinchZoom();
        }
    }

    private void PanMap(Vector2 delta)
    {
        double degreesPerPixelLat = 180.0 / (256 * Mathf.Pow(2, zoomLevel));
        double degreesPerPixelLon = 360.0 / (256 * Mathf.Pow(2, zoomLevel));

        latitude = Mathf.Clamp((float)(latitude - delta.y * degreesPerPixelLat), -85f, 85f);
        longitude = Mathf.Repeat((float)(longitude + delta.x * degreesPerPixelLon), 360f);

        StartCoroutine(LoadMap());
    }

    private void HandlePinchZoom()
    {
        Touch touch0 = Input.GetTouch(0);
        Touch touch1 = Input.GetTouch(1);

        Vector2 touch0PrevPos = touch0.position - touch0.deltaPosition;
        Vector2 touch1PrevPos = touch1.position - touch1.deltaPosition;

        float prevMagnitude = (touch0PrevPos - touch1PrevPos).magnitude;
        float currentMagnitude = (touch0.position - touch1.position).magnitude;

        float difference = currentMagnitude - prevMagnitude;

        if (Mathf.Abs(difference) > 10f)
        {
            int previousZoom = zoomLevel;
            zoomLevel = Mathf.Clamp(zoomLevel + (difference > 0 ? 1 : -1), (int)zoomMin, (int)zoomMax);

            Vector2 centerScreenPoint = (touch0.position + touch1.position) / 2f;
            Vector2 screenSize = new Vector2(Screen.width, Screen.height);

            double centerLatOffset = ((centerScreenPoint.y / screenSize.y) - 0.5) * 180.0 / Mathf.Pow(2, zoomLevel);
            double centerLonOffset = ((centerScreenPoint.x / screenSize.x) - 0.5) * 360.0 / Mathf.Pow(2, zoomLevel);

            latitude = Mathf.Clamp((float)(latitude + centerLatOffset), -85f, 85f);
            longitude = Mathf.Repeat((float)(longitude + centerLonOffset), 360f);

            StartCoroutine(LoadMap());
        }
    }
}