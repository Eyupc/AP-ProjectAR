using UnityEngine;

public class WebviewHandler : MonoBehaviour
{
    private WebViewObject webViewObject;

    void Start()
    {
        webViewObject = gameObject.AddComponent<WebViewObject>();

        webViewObject.Init(
            cb: (msg) =>
            {
                Debug.Log($"Message from WebView: {msg}");
                if (msg == "LOAD_FINISHED")
                {
                    Debug.Log("WebView has successfully loaded the URL.");
                    webViewObject.EvaluateJS(@"
                        console.log('Unity WebView loaded, checking Google Maps...');
                        
                        window.onerror = function(message, source, lineno, colno, error) {
                            console.error('JavaScript Error:', message, source, lineno, colno, error);
                            unityCallback('JavaScript Error: ' + message + ' at ' + source + ':' + lineno + ':' + colno);
                        };

                        if (typeof google === 'undefined') {
                            console.error('Google Maps API not loaded');
                        } else {
                            console.log('Google Maps API is available');
                            if (typeof google.maps !== 'undefined' && map) {
                                google.maps.event.trigger(map, 'resize');
                            }
                        }
                    ");
                }
            },

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

        webViewObject.SetMargins(0, 0, 0, 220);
        webViewObject.SetVisibility(true);
        webViewObject.SetScrollbarsVisibility(false);
        webViewObject.SetAlertDialogEnabled(true);
        webViewObject.SetCameraAccess(true);


#if UNITY_ANDROID
        webViewObject.SetHardwareAcceleration(true);
#endif

        string url = "https://ar.eyupc.dev";
        Debug.Log($"Attempting to load URL: {url}");
        webViewObject.LoadURL(url);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && webViewObject.CanGoBack())
        {
            webViewObject.GoBack();
        }
    }
}