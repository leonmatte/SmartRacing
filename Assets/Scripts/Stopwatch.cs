using System;
using TMPro;
using UnityEngine;

public class Stopwatch : MonoBehaviour
{
    bool _stopwatchActive;
    float _currentTime;
    public TMP_Text currentTimeText;
    public TMP_Text currentLap;
    public TMP_Text lastLapTimeText;

    public int CurrentLap;

    private int _lastCheckPointPassed;
    private Transform _checkpointsParent;
    private int _checkpointCount;
    private int _checkpointLayer;

    private readonly carControllerVer4[] _carControllers = new carControllerVer4[5];
    private readonly AltCarAIController[] _carAIControllers = new AltCarAIController[5];

    public GameObject[] cars = new GameObject[5];

    private void Awake()
    {
        CurrentLap = 1;
        _lastCheckPointPassed = 0;
        _checkpointsParent = GameObject.Find("Checkpoints").transform;
        _checkpointCount = _checkpointsParent.childCount;
        _checkpointLayer = LayerMask.NameToLayer("Checkpoint");

        for (int i = 0; i < 4; i++)
        {
            _carControllers[i] = cars[i].GetComponent<carControllerVer4>();
            _carAIControllers[i] = cars[i].GetComponent<AltCarAIController>();
        }
    }

    void Start()
    {
        _currentTime = 0;
        _stopwatchActive = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (_stopwatchActive)
        {
            _currentTime += Time.deltaTime;
        }

        TimeSpan time = TimeSpan.FromSeconds(_currentTime);
        currentTimeText.text = time.ToString(@"mm\:ss\:fff");
    }

    public void startTimer()
    {
        _stopwatchActive = true;
    }

    public void resetTimer()
    {
        _stopwatchActive = false;
        _currentTime = 0;
    }

    void OnTriggerEnter(Collider colission)
    {
        if (colission.gameObject.layer != _checkpointLayer)
        {
            return;
        }

        if (colission.gameObject.name == "1")
        {
            if (_lastCheckPointPassed == _checkpointCount)
            {
                lastLapTimeText.text = _currentTime.ToString(@"mm\:ss\:fff");
                resetTimer();
            }

            if (CurrentLap == 0 || _lastCheckPointPassed == _checkpointCount)
            {
                startTimer();
                CurrentLap++;
                currentLap.text = currentLap.ToString();
            }
        }

        if (colission.gameObject.name == (_lastCheckPointPassed + 1).ToString())
        {
            _lastCheckPointPassed++;
        }
    }

    void CarControllerSwitch(bool active)
    {
        switch (active)
        {
            case true:
            {
                foreach (carControllerVer4 controller in _carControllers)
                {
                    controller.enabled = true;
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
}