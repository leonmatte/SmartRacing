using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class TimeController : MonoBehaviour
{
    public GameObject UIRacePanel;

    public GameObject textCurrentTime;
    public GameObject textLastLap;
    public GameObject textBestLap;
    public GameObject textCurrentLap;

    public LapScoreManager UpdateUIForPlayer;
    
    private float currentTime;
    private float lastLapTime;
    private float bestLapTime;
    private int currentLap;

    // Update is called once per frame
    void Update()
    {
        if (UpdateUIForPlayer == null)
        {
            return;
        }

        if (UpdateUIForPlayer.CurrentLap == currentLap) return;
        currentLap = UpdateUIForPlayer.CurrentLap;
        textCurrentLap.GetComponent<TMP_Text>().text = $"LAP: {currentLap}";

    }
}
