using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Collections.Generic;
using HoloLab.ARFoundationQRTracking;
using System;
using UnityEditor;
using UnityEngine.XR.Interaction.Toolkit.Inputs;
using System.Runtime.CompilerServices;

public class MenuActions : MonoBehaviour
{
    public GameObject menuCanvas;
    public GameObject kebabPrefab;
    public GameObject shakriyehPrefab;
    public GameObject fattoushPrefab;
    public GameObject kibbehPrefab;

    [SerializeField] private GameObject questionMarkPrefab;
    private static GameObject questionMark;
    [SerializeField] private XRReferenceImageLibrary library;
    private static Dictionary<string, GameObject> spawnedPrefabs = new Dictionary<string, GameObject>();
    private ARTrackedImageManager arTrackedImageManager;

    private static MenuItem selectedItem = MenuItem.None;

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
        selectedItem = MenuItem.Kebab;
        UpdateAllSpawnedObjects();
        menuCanvas.SetActive(false);
    }

    public void OrderShakriyeh()
    {
        selectedItem = MenuItem.Shakriyeh;
        UpdateAllSpawnedObjects();
        menuCanvas.SetActive(false);

    }

    public void OrcderKibbeh()
    {
        selectedItem = MenuItem.Kibbeh;
        UpdateAllSpawnedObjects();
        menuCanvas.SetActive(false);

    }

    public void OrderFattoush()
    {
        selectedItem = MenuItem.Fattoush;
        UpdateAllSpawnedObjects();
        menuCanvas.SetActive(false);

    }

    private void UpdateAllSpawnedObjects()
    {
        foreach (var prefab in spawnedPrefabs.Values)
        {
            Destroy(prefab);
        }
        Destroy(questionMark);
        spawnedPrefabs.Clear();
    }

    public void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (ARTrackedImage trackedImage in eventArgs.added)
        {
            UpdateSpawnedObject(trackedImage);
        }

        foreach (ARTrackedImage trackedImage in eventArgs.updated)
        {
            string imageName = trackedImage.referenceImage.name;
            if (spawnedPrefabs.ContainsKey(imageName))
            {
                GameObject spawnedObject = spawnedPrefabs[imageName];
                spawnedObject.transform.position = trackedImage.transform.position;
                spawnedObject.transform.rotation = trackedImage.transform.rotation;
                if (selectedItem == MenuItem.Fattoush)
                {
                    spawnedObject.transform.Rotate(-90f, 0f, 0f);
                }

                Vector3 dishTopCenter = spawnedObject.transform.position;
                Renderer objectRenderer = spawnedObject.GetComponent<Renderer>();
                if (objectRenderer != null)
                {
                    dishTopCenter += Vector3.up * objectRenderer.bounds.extents.y * 2f;
                }
                else
                {
                    dishTopCenter += Vector3.up * 2f;
                }

                questionMark.transform.position = dishTopCenter;
                questionMark.transform.rotation = trackedImage.transform.rotation;
            }
            else
            {
                UpdateSpawnedObject(trackedImage);
            }
        }

        foreach (ARTrackedImage trackedImage in eventArgs.removed)
        {
            string imageName = trackedImage.referenceImage.name;
            if (spawnedPrefabs.ContainsKey(imageName))
            {
                Destroy(spawnedPrefabs[imageName]);
                spawnedPrefabs.Remove(imageName);
            }
        }
    }

    private void UpdateSpawnedObject(ARTrackedImage trackedImage)
    {
        string imageName = trackedImage.referenceImage.name;

        if (spawnedPrefabs.ContainsKey(imageName))
        {
            Destroy(spawnedPrefabs[imageName]);
            spawnedPrefabs.Remove(imageName);
        }

        GameObject prefabToSpawn = GetPrefabForMenuItem(selectedItem);

        if (prefabToSpawn != null)
        {
            GameObject spawnedObject = Instantiate(prefabToSpawn,
                trackedImage.transform.position,
                trackedImage.transform.rotation);
            spawnedObject.transform.localScale = new Vector3(8f, 8f, 8f);
            if (selectedItem == MenuItem.Fattoush)
            {
                spawnedObject.transform.Rotate(-90f, 0f, 0f);
                spawnedObject.transform.localScale *= 1.54f;
            }
            spawnedPrefabs[imageName] = spawnedObject;

            Vector3 dishTopCenter = spawnedObject.transform.position;
            Renderer objectRenderer = spawnedObject.GetComponent<Renderer>();
            if (objectRenderer != null)
            {
                dishTopCenter += Vector3.up * objectRenderer.bounds.extents.y * 2f;
            }
            else
            {
                dishTopCenter += Vector3.up * 2f;
            }

            if (questionMarkPrefab != null)
            {
                questionMark = Instantiate(
                    questionMarkPrefab,
                    dishTopCenter,
                    trackedImage.transform.rotation
                );
                questionMark.GetComponent<QuestionMarkScript>().SetInfo(selectedItem);
                questionMark.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            }

            Debug.Log($"Spawned {selectedItem} at marker with question mark on top.");
        }
    }

    private GameObject GetPrefabForMenuItem(MenuItem item)
    {
        return item switch
        {
            MenuItem.Kebab => kebabPrefab,
            MenuItem.Shakriyeh => shakriyehPrefab,
            MenuItem.Kibbeh => kibbehPrefab,
            MenuItem.Fattoush => fattoushPrefab,
            _ => null
        };
    }

    public void CompleteStop()
    {
        UserSystemManager.CompleteStop(2);
        TrackingManager trackingManager = FindObjectOfType<TrackingManager>();
        trackingManager.EnableQRTracking();
    }

}
public enum MenuItem
{
    None,
    Kebab,
    Shakriyeh,
    Kibbeh,
    Fattoush
}