using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class UserData
{
    public List<StopData> completedStops = new List<StopData>();
    public int currentStopIndex = 0;
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