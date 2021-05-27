using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class UIController : MonoBehaviour
{
    [FormerlySerializedAs("UIRacePanel")] [UsedImplicitly]
    public GameObject uiRacePanel;

    public GameObject textCurrentTime;
    public GameObject textLastLap;
    public GameObject textBestLap;
    public GameObject textCurrentLap;

    public GameObject[] cars = new GameObject[5];

    private PlayerManager _updateUIForPlayer;
    private float _currentLapTime;
    private float _lastLapTime;
    private float _bestLapTime;
    private int _currentLap;

    private bool _isUpdateUIForPlayerNull;
    private TMP_Text _tmpText;
    private TMP_Text _tmpText1;
    private TMP_Text _tmpText2;
    private TMP_Text _tmpText3;

    private void Awake()
    {
        _tmpText = textCurrentTime.GetComponent<TMP_Text>();
        _isUpdateUIForPlayerNull = _updateUIForPlayer == null;
        _tmpText3 = textCurrentLap.GetComponent<TMP_Text>();
        _tmpText2 = textBestLap.GetComponent<TMP_Text>();
        _tmpText1 = textLastLap.GetComponent<TMP_Text>();
    }

    private void Start()
    {
        foreach (GameObject car in cars)
        {
            if (car.GetComponent<carControllerVer4>().isPlayer)
            {
                _updateUIForPlayer = car.GetComponent<PlayerManager>();
            }
        }
    }

    private void Update()
    {
        
        
        if (_isUpdateUIForPlayerNull)
        {
            return;
        }

        // ReSharper disable once CompareOfFloatsByEqualityOperator
        if (_updateUIForPlayer.CurrentLapTime != _currentLapTime)
        {
            _currentLapTime = _updateUIForPlayer.CurrentLapTime;
            _tmpText.text = $"{(int) _currentLapTime / 60}:{(_currentLapTime) % 60:00.000}";
        }

        // ReSharper disable once CompareOfFloatsByEqualityOperator
        if (_updateUIForPlayer.LastLapTime != _lastLapTime)
        {
            _lastLapTime = _updateUIForPlayer.LastLapTime;
            _tmpText1.text = $"{(int) _lastLapTime / 60}:{(_lastLapTime) % 60:00.000}";
        }

        // ReSharper disable once CompareOfFloatsByEqualityOperator
        if (_updateUIForPlayer.BestLapTime != _bestLapTime)
        {
            _bestLapTime = _updateUIForPlayer.BestLapTime;
            _tmpText2.text = _bestLapTime < 1000000
                ? $"{(int) _bestLapTime / 60}:{(_bestLapTime) % 60:00.000}"
                : "0:00.00";
        }

        if (_updateUIForPlayer.CurrentLap == _currentLap) return;
        _currentLap = _updateUIForPlayer.CurrentLap;
        _tmpText3.text = _currentLap.ToString();
    }
}