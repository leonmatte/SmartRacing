using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Vehicles.Car;

public class Countdown : MonoBehaviour
{

    public GameObject CountDown;
    public AudioSource GetReady;
    public AudioSource GoAudio;
    public GameObject[] Cars = new GameObject[4];
    private LapScoreManager LapScoreManager;


    private void Awake()
    {
        foreach (GameObject car in Cars)
        {
            if (car.GetComponent<carControllerVer4>().isPlayer != true)
            {
                car.GetComponent<AltCarAIController>().enabled = false;
            }
            car.GetComponent<carControllerVer4>().enabled = false;
        }
    }

    void Start()
    {
        StartCoroutine(CountStart());
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
        LapScoreManager.StartTimer();

        foreach (GameObject car in Cars)
        {
            if (car.GetComponent<carControllerVer4>().isPlayer != true)
            {
                car.GetComponent<AltCarAIController>().enabled = true;
            }
            car.GetComponent<carControllerVer4>().enabled = true;
        }
        
        
        
    }
    
    
}
