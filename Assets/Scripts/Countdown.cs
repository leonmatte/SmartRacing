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
    public GameObject[] Cars = new GameObject[4];

    void Start()
    {
        StartCoroutine(CountStart());

        foreach (GameObject car in Cars)
        {
            if (car.GetComponent<carControllerVer4>().isPlayer != true)
            {
                car.GetComponent<AltCarAIController>().enabled = false;
            }
            car.GetComponent<carControllerVer4>().enabled = false;
        }

        LapTimer.SetActive(false);

        /*
        ai_cars[0].GetComponent<AltCarAIController>().enabled = false;
        ai_cars[1].GetComponent<AltCarAIController>().enabled = false;
        ai_cars[2].GetComponent<AltCarAIController>().enabled = false;
        ai_cars[3].GetComponent<AltCarAIController>().enabled = false;
        LapTimer.SetActive(false);

        */
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

        foreach (GameObject car in Cars)
        {
            if (car.GetComponent<carControllerVer4>().isPlayer != true)
            {
                car.GetComponent<AltCarAIController>().enabled = true;
            }
            car.GetComponent<carControllerVer4>().enabled = true;
        }

        /*
        GameObject.FindGameObjectWithTag("Player").GetComponent<carControllerVer4>().enabled = true;
        ai_cars[0].GetComponent<AltCarAIController>().enabled = true;
        ai_cars[1].GetComponent<AltCarAIController>().enabled = true;
        ai_cars[2].GetComponent<AltCarAIController>().enabled = true;
        ai_cars[3].GetComponent<AltCarAIController>().enabled = true;
        */

    }


}
