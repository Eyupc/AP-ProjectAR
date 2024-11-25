using UnityEngine;
using UnityEngine.UI;

public class MapCanvasSetup : MonoBehaviour
{
    public GameObject mapController;  // Reference to the MapController script
    private MapController mapControllerScript;

    void Start()
    {
        mapControllerScript = mapController.GetComponent<MapController>();  // Get the MapController script

        // Create Canvas
        GameObject canvas = new GameObject("MapCanvas");
        canvas.AddComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.AddComponent<CanvasScaler>().uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        canvas.AddComponent<GraphicRaycaster>();

        // Create Viewport (ScrollRect for panning)
        GameObject viewport = new GameObject("Viewport", typeof(Image), typeof(Mask));
        viewport.transform.SetParent(canvas.transform);
        RectTransform viewportRT = viewport.GetComponent<RectTransform>();
        viewportRT.anchorMin = new Vector2(0, 0);
        viewportRT.anchorMax = new Vector2(1, 1);
        viewportRT.offsetMin = new Vector2(0, 0);
        viewportRT.offsetMax = new Vector2(0, 0);
        viewport.GetComponent<Image>().color = new Color(0, 0, 0, 0.1f); // Slight transparency
        viewport.GetComponent<Mask>().showMaskGraphic = false;

        // Create ScrollRect
        ScrollRect scrollRect = viewport.AddComponent<ScrollRect>();
        scrollRect.horizontal = true;
        scrollRect.vertical = true;

        // Create MapContainer
        GameObject mapContainer = new GameObject("MapContainer", typeof(RectTransform));
        mapContainer.transform.SetParent(viewport.transform);
        RectTransform mapContainerRT = mapContainer.GetComponent<RectTransform>();
        mapContainerRT.anchorMin = new Vector2(0.5f, 0.5f);
        mapContainerRT.anchorMax = new Vector2(0.5f, 0.5f);
        mapContainerRT.pivot = new Vector2(0.5f, 0.5f);
        mapContainerRT.sizeDelta = new Vector2(2000, 2000); // Large enough to fit multiple tiles
        scrollRect.content = mapContainerRT;

        // Set the mapContainer reference in MapController
        mapControllerScript.mapContainer = mapContainerRT;
        mapControllerScript.scrollRect = scrollRect;

        // Create Zoom In Button
        GameObject zoomInButton = CreateButton(canvas.transform, "ZoomInButton", "+", new Vector2(-50, -50));
        zoomInButton.GetComponent<Button>().onClick.AddListener(() => mapControllerScript.UpdateZoom(1));

        // Create Zoom Out Button
        GameObject zoomOutButton = CreateButton(canvas.transform, "ZoomOutButton", "-", new Vector2(-50, -120));
        zoomOutButton.GetComponent<Button>().onClick.AddListener(() => mapControllerScript.UpdateZoom(-1));

        // Set the zoom buttons in MapController
        mapControllerScript.zoomInButton = zoomInButton.GetComponent<Button>();
        mapControllerScript.zoomOutButton = zoomOutButton.GetComponent<Button>();

        // Create Attribution Text
        GameObject attribution = new GameObject("AttributionText", typeof(Text));
        attribution.transform.SetParent(canvas.transform);
        Text attributionText = attribution.GetComponent<Text>();
        attributionText.text = "© OpenStreetMap contributors";
        attributionText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        attributionText.fontSize = 16;
        attributionText.color = Color.black;
        attributionText.alignment = TextAnchor.LowerLeft;

        RectTransform attributionRT = attribution.GetComponent<RectTransform>();
        attributionRT.anchorMin = new Vector2(0, 0);
        attributionRT.anchorMax = new Vector2(0, 0);
        attributionRT.pivot = new Vector2(0, 0);
        attributionRT.anchoredPosition = new Vector2(10, 10);
        attributionRT.sizeDelta = new Vector2(400, 30);

        // Set attribution reference in MapController
        mapControllerScript.attributionText = attributionText;

        Debug.Log("Canvas setup complete!");
    }

    GameObject CreateButton(Transform parent, string name, string text, Vector2 position)
    {
        // Button Object
        GameObject button = new GameObject(name, typeof(Button), typeof(Image));
        button.transform.SetParent(parent);

        RectTransform buttonRT = button.GetComponent<RectTransform>();
        buttonRT.sizeDelta = new Vector2(100, 50);
        buttonRT.anchorMin = new Vector2(1, 0); // Bottom-right
        buttonRT.anchorMax = new Vector2(1, 0);
        buttonRT.pivot = new Vector2(1, 0);
        buttonRT.anchoredPosition = position;

        // Button Background
        Image buttonImage = button.GetComponent<Image>();
        buttonImage.color = new Color(0.2f, 0.6f, 1.0f, 1.0f); // Blue color

        // Button Text
        GameObject buttonText = new GameObject("Text", typeof(Text));
        buttonText.transform.SetParent(button.transform);
        Text textComponent = buttonText.GetComponent<Text>();
        textComponent.text = text;
        textComponent.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        textComponent.fontSize = 24;
        textComponent.color = Color.white;
        textComponent.alignment = TextAnchor.MiddleCenter;

        RectTransform textRT = buttonText.GetComponent<RectTransform>();
        textRT.anchorMin = new Vector2(0, 0);
        textRT.anchorMax = new Vector2(1, 1);
        textRT.offsetMin = Vector2.zero;
        textRT.offsetMax = Vector2.zero;

        return button;
    }
}