using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.SceneManagement;

public class CheckpointTracker : MonoBehaviour
{
    [SerializeField] private List<Transform> carTransformList;
    [SerializeField] private List<carControllerVer4> controllerList;
    private int positions;

    private List<Checkpoint> checkpointList;
    private List<int> nextCheckpointList;

    private float _currentLapTime;
    private float _lastLapTime;
    private float _bestLapTime;
    private int _currentLap;
    private bool _stopwatchActive;
    private int _lastCheckPointPassed;
    private float _lapTimerTimestamp;
    private Transform _checkpointsParent;
    private int _checkpointCount;
    public GameObject uiRacePanel;
    public GameObject textCurrentTime;
    public GameObject textLastLap;
    public GameObject textBestLap;
    public GameObject textCurrentLap;
    private TMP_Text _tmpText;
    private TMP_Text _tmpText1;
    private TMP_Text _tmpText2;
    private TMP_Text _tmpText3;

    void Awake()
    {
        Transform checkpointsTransform = transform.Find("Checkpoints");

        checkpointList = new List<Checkpoint>();

        positions = 0;

        foreach (carControllerVer4 controller in controllerList)
        {
            if (controller.isPlayer)
            {
            }
        }

        foreach (Transform checkpoint in checkpointsTransform)
        {
            Checkpoint checkpointObject = checkpoint.GetComponent<Checkpoint>();
            checkpointObject.SetTrackCheckpoints(this);
            checkpointList.Add(checkpointObject);
        }

        nextCheckpointList = new List<int>();
        foreach (Transform carTransform in carTransformList)
        {
            nextCheckpointList.Add(0);
        }

        _tmpText = textCurrentTime.GetComponent<TMP_Text>();
        _tmpText3 = textCurrentLap.GetComponent<TMP_Text>();
        _tmpText2 = textBestLap.GetComponent<TMP_Text>();
        _tmpText1 = textLastLap.GetComponent<TMP_Text>();
    }

    public void CarThroughCheckpoint(Checkpoint checkpoint, Transform carTransform)
    {
        int nextCheckpointIndex = nextCheckpointList[carTransformList.IndexOf(carTransform)];
        if (checkpointList.IndexOf(checkpoint) == nextCheckpointIndex) // Si el coche pasa por el checkpoint que le toca
        {
            if (nextCheckpointIndex == 0) // Si el checkpoint es el primero
            {
                if (controllerList[carTransformList.IndexOf(carTransform)].isPlayer)
                {
                    StartLap();
                    if (_currentLap == 1)
                    {
                        _currentLapTime = 0;
                    }
                }

                controllerList[carTransformList.IndexOf(carTransform)].lapCounter++; // Siguiente vuelta
                print("Coche: " + carTransformList.IndexOf(carTransform) + ", vuelta: " +
                      controllerList[carTransformList.IndexOf(carTransform)].lapCounter);
                if (controllerList[carTransformList.IndexOf(carTransform)].lapCounter >
                    3) // Si el coche ha completado las tres vueltas
                {
                    controllerList[carTransformList.IndexOf(carTransform)]
                        .GetInputFromAI(Random.Range(-1f, 1f), 0, true, true, false, false);
                    controllerList[carTransformList.IndexOf(carTransform)].driving = false; // El coche deja de correr

                    positions++; // Otro coche más ha terminado la carrera

                    if (controllerList[carTransformList.IndexOf(carTransform)]
                        .isPlayer) // Si el coche que ha terminado la carrera es el jugador
                    {
                        print("HAS QUEDADO EL " + positions); // Se muestra la posición en que ha terminado
                        // Time.timeScale = 0f;
                        // AudioSource[] audios = FindObjectsOfType<AudioSource>();
                        // foreach (AudioSource a in audios)
                        // {
                        //     a.Pause();
                        // }

                        Cursor.visible = true;
                        SceneManager.LoadScene(11, LoadSceneMode.Additive);
                        PlayerPrefs.GetInt("posicionJugador",
                            positions); //Mandar la posición del jugador para recoger en otro script
                    }
                }
            }

            print("Lessgooo " + carTransformList.IndexOf(carTransform));
            nextCheckpointList[carTransformList.IndexOf(carTransform)] =
                (nextCheckpointIndex + 1) % checkpointList.Count;
        }
        else
        {
            print("whutttt " + carTransformList.IndexOf(carTransform));
        }
    }

    void Update()
    {
        if (_stopwatchActive)
        {
            _currentLapTime += Time.deltaTime;
        }

        _tmpText.text = $"{(int) _currentLapTime / 60}:{(_currentLapTime) % 60:00.000}";

        _tmpText1.text = $"{(int) _lastLapTime / 60}:{(_lastLapTime) % 60:00.000}";

        _tmpText2.text = _bestLapTime < 1000000
            ? $"{(int) _bestLapTime / 60}:{(_bestLapTime) % 60:00.000}"
            : "0:00.00";
        _tmpText3.text = _currentLap.ToString();
    }

    void StartLap()
    {
        EndLap();
        _currentLap++;
        _stopwatchActive = true;
        _lapTimerTimestamp = Time.time - _lastLapTime;
    }

    void EndLap()
    {
        _stopwatchActive = false;
        
        if (_currentLap == 0)
        {
            _lastLapTime = 0;
        }
        else
        {
            _lastLapTime = _currentLapTime;    
        }
        if (_bestLapTime == 0)
        {
            _bestLapTime = _lastLapTime;
        }
        else
        {
            _bestLapTime = Mathf.Min(_lastLapTime, _bestLapTime);    
        }
        _currentLapTime = 0;
    }
}