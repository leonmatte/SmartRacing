using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIRaceController : MonoBehaviour
{
    public GameObject UIRacePanel;

    public GameObject textCurrentTime;
    public GameObject textLastLap;
    public GameObject textBestLap;
    public GameObject textCurrentLap;

    public PlayerManager UpdateUIForPlayer;
    
    private float currentLapTime;
    private float lastLapTime;
    private float bestLapTime;
    private int currentLap = -1;

    public GameObject CountDown;
    public AudioSource GetReady;
    public AudioSource GoAudio;
    public GameObject[] Cars = new GameObject[4];
    
    void Start()
    {
        StartCoroutine(CountStart());

        foreach (GameObject car in Cars)
        {
            if (car.GetComponent<carControllerVer4>().isPlayer != true)
            {
                car.GetComponent<AltCarAIController>().enabled = false;
                car.GetComponent<UIRaceController>().enabled = false;
            }
            car.GetComponent<carControllerVer4>().enabled = false;
            car.GetComponent<UIRaceController>().enabled = true;
        }
    }
    
    IEnumerator CountStart()
    {
        yield return new WaitForSeconds(0.3f);
        CountDown.GetComponent<Text>().text = "3";
        GetReady.Play();
        CountDown.SetActive(true);
        yield return new WaitForSeconds(1);
        CountDown.SetActive(false);
        CountDown.GetComponent<Text>().text = "2";
        GetReady.Play();
        CountDown.SetActive(true);
        yield return new WaitForSeconds(1);
        CountDown.SetActive(false);
        CountDown.GetComponent<Text>().text = "1";
        GetReady.Play();
        CountDown.SetActive(true);
        yield return new WaitForSeconds(1);
        CountDown.SetActive(false);
        GoAudio.Play();

        foreach (GameObject car in Cars)
        {
            if (car.GetComponent<carControllerVer4>().isPlayer != true)
            {
                car.GetComponent<AltCarAIController>().enabled = true;
            }
            car.GetComponent<carControllerVer4>().enabled = true;
        }
        
    }
    // Update is called once per frame
    void Update()
    {
        if (UpdateUIForPlayer == null)
        {
            return;
        }

        if (UpdateUIForPlayer.CurrentLapTime != currentLapTime)
        {
            currentLapTime = UpdateUIForPlayer.CurrentLapTime;
            textCurrentTime.GetComponent<TMP_Text>().text = $"{(int) currentLapTime / 60}:{(currentLapTime) % 60:00.000}";
        }
        if (UpdateUIForPlayer.LastLapTime != lastLapTime)
        {
            lastLapTime = UpdateUIForPlayer.LastLapTime;
            textLastLap.GetComponent<TMP_Text>().text = $"{(int) lastLapTime / 60}:{(lastLapTime) % 60:00.000}";
        }
        if (UpdateUIForPlayer.BestLapTime != bestLapTime)
        {
            currentLapTime = UpdateUIForPlayer.BestLapTime;
            textBestLap.GetComponent<TMP_Text>().text = bestLapTime < 1000000 ? $"{(int) bestLapTime / 60}:{(bestLapTime) % 60:00.000}" : "NONE";
        }
        
        if (UpdateUIForPlayer.CurrentLap != currentLap)
        {
            currentLap = UpdateUIForPlayer.CurrentLap;
            textCurrentLap.GetComponent<TMP_Text>().text = currentLap.ToString();
        }

    }
}
