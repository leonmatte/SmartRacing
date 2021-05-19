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
    public GameObject CarUserControl;

    void Start()
    {
        StartCoroutine(CountStart());
        CarUserControl.GetComponent<CarUserControl>().enabled = false;

    }


    IEnumerator CountStart()
    {
        LapTimer.SetActive(false);
        yield return new WaitForSeconds(0.5f);
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
        CarUserControl.GetComponent<CarUserControl>().enabled = true;

    }


}