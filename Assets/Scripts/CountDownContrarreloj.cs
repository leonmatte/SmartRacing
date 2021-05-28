using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CountDownContrarreloj : MonoBehaviour
{
    //Cars and controllers
    public GameObject[] cars;
    private carControllerVer4[] _carControllers;
    private AltCarAIController[] _carAIControllers;
    private GameObject _carPlayer;
    
    //COUNTDOWN
    [SerializeField] private Text countdownText;
    private bool _countDownActive;



    private int currentTime = 3;


    private int secondsCountdown = 3;
    private bool isCountDownFinish = false;

    private void Awake()
    {
        _carControllers = new carControllerVer4[cars.Length];
        _carAIControllers = new AltCarAIController[cars.Length];

        for (int i = 0; i < cars.Length; i++)
        {
            _carControllers[i] = cars[i].GetComponent<carControllerVer4>();
            _carAIControllers[i] = cars[i].GetComponent<AltCarAIController>();
        }


        foreach (GameObject car in cars)
        {
            if (car.GetComponent<carControllerVer4>().isPlayer)
            {
                _carPlayer = car;
            }
        }
        _countDownActive = true;
    }

    void Start()
    {
        CarControllerSwitch(false);
        StartTimer();
    }

    void Update()
    {
        if (!isCountDownFinish)
        {
            StartCountDown();
        }
    }


    void CarControllerSwitch(bool active)
    {
        foreach (carControllerVer4 controller in _carControllers)
        {
            controller.enabled = active;
        }
    }


    private void StartTimer()
    {
        Invoke("UpdateTimer", 1f);
    }

    private void UpdateTimer()
    {
        secondsCountdown--;

        Invoke("UpdateTimer", 1f);
    }

    public void StartCountDown()
    {
        currentTime = secondsCountdown;
        if (countdownText != null)
        {
            countdownText.text = currentTime.ToString("0");

            if (currentTime <= 3)
            {
                //Sonidos??
            }

            if (currentTime <= 2)
            {
                //Sonidos??
            }

            if (currentTime <= 1)
            {
                //Sonidos??
            }

            if (currentTime <= 0)
            {
                countdownText.text = "Go!";
            }

            if (currentTime <= -1)
            {
                isCountDownFinish = true;
                CancelInvoke();
                countdownText.enabled = false;
                CarControllerSwitch(true);
            }
        }
    }
}
