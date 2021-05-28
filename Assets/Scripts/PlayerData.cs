using System;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    [SerializeField] private String name;
    [SerializeField] private String bestLapTime;

    public PlayerData(string name, String bestLapTime)
    {
        this.name = name;
        
        this.bestLapTime = bestLapTime;
    }


    public String GetName()
    {
        return name;
    }
    
    public String GetBestLapTime()
    {
        return bestLapTime;
    }
    
}
