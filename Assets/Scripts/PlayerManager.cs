using System.Collections;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public float BestLapTime { get; private set; } = Mathf.Infinity;
    public float LastLapTime { get; private set; }
    public float CurrentLapTime { get; private set; }
    public int CurrentLap { get; private set; }

    private int _lastCheckPointPassed;
    private float _lapTimerTimestamp;
    private Transform _checkpointsParent;
    private int _checkpointCount;
    [UsedImplicitly] private int _checkpointLayer;

    public GameObject[] cars = new GameObject[5];
    private int _counter;

    public float[] lapTimes = new float[3];

    public GameObject countDown;
    public AudioSource getReady;
    public AudioSource goAudio;

    private readonly carControllerVer4[] _carControllers = new carControllerVer4[5];
    private readonly AltCarAIController[] _carAIControllers = new AltCarAIController[5];

    private Text _text;
    private Text _text1;
    private Text _text2;

    private bool _countDownActive = true;

    void Awake()
    {
        LastLapTime = 0;
        CurrentLapTime = 0;
        CurrentLap = 0;
        _lastCheckPointPassed = 0;
        _checkpointsParent = GameObject.Find("Checkpoints").transform;
        _checkpointCount = _checkpointsParent.childCount;
        _checkpointLayer = LayerMask.NameToLayer("Checkpoint");
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
                car.GetComponent<UIController>().enabled = true;
            }

            car.GetComponent<UIController>().enabled = false;
        }
    }

    void Start()
    {
        StartCoroutine(CountStart());
    }

    IEnumerator CountStart()
    {
        CarControllerSwitch(false);
        yield return new WaitForSeconds(0.3f);
        _text.text = "3";
        getReady.Play();
        countDown.SetActive(true);
        yield return new WaitForSeconds(1);
        countDown.SetActive(false);
        _text1.text = "2";
        getReady.Play();
        countDown.SetActive(true);
        yield return new WaitForSeconds(1);
        countDown.SetActive(false);
        _text2.text = "1";
        getReady.Play();
        countDown.SetActive(true);
        yield return new WaitForSeconds(1);
        countDown.SetActive(false);
        goAudio.Play();
        CarControllerSwitch(true);
        _countDownActive = false;
    }

    void OnTriggerEnter(Collider triggerCollision)
    {
        while (CurrentLap <= 3)
        {
            _counter++;
            if (triggerCollision.gameObject.layer != _checkpointLayer)
            {
                return;
            }

            if (triggerCollision.gameObject.name != "1") continue;
            if (_lastCheckPointPassed == _checkpointCount)
            {
                EndLap();
                lapTimes[(_counter - 1)] = BestLapTime;
            }

            if (triggerCollision.gameObject.name == "1" && CurrentLap == 0 || _lastCheckPointPassed == _checkpointCount)
            {
                StartLap();
                CurrentLap++;
            }

            if (triggerCollision.gameObject.name == (_lastCheckPointPassed + 1).ToString())
            {
                _lastCheckPointPassed++;
            }
        }

        CarControllerSwitch(false);
    }

    void Update()
    {
        if (!_countDownActive)
        {
            CurrentLapTime += _lapTimerTimestamp > 0 ? Time.time - _lapTimerTimestamp : 0;
        }
    }

    void StartLap()
    {
        Debug.Log("StartLap!");
        _lastCheckPointPassed = 1;
        _lapTimerTimestamp = Time.time - LastLapTime;
    }

    void EndLap()
    {
        LastLapTime = Time.time - _lapTimerTimestamp;
        BestLapTime = Mathf.Min(LastLapTime, BestLapTime);
        Debug.Log("EndLap - LapTime was " + LastLapTime + " seconds");
        CurrentLap++;
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