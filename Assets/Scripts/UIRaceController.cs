using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UIRaceController : MonoBehaviour
{
    [FormerlySerializedAs("UIRacePanel")] public GameObject uiRacePanel;

    public GameObject textCurrentTime;
    public GameObject textLastLap;
    public GameObject textBestLap;
    public GameObject textCurrentLap;

    [FormerlySerializedAs("UpdateUIForPlayer")] public PlayerManager updateUIForPlayer;

    private float _currentLapTime;
    private float _lastLapTime;
    private float _bestLapTime;
    private int _currentLap = -1;

    [FormerlySerializedAs("CountDown")] public GameObject countDown;
    [FormerlySerializedAs("GetReady")] public AudioSource getReady;
    [FormerlySerializedAs("GoAudio")] public AudioSource goAudio;
    public GameObject[] cars = new GameObject[5];
    private bool _isUpdateUIForPlayerNull;
    private TMP_Text _tmpText;
    private TMP_Text _tmpText1;
    private TMP_Text _tmpText2;
    private TMP_Text _tmpText3;
    private Text _text;
    private Text _text1;
    private Text _text2;

    private void Awake()
    {
        _text2 = countDown.GetComponent<Text>();
        _text1 = countDown.GetComponent<Text>();
        _text = countDown.GetComponent<Text>();
        _tmpText = textCurrentTime.GetComponent<TMP_Text>();
        _isUpdateUIForPlayerNull = updateUIForPlayer == null;
        _tmpText3 = textCurrentLap.GetComponent<TMP_Text>();
        _tmpText2 = textBestLap.GetComponent<TMP_Text>();
        _tmpText1 = textLastLap.GetComponent<TMP_Text>();
    }

    void Start()
    {
        StartCoroutine(CountStart());

        foreach (GameObject car in cars)
        {
            if (car.GetComponent<carControllerVer4>().isPlayer != true)
            {
                car.GetComponent<AltCarAIController>().enabled = false;
                car.GetComponent<UIRaceController>().enabled = false;
            }

            car.GetComponent<carControllerVer4>().enabled = false;
            car.GetComponent<UIRaceController>().enabled = true;
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

    // Update is called once per frame
    void Update()
    {
        if (_isUpdateUIForPlayerNull)
        {
            return;
        }

        if (updateUIForPlayer.CurrentLapTime != _currentLapTime)
        {
            _currentLapTime = updateUIForPlayer.CurrentLapTime;
            _tmpText.text = $"{(int) _currentLapTime / 60}:{(_currentLapTime) % 60:00.000}";
        }

        if (updateUIForPlayer.LastLapTime != _lastLapTime)
        {
            _lastLapTime = updateUIForPlayer.LastLapTime;
            _tmpText1.text = $"{(int) _lastLapTime / 60}:{(_lastLapTime) % 60:00.000}";
        }

        if (updateUIForPlayer.BestLapTime != _bestLapTime)
        {
            _currentLapTime = updateUIForPlayer.BestLapTime;
            _tmpText2.text = _bestLapTime < 1000000
                ? $"{(int) _bestLapTime / 60}:{(_bestLapTime) % 60:00.000}"
                : "0:00.00";
        }

        if (updateUIForPlayer.CurrentLap == _currentLap) return;
        _currentLap = updateUIForPlayer.CurrentLap;
        _tmpText3.text = _currentLap.ToString();
    }
}