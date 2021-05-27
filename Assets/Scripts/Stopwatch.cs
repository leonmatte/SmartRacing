using System;
using TMPro;
using UnityEngine;

public class Stopwatch : MonoBehaviour
{
    bool _stopwatchActive;
    float _currentTime;
    public TMP_Text currentTimeText;
    
    public float BestLapTime { get; private set; } = Mathf.Infinity;
    public float LastLapTime { get; private set; }
    public float CurrentLapTime { get; private set; }
    public int CurrentLap { get; private set; }

    private int _lastCheckPointPassed;
    private float _lapTimerTimestamp;
    private Transform _checkpointsParent;
    private int _checkpointCount;
    private int _checkpointLayer;

    private void Awake()
    {
        LastLapTime = 0;
        CurrentLapTime = 0;
        CurrentLap = 0;
        _lastCheckPointPassed = 0;
        _checkpointsParent = GameObject.Find("Checkpoints").transform;
        _checkpointCount = _checkpointsParent.childCount;
        _checkpointLayer = LayerMask.NameToLayer("Checkpoint");
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

    public void StartStopwatch()
    {
        _stopwatchActive = true;
    }

    public void StopStopwatch()
    {
        _stopwatchActive = false;
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
                StopStopwatch();
            }

            if (CurrentLap == 0|| _lastCheckPointPassed == _checkpointCount)
            {
                StartStopwatch();
            }
        }

        if (colission.gameObject.name == (_lastCheckPointPassed + 1).ToString())
        {
            _lastCheckPointPassed++;
        }
    }
}
