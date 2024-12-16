using UnityEngine;
using UnityEngine.XR.ARFoundation;
using HoloLab.ARFoundationQRTracking;

public class TrackingManager : MonoBehaviour
{
    [SerializeField] private ARFoundationQRTracker qrTracker;
    [SerializeField] private ARTrackedImageManager imageTracker;
    [SerializeField] private ARSession arSession;
    [SerializeField] private QRCodeTracker qrCodeTracker;
    [SerializeField] private MenuActions menuActions;

    private void Awake()
    {
        // Ensure references are found if not manually assigned
        if (qrTracker == null)
            qrTracker = FindObjectOfType<ARFoundationQRTracker>();

        if (imageTracker == null)
            imageTracker = FindObjectOfType<ARTrackedImageManager>();

        if (arSession == null)
            arSession = FindObjectOfType<ARSession>();

        if (qrCodeTracker == null)
            qrCodeTracker = FindObjectOfType<QRCodeTracker>();

        if (menuActions == null)
            menuActions = FindObjectOfType<MenuActions>();

        // Default to QR tracking
        EnableQRTracking();
    }

    public void EnableQRTracking()
    {
        Debug.Log("ABC-" + imageTracker);
        if (arSession != null)
        {
            arSession.Reset();
        }
        if (qrTracker != null)
        {
            qrTracker.enabled = true;
            qrCodeTracker.EnableTracking();
            Debug.Log("QR Tracking Enabled");
        }

        if (imageTracker != null)
        {
            imageTracker.enabled = false;
            imageTracker.trackedImagesChanged -= menuActions.OnTrackedImagesChanged;
            Debug.Log("Image Tracking Disabled");
        }
    }

    public void EnableImageTracking()
    {
        if (arSession != null)
        {
            arSession.Reset();
        }
        if (qrTracker != null)
        {
            qrTracker.enabled = false;
            qrCodeTracker.DisableTracking();
            Debug.Log("QR Tracking Disabled");
        }

        if (imageTracker != null)
        {
            imageTracker.enabled = true;
            imageTracker.trackedImagesChanged += menuActions.OnTrackedImagesChanged;
            Debug.Log("Image Tracking Enabled");
        }

    }
}