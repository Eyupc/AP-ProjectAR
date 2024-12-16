using HoloLab.ARFoundationQRTracking;
using UnityEngine;
using System.Collections.Generic;
using System;

public class QRCodeTracker : MonoBehaviour
{
    [SerializeField] private GameObject QRPrefab;
    [SerializeField] private float trackingTimeout = 0.5f;
    [SerializeField] private QRActionRegistry actionRegistry;

    private ARFoundationQRTracker qrTracker;
    private Dictionary<string, GameObject> trackedQRObjects = new Dictionary<string, GameObject>();
    private Dictionary<string, float> lastTrackingTimes = new Dictionary<string, float>();
    private HashSet<string> executedActions = new HashSet<string>();

    public bool IsTracking { get; private set; } = false;

    private void OnEnable()
    {
        qrTracker = FindObjectOfType<ARFoundationQRTracker>();

        if (IsTracking)
        {
            SubscribeToQRTracker();
        }
    }

    private void OnDisable()
    {
        UnsubscribeFromQRTracker();
    }

    public void EnableTracking()
    {
        IsTracking = true;
        SubscribeToQRTracker();
    }

    public void DisableTracking()
    {
        IsTracking = false;
        UnsubscribeFromQRTracker();

        foreach (var codeId in new List<string>(trackedQRObjects.Keys))
        {
            RemoveQRCode(codeId);
        }
    }

    private void SubscribeToQRTracker()
    {
        if (qrTracker != null)
        {
            qrTracker.OnTrackedQRImagesChanged += QRTracker_OnTrackedQRImagesChanged;
        }
    }

    private void UnsubscribeFromQRTracker()
    {
        if (qrTracker != null)
        {
            qrTracker.OnTrackedQRImagesChanged -= QRTracker_OnTrackedQRImagesChanged;
        }
    }

    private void Update()
    {
        List<string> codesToRemove = new List<string>();
        float currentTime = Time.time;

        foreach (var tracking in lastTrackingTimes)
        {
            if (currentTime - tracking.Value > trackingTimeout)
            {
                codesToRemove.Add(tracking.Key);
            }
        }

        foreach (var codeId in codesToRemove)
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
        if (UserSystemManager.IsStopCompleted(qrImage.Text))
        {
            Debug.Log($"Stop {qrImage.Text} is already completed. Not spawning QR object.");
            return;
        }

        if (!trackedQRObjects.ContainsKey(qrImage.Text))
        {
            GameObject qrObject = Instantiate(QRPrefab, qrImage.transform.position,
                qrImage.transform.rotation, qrImage.transform);
            trackedQRObjects[qrImage.Text] = qrObject;

            if (!executedActions.Contains(qrImage.Text) &&
                actionRegistry.TryGetAction(qrImage.Text, out QRActionBase action))
            {
                action.Execute(qrImage.transform.position, qrImage.transform.rotation);
                executedActions.Add(qrImage.Text);
            }
        }
        else
        {
            GameObject qrObject = trackedQRObjects[qrImage.Text];
            qrObject.transform.position = qrImage.transform.position;
            qrObject.transform.rotation = qrImage.transform.rotation;
            qrObject.transform.SetParent(qrImage.transform, true);
        }

        lastTrackingTimes[qrImage.Text] = Time.time;
    }

    private void QRTracker_OnTrackedQRImagesChanged(ARTrackedQRImagesChangedEventArgs eventArgs)
    {
        Debug.Log($"QR code count: {eventArgs.Added.Count} {eventArgs.Updated.Count} {eventArgs.Removed.Count}");

        foreach (var addedQR in eventArgs.Added)
        {
            Debug.Log($"Marker detected: {addedQR.Text}");
            CreateOrUpdateQRObject(addedQR);
        }

        foreach (var updatedQR in eventArgs.Updated)
        {
            Debug.Log($"Marker updated: {updatedQR.Text}");
            CreateOrUpdateQRObject(updatedQR);
        }

        foreach (var removedQR in eventArgs.Removed)
        {
            RemoveQRCode(removedQR.Text);
        }
    }

    private void OnDestroy()
    {
        UnsubscribeFromQRTracker();
    }
}