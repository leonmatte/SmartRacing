using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LapScoreManager : MonoBehaviour
{ 
    public float BestLapTime { get; private set; } = Mathf.Infinity; 
    public float LastLapTime { get; private set; }
    public float CurrentLapTime { get; private set; }
    
    public int CurrentLap { get; set; }
    public int position_player { get; set; }

    private float lapTimerTimestamp;

    void Awake()
    {
        LastLapTime = 0;
        CurrentLapTime = 0;
        CurrentLap = 0;
    }

    public void StartTimer()
    {
        Debug.Log("StartedLapTimer!");
        lapTimerTimestamp = Time.time;
    }

    public void EndTimer()
    {
        Debug.Log("EndedLapTimer!");
        LastLapTime = Time.time - lapTimerTimestamp;
        BestLapTime = Mathf.Min(LastLapTime, BestLapTime);
    }
    void Update()
    {
        CurrentLapTime = lapTimerTimestamp > 0 ? Time.time - lapTimerTimestamp: 0;
    }
}
