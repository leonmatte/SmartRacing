using System.Collections;
using JetBrains.Annotations;
using Shapes2D;
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

    public GameObject countDown;
    public AudioSource getReady;
    public AudioSource goAudio;
    public GameObject[] cars = new GameObject[5];

    private Text _text;
    private Text _text1;
    private Text _text2;

    void Awake()
    {
        _text2 = countDown.GetComponent<Text>();
        _text1 = countDown.GetComponent<Text>();
        _text = countDown.GetComponent<Text>();
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
        StartCoroutine(CountStart());
        foreach (GameObject car in cars)
        {
            if (car.GetComponent<AltCarAIController>().enabled)
            {
                car.GetComponent<AltCarAIController>().enabled = false;
                car.GetComponent<UIController>().enabled = false;
                car.GetComponent<carControllerVer4>().enabled = false;

            }

            car.GetComponent<carControllerVer4>().enabled = false;
            car.GetComponent<UIController>().enabled = true;
        }
    }

    IEnumerator CountStart()
    {
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

        foreach (GameObject car in cars)
        {
            if (car.GetComponent<carControllerVer4>().isPlayer != true)
            {
                car.GetComponent<AltCarAIController>().enabled = true;
            }

            car.GetComponent<carControllerVer4>().enabled = true;
        }
        goAudio.Play();
    }

    void StartLap()
    {
        Debug.Log("StartLap!");
        CurrentLap++;
        _lastCheckPointPassed = 1;
        _lapTimerTimestamp = Time.time;
    }

    void EndLap()
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