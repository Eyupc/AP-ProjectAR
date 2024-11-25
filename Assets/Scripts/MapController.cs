using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class MapController : MonoBehaviour
{
    [Header("Map Settings")]
    public Vector2 centerLocation = new Vector2(51.25f, 4.4f); // Center of Antwerp
    public int zoom = 16;

    [Header("UI References")]
    public RectTransform mapContainer;
    public ScrollRect scrollRect;
    public Button zoomInButton;
    public Button zoomOutButton;
    public Text attributionText;

    private Dictionary<string, Texture2D> tileCache = new Dictionary<string, Texture2D>();
    private string[] TILE_SERVERS = new string[]
    {
        "https://a.tile.openstreetmap.org/{0}/{1}/{2}.png",
        "https://b.tile.openstreetmap.org/{0}/{1}/{2}.png",
        "https://c.tile.openstreetmap.org/{0}/{1}/{2}.png"
    };

    private int currentServer = 0;

    void Start()
    {
        LoadMapTile();
    }

    void LoadMapTile()
    {
        StartCoroutine(DownloadMapTile());
    }

    IEnumerator DownloadMapTile()
    {
        int x = LongitudeToTileX(centerLocation.y, zoom);
        int y = LatitudeToTileY(centerLocation.x, zoom);

        string url = string.Format(TILE_SERVERS[currentServer], zoom, x, y);
        currentServer = (currentServer + 1) % TILE_SERVERS.Length;

        using (UnityWebRequest www = UnityWebRequestTexture.GetTexture(url))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                Texture2D texture = DownloadHandlerTexture.GetContent(www);
                tileCache[url] = texture;

                // Convert Texture2D to Sprite and apply to the map container Image
                Sprite mapSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

                // Apply the sprite to the map container's Image component
                mapContainer.GetComponent<Image>().sprite = mapSprite;
            }
            else
            {
                Debug.LogError($"Failed to load map tile: {www.error}");
            }
        }
    }

    public void UpdateZoom(int delta)
    {
        int newZoom = Mathf.Clamp(zoom + delta, 1, 19);
        if (newZoom != zoom)
        {
            zoom = newZoom;
            LoadMapTile();
        }
    }

    int LongitudeToTileX(float longitude, int zoom)
    {
        return (int)((longitude + 180.0) / 360.0 * (1 << zoom));
    }

    int LatitudeToTileY(float latitude, int zoom)
    {
        float latRad = latitude * Mathf.PI / 180.0f;
        return (int)((1.0 - Mathf.Log(Mathf.Tan(latRad) + 1.0f / Mathf.Cos(latRad)) / Mathf.PI) / 2.0 * (1 << zoom));
    }
}