using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class UserData
{
    public List<StopData> stops = new List<StopData>();
    public int currentStopIndex = 0;
    public Character character = Character.Man;
    public Language language = Language.Dutch;
    public AudioState audioState = AudioState.Unmuted;
}

[System.Serializable]
public class StopData
{
    public int stopId;
    public string stopName;
    public bool isCompleted;
    public System.DateTime completionDate;

    public StopData(int id, string name)
    {
        stopId = id;
        stopName = name;
        isCompleted = false;
    }
}