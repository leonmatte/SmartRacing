using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public float BestLapTime { get; private set; } = Mathf.Infinity;
    public float LastLapTime { get; private set; }
    public float CurrentLapTime { get; private set; }
    public int CurrentLap { get; private set; }

    private float _lapTimer;
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

    private void StartLap()
    {
        Debug.Log("StartLap!");
        CurrentLap++;
        _lastCheckPointPassed = 1;
        _lapTimerTimestamp = Time.time;
    }

    private void EndLap()
    {
        LastLapTime = Time.time - _lapTimerTimestamp;
        BestLapTime = Mathf.Min(LastLapTime, BestLapTime);
        Debug.Log("EndLap - LapTime was " + LastLapTime + " seconds");
    }

    void OnTriggerEnter(Collider impactCollider)
    {
        if (impactCollider.gameObject.name == "1")
        {
            if (_lastCheckPointPassed == _checkpointCount)
            {
                EndLap();
            }

            if (CurrentLap == 0 || _lastCheckPointPassed == _checkpointCount)
            {
                StartLap();
            }
        }

        if (impactCollider.gameObject.name == (_lastCheckPointPassed + 1).ToString())
        {
            _lastCheckPointPassed++;
        }
    }

    void Update()
    {
        CurrentLapTime = _lapTimerTimestamp > 0 ? Time.time - _lapTimerTimestamp : 0;
    }
}