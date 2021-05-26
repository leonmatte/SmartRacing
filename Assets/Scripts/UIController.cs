using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class UIController : MonoBehaviour
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
    
    private bool _isUpdateUIForPlayerNull;
    private TMP_Text _tmpText;
    private TMP_Text _tmpText1;
    private TMP_Text _tmpText2;
    private TMP_Text _tmpText3;

    private void Awake()
    {
        _tmpText = textCurrentTime.GetComponent<TMP_Text>();
        _isUpdateUIForPlayerNull = updateUIForPlayer == null;
        _tmpText3 = textCurrentLap.GetComponent<TMP_Text>();
        _tmpText2 = textBestLap.GetComponent<TMP_Text>();
        _tmpText1 = textLastLap.GetComponent<TMP_Text>();
    }

    private void Update()
    {
        if (_isUpdateUIForPlayerNull)
        {
            return;
        }

        // ReSharper disable once CompareOfFloatsByEqualityOperator
        if (updateUIForPlayer.CurrentLapTime != _currentLapTime)
        {
            _currentLapTime = updateUIForPlayer.CurrentLapTime;
            _tmpText.text = $"{(int) _currentLapTime / 60}:{(_currentLapTime) % 60:00.000}";
        }

        // ReSharper disable once CompareOfFloatsByEqualityOperator
        if (updateUIForPlayer.LastLapTime != _lastLapTime)
        {
            _lastLapTime = updateUIForPlayer.LastLapTime;
            _tmpText1.text = $"{(int) _lastLapTime / 60}:{(_lastLapTime) % 60:00.000}";
        }

        // ReSharper disable once CompareOfFloatsByEqualityOperator
        if (updateUIForPlayer.BestLapTime != _bestLapTime)
        {
            _bestLapTime = updateUIForPlayer.BestLapTime;
            _tmpText2.text = _bestLapTime < 1000000
                ? $"{(int) _bestLapTime / 60}:{(_bestLapTime) % 60:00.000}"
                : "0:00.00";
        }

        if (updateUIForPlayer.CurrentLap == _currentLap) return;
        _currentLap = updateUIForPlayer.CurrentLap;
        _tmpText3.text = _currentLap.ToString();
    }
}