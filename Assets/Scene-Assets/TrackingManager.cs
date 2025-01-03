using UnityEngine;
using UnityEngine.XR.ARFoundation;
using HoloLab.ARFoundationQRTracking;
using System.Collections;

public class TrackingManager : MonoBehaviour
{
    [SerializeField] private ARFoundationQRTracker qrTracker;
    [SerializeField] private ARTrackedImageManager imageTracker;
    [SerializeField] private ARSession arSession;
    [SerializeField] private QRCodeTracker qrCodeTracker;
    [SerializeField] private MenuActions menuActions;

    private bool isChangingTrackers = false;

    private void Awake()
    {
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

        // Ensure both trackers start disabled
        if (imageTracker != null) imageTracker.enabled = false;
        if (qrTracker != null) qrTracker.enabled = false;

        // Default to QR tracking
        StartCoroutine(EnableQRTrackingCoroutine());
    }

    public void EnableQRTracking()
    {
        if (!isChangingTrackers)
        {
            StartCoroutine(EnableQRTrackingCoroutine());
        }
    }

    public void EnableImageTracking()
    {
        if (!isChangingTrackers)
        {
            StartCoroutine(EnableImageTrackingCoroutine());
        }
    }

    private IEnumerator EnableQRTrackingCoroutine()
    {
        isChangingTrackers = true;
        Debug.Log("Enabling QR Tracking - Starting transition");

        // First disable current tracking
        if (imageTracker != null)
        {
            imageTracker.enabled = false;
            imageTracker.trackedImagesChanged -= menuActions.OnTrackedImagesChanged;
            Debug.Log("Image Tracking Disabled");
        }

        // Reset session
        if (arSession != null)
        {
            arSession.Reset();
            yield return new WaitForSeconds(0.5f); // Wait for session reset
        }

        // Enable QR tracking
        if (qrTracker != null && qrCodeTracker != null)
        {
            qrTracker.enabled = true;
            yield return new WaitForSeconds(0.2f); // Give the tracker time to initialize

            qrCodeTracker.EnableTracking();
            Debug.Log("QR Tracking Enabled");
        }

        isChangingTrackers = false;
    }

    private IEnumerator EnableImageTrackingCoroutine()
    {
        isChangingTrackers = true;
        Debug.Log("Enabling Image Tracking - Starting transition");

        // First disable current tracking
        if (qrTracker != null && qrCodeTracker != null)
        {
            qrCodeTracker.DisableTracking();
            qrTracker.enabled = false;
            Debug.Log("QR Tracking Disabled");
        }

        // Reset session
        if (arSession != null)
        {
            arSession.Reset();
            yield return new WaitForSeconds(0.5f); // Wait for session reset
        }

        // Enable image tracking
        if (imageTracker != null)
        {
            imageTracker.enabled = true;
            yield return new WaitForSeconds(0.2f); // Give the tracker time to initialize

            imageTracker.trackedImagesChanged += menuActions.OnTrackedImagesChanged;
            Debug.Log("Image Tracking Enabled");
        }

        isChangingTrackers = false;
    }

    private void OnDisable()
    {
        // Cleanup event handlers
        if (imageTracker != null)
        {
            imageTracker.trackedImagesChanged -= menuActions.OnTrackedImagesChanged;
        }
    }
}