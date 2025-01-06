using UnityEngine;
using System.IO;
using Unity.VisualScripting;
using System.Linq;
using System.Collections.Generic;
using System;

public static class UserSystemManager
{
    private static string SAVE_FILE => Path.Combine(Application.persistentDataPath, "user_data.json");
    private static UserData userData;
    public static event System.Action OnStopCompleted = delegate { };

    static UserSystemManager()
    {
        LoadData();
    }

    public static void LoadData()
    {
        if (File.Exists(SAVE_FILE))
        {
            string json = File.ReadAllText(SAVE_FILE);
            userData = JsonUtility.FromJson<UserData>(json);
            Debug.Log("EYUP-User data loaded");
        }
        else
        {
            userData = new UserData();
            InitializeDefaultStops();
            Debug.Log("EYUP-User data not loaded");
        }
    }

    public static void ResetSession()
    {
        userData = new UserData();
        InitializeDefaultStops();
        OnStopCompleted.Invoke();
    }
    private static void InitializeDefaultStops()
    {
        userData.stops.Add(new StopData(1, "Poem"));
        userData.stops.Add(new StopData(3, "Restaurant"));
        userData.stops.Add(new StopData(5, "AleppoSoap"));
        SaveData();
    }

    public static void SaveData()
    {
        string json = JsonUtility.ToJson(userData);
        File.WriteAllText(SAVE_FILE, json);
        Debug.Log("EYUP-User data saved");
    }

    public static void CompleteStop(int stopId)
    {
        var stop = userData.stops.Find(s => s.stopId == stopId);
        if (stop != null && !stop.isCompleted)
        {
            stop.isCompleted = true;
            stop.completionDate = System.DateTime.Now;
            SaveData();

            switch (stop.stopId)
            {
                case 1:
                    var obj = GameObject.FindWithTag("Stop1_Player");
                    if (obj != null) GameObject.Destroy(obj);
                    break;
                case 3:
                    var menuCanvasObj = GameObject.FindWithTag("MenuCanvas");
                    menuCanvasObj.SetActive(false);
                    break;
                case 5:
                    break;
                default:
                    break;
            }
            Debug.Log("AAA-Called StopCompleted");
            OnStopCompleted.Invoke();
        }
    }

    public static bool IsStopCompleted(string stopName)
    {
        var stop = userData.stops.Find(s => s.stopName == stopName);
        return stop?.isCompleted ?? false;
    }

    public static List<int>? GetCompletedStops()
    {
        return userData.stops.Count(v => v.isCompleted) > 0 ? userData.stops.Where(v => v.isCompleted).Select((v) => v.stopId).ToList() : null;
    }


    public static Character Character
    {
        get { return userData.character; }
        set { userData.character = value; }
    }


    public static Language Language
    {
        get { return userData.language; }
        set { userData.language = value; }
    }

    public static AudioState AudioState
    {
        get { return userData.audioState; }
        set { userData.audioState = value; }
    }

}