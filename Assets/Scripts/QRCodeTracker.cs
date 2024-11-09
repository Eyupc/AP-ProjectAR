using HoloLab.ARFoundationQRTracking;
using UnityEngine;
using System.Collections.Generic;

public class QRCodeTracker : MonoBehaviour
{
    [SerializeField] private GameObject QRPrefab;
    [SerializeField] private float trackingTimeout = 0.5f;

    private ARFoundationQRTracker qrTracker;
    private Dictionary<string, GameObject> trackedQRObjects = new Dictionary<string, GameObject>();
    private Dictionary<string, float> lastTrackingTimes = new Dictionary<string, float>();

    private void Awake()
    {
        qrTracker = FindObjectOfType<ARFoundationQRTracker>();
        qrTracker.OnTrackedQRImagesChanged += QRTracker_OnTrackedQRImagesChanged;
    }

    private void Update()
    {
        // Check for QR codes that haven't been updated recently
        List<string> codestoRemove = new List<string>();
        float currentTime = Time.time;

        foreach (var tracking in lastTrackingTimes)
        {
            if (currentTime - tracking.Value > trackingTimeout)
            {
                codestoRemove.Add(tracking.Key);
            }
        }

        // Remove lost QR codes and clean up tracking
        foreach (var codeId in codestoRemove)
        {
            RemoveQRCode(codeId);
        }
    }

    private void RemoveQRCode(string codeId)
    {
        Debug.Log($"QR code lost (timeout): {codeId}");

        if (trackedQRObjects.TryGetValue(codeId, out GameObject qrObject))
        {
            Destroy(qrObject);
            trackedQRObjects.Remove(codeId);
        }

        lastTrackingTimes.Remove(codeId);
    }

    private void CreateOrUpdateQRObject(ARTrackedQRImage qrImage)
    {
        // If we don't have an object for this QR code, create one
        if (!trackedQRObjects.ContainsKey(qrImage.Text))
        {
            GameObject qrObject = Instantiate(QRPrefab, qrImage.transform.position, qrImage.transform.rotation, qrImage.transform);
            trackedQRObjects[qrImage.Text] = qrObject;
        }
        else
        {
            // Update existing object's transform
            GameObject qrObject = trackedQRObjects[qrImage.Text];
            qrObject.transform.position = qrImage.transform.position;
            qrObject.transform.rotation = qrImage.transform.rotation;
            qrObject.transform.SetParent(qrImage.transform, true);
        }

        // Update tracking time
        lastTrackingTimes[qrImage.Text] = Time.time;
    }

    private void QRTracker_OnTrackedQRImagesChanged(ARTrackedQRImagesChangedEventArgs eventArgs)
    {
        Debug.Log($"QR code count: {eventArgs.Added.Count} {eventArgs.Updated.Count} {eventArgs.Removed.Count}");

        // Handle new QR codes
        foreach (var addedQR in eventArgs.Added)
        {
            Debug.Log($"Marker detected: {addedQR.Text}");
            CreateOrUpdateQRObject(addedQR);
        }

        // Handle updated QR codes
        foreach (var updatedQR in eventArgs.Updated)
        {
            Debug.Log($"Marker updated: {updatedQR.Text}");
            CreateOrUpdateQRObject(updatedQR);
        }

        // Handle explicitly removed QR codes
        foreach (var removedQR in eventArgs.Removed)
        {
            RemoveQRCode(removedQR.Text);
        }
    }

    private void OnDestroy()
    {
        if (qrTracker != null)
        {
            qrTracker.OnTrackedQRImagesChanged -= QRTracker_OnTrackedQRImagesChanged;

            // Clean up all tracked QR codes
            foreach (var codeId in new List<string>(trackedQRObjects.Keys))
            {
                RemoveQRCode(codeId);
            }
        }
    }
}