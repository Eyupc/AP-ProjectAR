using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Collections.Generic;
using HoloLab.ARFoundationQRTracking;
using System;

public class MenuActions : MonoBehaviour
{
    public GameObject menuCanvas;
    public GameObject kebabPrefab;
    [SerializeField] private XRReferenceImageLibrary library;
    private Dictionary<string, GameObject> spawnedPrefabs = new Dictionary<string, GameObject>();
    private ARTrackedImageManager arTrackedImageManager;

    private static bool isKebabOrdered = false;

    private void Start()
    {
        TrackingManager trackingManager = FindObjectOfType<TrackingManager>();
        trackingManager.EnableImageTracking();
        arTrackedImageManager = FindObjectOfType<ARTrackedImageManager>();

        if (arTrackedImageManager == null)
        {
            Debug.LogError("No ARTrackedImageManager found in the scene!");
            return;
        }

        arTrackedImageManager.referenceLibrary = library;
        arTrackedImageManager.requestedMaxNumberOfMovingImages = 1;
    }

    public void ShowMenu()
    {
        if (menuCanvas != null)
        {
            menuCanvas.SetActive(true);
        }
        else
        {
            Debug.LogError($"{nameof(MenuActions)}: Menu Canvas is not assigned!");
        }
    }

    public void CloseMenu()
    {
        if (menuCanvas != null)
        {
            menuCanvas.SetActive(false);
        }
        else
        {
            Debug.LogError($"{nameof(MenuActions)}: Menu Canvas is not assigned!");
        }
    }

    public void OrderKebab()
    {
        Debug.Log("Kebab ordered - Please scan the marker to place your order");
        isKebabOrdered = true;
    }

    // Make this method public so TrackingManager can use it
    public void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        Debug.Log("AAA- OnTrackedImagesChanged called");
        foreach (ARTrackedImage trackedImage in eventArgs.added)
        {
            UpdateSpawnedObject(trackedImage);
        }

        foreach (ARTrackedImage trackedImage in eventArgs.updated)
        {
            if (spawnedPrefabs.ContainsKey(trackedImage.referenceImage.name))
            {
                GameObject spawnedObject = spawnedPrefabs[trackedImage.referenceImage.name];
                spawnedObject.transform.position = trackedImage.transform.position;
                spawnedObject.transform.rotation = trackedImage.transform.rotation;
            }
            else
            {
                UpdateSpawnedObject(trackedImage);
            }
        }

        foreach (ARTrackedImage trackedImage in eventArgs.removed)
        {
            if (spawnedPrefabs.ContainsKey(trackedImage.referenceImage.name))
            {
                Destroy(spawnedPrefabs[trackedImage.referenceImage.name]);
                spawnedPrefabs.Remove(trackedImage.referenceImage.name);
            }
        }
    }

    private void UpdateSpawnedObject(ARTrackedImage trackedImage)
    {
        string markerName = trackedImage.referenceImage.name;

        Debug.Log(markerName + isKebabOrdered);
        if (markerName == "kebabMarker" && isKebabOrdered)
        {
            if (spawnedPrefabs.ContainsKey(markerName))
            {
                Destroy(spawnedPrefabs[markerName]);
                spawnedPrefabs.Remove(markerName);
            }

            GameObject spawnedObject = Instantiate(kebabPrefab,
                trackedImage.transform.position,
                trackedImage.transform.rotation);

            spawnedPrefabs[markerName] = spawnedObject;
            isKebabOrdered = false;
            Debug.Log("Kebab has been placed!");
        }
    }
}