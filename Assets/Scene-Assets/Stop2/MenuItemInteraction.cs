using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuItemInteraction : MonoBehaviour
{
    // Details configuration
    [System.Serializable]
    public class MenuItemDetails
    {
        public string itemName;
        public string description;
        public string[] nutritionalInfo;
        public AudioClip descriptionAudio;
    }

    // Predefined item details
    public MenuItemDetails kebabDetails = new MenuItemDetails
    {
        itemName = "Classic Kebab",
        description = "A delicious Middle Eastern grilled meat dish, traditionally served with fresh bread and salad.",
        nutritionalInfo = new string[]
        {
            "Protein: 25g",
            "Calories: 350",
            "Carbs: 15g"
        }
    };

    public MenuItemDetails shakriyehDetails = new MenuItemDetails
    {
        itemName = "Traditional Shakriyeh",
        description = "A creamy Levantine yogurt-based stew with tender meat, typically served over rice.",
        nutritionalInfo = new string[]
        {
            "Protein: 20g",
            "Calories: 400",
            "Calcium: 15%"
        }
    };

    // UI Elements
    private Canvas detailsCanvas;
    private AudioSource audioSource;

    // Interaction state
    private MenuActions.MenuItem currentMenuItem;
    private bool isDetailsVisible = false;

    private void Start()
    {
        CreateDetailsCanvas();
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    private void CreateDetailsCanvas()
    {
        // Create Canvas
        detailsCanvas = gameObject.AddComponent<Canvas>();
        detailsCanvas.renderMode = RenderMode.WorldSpace;

        // Add Canvas Scaler
        CanvasScaler scaler = gameObject.AddComponent<CanvasScaler>();
        scaler.dynamicPixelsPerUnit = 10f;

        // Add Graphic Raycaster
        gameObject.AddComponent<GraphicRaycaster>();

        // Create background panel
        GameObject backgroundPanel = new GameObject("BackgroundPanel");
        backgroundPanel.transform.SetParent(detailsCanvas.transform, false);
        Image panelImage = backgroundPanel.AddComponent<Image>();
        panelImage.color = new Color(0, 0, 0, 0.7f);
        RectTransform panelRect = backgroundPanel.GetComponent<RectTransform>();
        panelRect.anchorMin = new Vector2(0, 0);
        panelRect.anchorMax = new Vector2(1, 1);
        panelRect.sizeDelta = new Vector2(0, 0);

        // Create text elements
        CreateTextElement("NameText", 0.5f, 0.9f, 40, Color.white);
        CreateTextElement("DescriptionText", 0.5f, 0.7f, 25, Color.white);
        CreateNutritionalInfoText(0.5f, 0.4f, 20, Color.yellow);

        // Create sound button
        CreateSoundButton(0.5f, 0.2f);

        // Initially hide the canvas
        detailsCanvas.gameObject.SetActive(false);
    }

    private void CreateTextElement(string name, float anchorX, float anchorY, int fontSize, Color textColor)
    {
        GameObject textObj = new GameObject(name);
        textObj.transform.SetParent(detailsCanvas.transform, false);

        TextMeshProUGUI text = textObj.AddComponent<TextMeshProUGUI>();
        text.alignment = TextAlignmentOptions.Center;
        text.fontSize = fontSize;
        text.color = textColor;

        RectTransform rectTransform = text.rectTransform;
        rectTransform.anchorMin = new Vector2(anchorX, anchorY);
        rectTransform.anchorMax = new Vector2(anchorX, anchorY);
        rectTransform.sizeDelta = new Vector2(300, 100);
    }

    private void CreateNutritionalInfoText(float anchorX, float anchorY, int fontSize, Color textColor)
    {
        GameObject infoTextObj = new GameObject("NutritionalInfoText");
        infoTextObj.transform.SetParent(detailsCanvas.transform, false);

        TextMeshProUGUI text = infoTextObj.AddComponent<TextMeshProUGUI>();
        text.alignment = TextAlignmentOptions.Center;
        text.fontSize = fontSize;
        text.color = textColor;

        RectTransform rectTransform = text.rectTransform;
        rectTransform.anchorMin = new Vector2(anchorX, anchorY);
        rectTransform.anchorMax = new Vector2(anchorX, anchorY);
        rectTransform.sizeDelta = new Vector2(300, 200);
    }

    private void CreateSoundButton(float anchorX, float anchorY)
    {
        GameObject buttonObj = new GameObject("SoundButton");
        buttonObj.transform.SetParent(detailsCanvas.transform, false);

        Image buttonImage = buttonObj.AddComponent<Image>();
        buttonImage.color = Color.gray;

        RectTransform rectTransform = buttonObj.GetComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(anchorX, anchorY);
        rectTransform.anchorMax = new Vector2(anchorX, anchorY);
        rectTransform.sizeDelta = new Vector2(100, 50);

        Button button = buttonObj.AddComponent<Button>();
        button.onClick.AddListener(PlayDescription);

        // Add button text
        GameObject textObj = new GameObject("ButtonText");
        textObj.transform.SetParent(buttonObj.transform, false);

        TextMeshProUGUI buttonText = textObj.AddComponent<TextMeshProUGUI>();
        buttonText.text = "🔊 Sound";
        buttonText.alignment = TextAlignmentOptions.Center;
        buttonText.fontSize = 20;
        buttonText.color = Color.white;

        RectTransform textRectTransform = buttonText.rectTransform;
        textRectTransform.anchorMin = new Vector2(0, 0);
        textRectTransform.anchorMax = new Vector2(1, 1);
    }

    private void Update()
    {
        // Simple tap/click detection
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject == this.gameObject)
                {
                    ToggleDetails();
                }
            }
        }
    }

    public void SetMenuItem(MenuActions.MenuItem menuItem)
    {
        currentMenuItem = menuItem;
    }

    private void ToggleDetails()
    {
        isDetailsVisible = !isDetailsVisible;
        detailsCanvas.gameObject.SetActive(isDetailsVisible);

        if (isDetailsVisible)
        {
            UpdateDetailsDisplay();
        }
    }

    private void UpdateDetailsDisplay()
    {
        MenuItemDetails details = currentMenuItem == MenuActions.MenuItem.Kebab
            ? kebabDetails
            : shakriyehDetails;

        // Update name text
        TextMeshProUGUI nameText = detailsCanvas.transform.Find("NameText")?.GetComponent<TextMeshProUGUI>();
        if (nameText != null)
            nameText.text = details.itemName;

        // Update description text
        TextMeshProUGUI descriptionText = detailsCanvas.transform.Find("DescriptionText")?.GetComponent<TextMeshProUGUI>();
        if (descriptionText != null)
            descriptionText.text = details.description;

        // Update nutritional info
        TextMeshProUGUI nutritionalText = detailsCanvas.transform.Find("NutritionalInfoText")?.GetComponent<TextMeshProUGUI>();
        if (nutritionalText != null)
            nutritionalText.text = string.Join("\n", details.nutritionalInfo);
    }

    public void PlayDescription()
    {
        MenuItemDetails details = currentMenuItem == MenuActions.MenuItem.Kebab
            ? kebabDetails
            : shakriyehDetails;

        if (audioSource != null && details.descriptionAudio != null)
        {
            audioSource.clip = details.descriptionAudio;
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("Audio source or audio clip is missing!");
        }
    }
}