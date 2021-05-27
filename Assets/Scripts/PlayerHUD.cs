using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
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

    void Awake()
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
        _countDownActive = true;

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

        CarControllerSwitch(false);
    }

    void Update()
    {
        if (_stopwatchActive)
        {
            _currentLapTime += Time.deltaTime;
        }

        //TimeSpan time = TimeSpan.FromSeconds(_currentLapTime);
        //Curre.text = time.ToString(@"mm\:ss\:fff");
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