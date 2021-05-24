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
    public GameObject LapTimer;
    public GameObject[] ai_cars = new GameObject[3];

    void Start()
    {
        StartCoroutine(CountStart());
        ai_cars[0].GetComponent<AltCarAIController>().enabled = false;
        ai_cars[1].GetComponent<AltCarAIController>().enabled = false;
        ai_cars[2].GetComponent<AltCarAIController>().enabled = false;
        ai_cars[3].GetComponent<AltCarAIController>().enabled = false;
        LapTimer.SetActive(false);
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
        LapTimer.SetActive(true);
        GameObject.FindGameObjectWithTag("Player").GetComponent<carControllerVer4>().enabled = true;
        ai_cars[0].GetComponent<AltCarAIController>().enabled = true;
        ai_cars[1].GetComponent<AltCarAIController>().enabled = true;
        ai_cars[2].GetComponent<AltCarAIController>().enabled = true;
        ai_cars[3].GetComponent<AltCarAIController>().enabled = true;
    }


}
