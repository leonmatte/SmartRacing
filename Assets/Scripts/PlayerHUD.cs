using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
    //UI CONTROLLER Variables
    public GameObject uiRacePanel;
    public GameObject textCurrentTime;
    public GameObject textLastLap;
    public GameObject textBestLap;
    public GameObject textCurrentLap;
    private TMP_Text _tmpText;
    private TMP_Text _tmpText1;
    private TMP_Text _tmpText2;
    private TMP_Text _tmpText3;

    //Time internal values
    private float _currentLapTime;
    private float _lastLapTime;
    private float _bestLapTime;
    private int _currentLap;
    private bool _stopwatchActive;

    //Checkpoint variables
    private int _lastCheckPointPassed;
    private float _lapTimerTimestamp;
    private Transform _checkpointsParent;
    private int _checkpointCount;

    //Cars and controllers
    public GameObject[] cars = new GameObject[5];
    private readonly carControllerVer4[] _carControllers = new carControllerVer4[5];
    private readonly AltCarAIController[] _carAIControllers = new AltCarAIController[5];
    private GameObject _carPlayer;

    //Player Saved Scores
    private int _counter;
    public float[] lapTimes = new float[3];

    //COUNTDOWN
    public GameObject countDown;
    public AudioSource getReady;
    public AudioSource goAudio;
    private Text _text;
    private Text _text1;
    private Text _text2;
    private bool _countDownActive;

    private int currentTime = 3;
    private float startingTime = 3f;

    [SerializeField] private Text countdownText;
    
    private int secondsCountdown = 3;

    private void Awake()
    {
        _lastLapTime = 0;
        _currentLapTime = 0;
        _currentLap = 0;
        _lastCheckPointPassed = 0;
        _checkpointsParent = GameObject.Find("Checkpoints").transform;
        _checkpointCount = _checkpointsParent.childCount;
        _text2 = countDown.GetComponent<Text>();
        _text1 = countDown.GetComponent<Text>();
        _text = countDown.GetComponent<Text>();
        
        for (int i = 0; i < 4; i++)
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
        
        _tmpText = textCurrentTime.GetComponent<TMP_Text>();
        _tmpText3 = textCurrentLap.GetComponent<TMP_Text>();
        _tmpText2 = textBestLap.GetComponent<TMP_Text>();
        _tmpText1 = textLastLap.GetComponent<TMP_Text>();

        _countDownActive = true;
    }

    void Start()
    {
        // currentTime = startingTime;
        StartTimer();
    }

    void OnTriggerEnter(Collider triggerCollision)
    {
        if (triggerCollision.gameObject.name == _carPlayer.gameObject.name)
        {
            while (_currentLap <= 3)
            {
                _counter++;
                if (triggerCollision.gameObject.name != "1") continue;
                if (_lastCheckPointPassed == _checkpointCount)
                {
                    EndLap();
                    lapTimes[(_counter - 1)] = _bestLapTime;
                }

                if (_currentLap == 0 || _lastCheckPointPassed == _checkpointCount)
                {
                    StartLap();
                }

                if (triggerCollision.gameObject.name == "1" && _currentLap == 0)
                {
                    _currentLap = 1;
                }

                if (triggerCollision.gameObject.name == (_lastCheckPointPassed + 1).ToString())
                {
                    _lastCheckPointPassed++;
                }
            }
        }

        CarControllerSwitch(false);
    }

    void Update()
    {
        StartCountDown();
        if (_stopwatchActive)
        {
            _currentLapTime += Time.deltaTime;
        }

        _tmpText.text = $"{(int) _currentLapTime / 60}:{(_currentLapTime) % 60:00.000}";

        _tmpText1.text = $"{(int) _lastLapTime / 60}:{(_lastLapTime) % 60:00.000}";

        _tmpText2.text = _bestLapTime < 1000000
            ? $"{(int) _bestLapTime / 60}:{(_bestLapTime) % 60:00.000}"
            : "0:00.00";
        _tmpText3.text = _currentLap.ToString();
    }

    void StartLap()
    {
        _stopwatchActive = true;
        _lastCheckPointPassed = 1;
        _lapTimerTimestamp = Time.time - _lastLapTime;
    }

    void EndLap()
    {
        _stopwatchActive = false;
        _lastLapTime = Time.time - _lapTimerTimestamp;
        _bestLapTime = Mathf.Min(_lastLapTime, _bestLapTime);
        _currentLapTime = 0;
        _currentLap++;
    }

    void CarControllerSwitch(bool active)
    {
        switch (active)
        {
            case true:
            {
                foreach (carControllerVer4 controller in _carControllers)
                {
                    if (controller.isPlayer)
                    {
                        controller.enabled = true;
                    }
                }

                foreach (AltCarAIController aiController in _carAIControllers)
                {
                    aiController.enabled = true;
                }

                break;
            }
            case false:
            {
                foreach (carControllerVer4 controller in _carControllers)
                {
                    controller.enabled = false;
                }

                foreach (AltCarAIController aiController in _carAIControllers)
                {
                    aiController.enabled = false;
                }

                break;
            }
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
            CancelInvoke();
            countdownText.enabled = false;
        }
    }
}