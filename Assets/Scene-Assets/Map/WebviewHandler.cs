using System.Collections;
using System.Linq;
using UnityEngine;

public class WebviewHandler : MonoBehaviour
{
    private WebViewObject webViewObject;

    void OnDestroy()
    {
        UserSystemManager.OnStopCompleted -= HandleStopCompleted;
    }
    void Start()
    {
        UserSystemManager.OnStopCompleted += HandleStopCompleted;
        webViewObject = gameObject.AddComponent<WebViewObject>();
        webViewObject.Init(
            cb: (msg) => Debug.Log($"CallFromJS: {msg}"),
            err: (msg) => Debug.LogError($"WebView Error: {msg}"),
            httpErr: (msg) => Debug.LogError($"HTTP Error: {msg}"),
            started: (msg) => Debug.Log($"WebView Started: {msg}"),
            hooked: (msg) => Debug.Log($"WebView Hooked: {msg}"),
            ld: (msg) => Debug.Log($"WebView Loaded: {msg}"),
            enableWKWebView: true,
            transparent: false,
            zoom: false,
            ua: "Mozilla/5.0 (iPhone; CPU iPhone OS 14_2 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/14.0 Mobile/15E148 Safari/604.1"
        );

        webViewObject.SetMargins(0, 0, 0, 240);
        webViewObject.SetVisibility(true);
        webViewObject.SetScrollbarsVisibility(false);
        webViewObject.SetScrollBounceEnabled(false);
        webViewObject.SetAlertDialogEnabled(true);
        webViewObject.SetCameraAccess(true);

        var completedStops = UserSystemManager.GetCompletedStops();
        string url = "https://ar.eyupc.dev";
        if (completedStops != null)
        {
            string stops = string.Join(",", completedStops);
            url = $"https://ar.eyupc.dev?completed={stops}";
        }

        Debug.Log($"Attempting to load URL: {url}");
        webViewObject.LoadURL(url);

        StartCoroutine(StartLocationService());
    }

    IEnumerator StartLocationService()
    {
        if (!Input.location.isEnabledByUser)
        {
            Debug.LogError("Location services are not enabled by the user.");
            yield break;
        }

        Input.location.Start();

        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        if (maxWait <= 0)
        {
            Debug.LogError("Timed out while initializing location services.");
            yield break;
        }

        if (Input.location.status == LocationServiceStatus.Failed)
        {
            Debug.LogError("Unable to determine device location.");
            yield break;
        }

        Input.location.Stop();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && webViewObject.CanGoBack())
        {
            webViewObject.GoBack();
        }
    }

    void HandleStopCompleted()
    {
        Debug.Log("AAA-HandleStopCompleted");
        var completedStops = UserSystemManager.GetCompletedStops();
        string url = "https://ar.eyupc.dev";
        if (completedStops != null)
        {
            string stops = string.Join(",", completedStops);
            url = $"https://ar.eyupc.dev?completed={stops}";
            Debug.Log($"Refreshing map with completed stops: {stops}");
        }

        webViewObject.LoadURL(url);
    }
}