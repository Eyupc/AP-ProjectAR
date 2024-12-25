using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class UserData
{
    public List<StopData> completedStops = new List<StopData>();
    public int currentStopIndex = 0;
    public Character character = Character.Man;
    public Language language = Language.Dutch;
    public AudioState audioState = AudioState.Unmuted;
}

[System.Serializable]
public class StopData
{
    public string stopId;
    public string stopName;
    public bool isCompleted;
    public System.DateTime completionDate;

    public StopData(string id, string name)
    {
        stopId = id;
        stopName = name;
        isCompleted = false;
    }
}