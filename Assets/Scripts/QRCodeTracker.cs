using HoloLab.ARFoundationQRTracking;
using UnityEngine;
using System.Collections.Generic;

public class QRCodeTracker : MonoBehaviour
{
    [SerializeField] private GameObject QRPrefab;
    [SerializeField] private GameObject AvatarPrefab; // Add reference to the avatar prefab
    [SerializeField] private float trackingTimeout = 0.5f;

    private ARFoundationQRTracker qrTracker;
    private Dictionary<string, GameObject> trackedQRObjects = new Dictionary<string, GameObject>();
    private Dictionary<string, float> lastTrackingTimes = new Dictionary<string, float>();
    private HashSet<string> spawnedAvatars = new HashSet<string>(); // Track which QR codes have already spawned avatars

    private void Awake()
    {
        qrTracker = FindObjectOfType<ARFoundationQRTracker>();
        qrTracker.OnTrackedQRImagesChanged += QRTracker_OnTrackedQRImagesChanged;
    }

    private void Update()
    {
        List<string> codestoRemove = new List<string>();
        float currentTime = Time.time;

        foreach (var tracking in lastTrackingTimes)
        {
            if (currentTime - tracking.Value > trackingTimeout)
            {
                codestoRemove.Add(tracking.Key);
            }
        }

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
        // Note: We don't remove from spawnedAvatars because we want to remember which codes have spawned avatars
    }

    private void CreateOrUpdateQRObject(ARTrackedQRImage qrImage)
    {
        if (!trackedQRObjects.ContainsKey(qrImage.Text))
        {
            GameObject qrObject = Instantiate(QRPrefab, qrImage.transform.position, qrImage.transform.rotation, qrImage.transform);
            trackedQRObjects[qrImage.Text] = qrObject;

            // Check if this is the 'Avatar' QR code and hasn't spawned an avatar yet
            if (qrImage.Text == "Avatar" && !spawnedAvatars.Contains(qrImage.Text))
            {
                SpawnAvatar();
                spawnedAvatars.Add(qrImage.Text); // Mark this QR code as having spawned an avatar
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

    private void SpawnAvatar()
    {
        // Get the camera's position (user's position)
        Camera mainCamera = Camera.main;
        if (mainCamera != null)
        {
            // Calculate position 1 meter to the right of the user
            Vector3 userPosition = mainCamera.transform.position;
            Vector3 rightDirection = mainCamera.transform.right;

            // Calculate spawn position (1 meter to the right, same height as user)
            Vector3 spawnPosition = userPosition + rightDirection * -2f;
            spawnPosition.y -= 1.7f;

            // Create the avatar at the calculated position
            GameObject avatar = Instantiate(AvatarPrefab, spawnPosition, Quaternion.identity);

            // Make the avatar look at the user
            avatar.transform.LookAt(new Vector3(userPosition.x, avatar.transform.position.y, userPosition.z));
        }
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
        if (qrTracker != null)
        {
            qrTracker.OnTrackedQRImagesChanged -= QRTracker_OnTrackedQRImagesChanged;

            foreach (var codeId in new List<string>(trackedQRObjects.Keys))
            {
                RemoveQRCode(codeId);
            }
        }
    }
}