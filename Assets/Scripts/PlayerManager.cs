using JetBrains.Annotations;
using UnityEngine;

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

    private GameObject _carPlayer;
    private carControllerVer4 _carControllerVer4;

    private bool _carControllerEnabled;

    void Awake()
    {
        LastLapTime = 0;
        CurrentLapTime = 0;
        CurrentLap = 0;
        _lastCheckPointPassed = 0;
        _carControllerEnabled = false;
        _checkpointsParent = GameObject.Find("Checkpoints").transform;
        _checkpointCount = _checkpointsParent.childCount;
        _checkpointLayer = LayerMask.NameToLayer("Checkpoint");

        foreach (GameObject car in cars)
        {
            if (car.GetComponent<carControllerVer4>().isPlayer)
            {
                _carPlayer = car;
            }
        }
        
        _carControllerVer4 = _carPlayer.GetComponent<carControllerVer4>();
        
        foreach (GameObject car in cars)
        {
            if (car.GetComponent<carControllerVer4>().isPlayer)
            {
                car.GetComponent<carControllerVer4>().enabled = true;
            }
            car.GetComponent<AltCarAIController>().enabled = true;
            car.GetComponent<carControllerVer4>().enabled = true;
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

            if (CurrentLap == 0 || _lastCheckPointPassed == _checkpointCount)
            {
                StartLap();
            }

            if (triggerCollision.gameObject.name == "1" && CurrentLap == 0)
            {
                _carControllerEnabled = true;
            }

            if (triggerCollision.gameObject.name == (_lastCheckPointPassed + 1).ToString())
            {
                _lastCheckPointPassed++;
            }
        }

        foreach (GameObject car in cars)
        {
            if (car.GetComponent<carControllerVer4>().isPlayer)
            {
                car.GetComponent<carControllerVer4>().enabled = false;
            }
            car.GetComponent<AltCarAIController>().enabled = false;
            car.GetComponent<carControllerVer4>().enabled = false;
        }
    }

    void Update()
    {
        if (_carControllerEnabled)
        {
            CurrentLapTime = _lapTimerTimestamp > 0 ? Time.time - _lapTimerTimestamp : 0;
        }
    }
}