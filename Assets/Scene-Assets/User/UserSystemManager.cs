using UnityEngine;
using System.IO;
using Unity.VisualScripting;

public static class UserSystemManager
{
    private static string SAVE_FILE => Path.Combine(Application.persistentDataPath, "user_data.json");
    private static UserData userData;
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

    private static void InitializeDefaultStops()
    {
        userData.completedStops.Add(new StopData("Poem", "The Poem Stop"));
        userData.completedStops.Add(new StopData("Restaurant", "The Restaurant Stop"));
        userData.completedStops.Add(new StopData("AleppoSoap", "Aleppo Soap Stop"));
        SaveData();
    }

    public static void SaveData()
    {
        string json = JsonUtility.ToJson(userData);
        File.WriteAllText(SAVE_FILE, json);

        Debug.Log("EYUP-User data saved");
    }

    public static void CompleteStop(string stopId)
    {
        var stop = userData.completedStops.Find(s => s.stopId == stopId);
        if (stop != null && !stop.isCompleted)
        {
            stop.isCompleted = true;
            stop.completionDate = System.DateTime.Now;
            SaveData();

            switch (stop.stopId)
            {
                case "Poem":
                    var obj = GameObject.FindWithTag("Stop1_Player");
                    if (obj != null) GameObject.Destroy(obj);
                    break;
                case "Restaurant":
                    break;
                case "AleppoSoap":
                    break;
                default:
                    break;
            }
        }
    }

    public static bool IsStopCompleted(string stopId)
    {
        var stop = userData.completedStops.Find(s => s.stopId == stopId);
        return stop?.isCompleted ?? false;
    }

    public static StopData GetCurrentStop()
    {
        if (userData.currentStopIndex < userData.completedStops.Count)
        {
            return userData.completedStops[userData.currentStopIndex];
        }
        return null;
    }

    public static void AdvanceToNextStop()
    {
        if (userData.currentStopIndex < userData.completedStops.Count - 1)
        {
            userData.currentStopIndex++;
            SaveData();
        }
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