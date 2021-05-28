using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CargarDatos : MonoBehaviour
{
    private PlayerData currentPlayer;

    private PlayerData dataFound;

    private String pathFile = "Data/SavedData";

    private String nameFile = "PlayerScores";

    [SerializeField] private TextMeshProUGUI firstPosition;
    [SerializeField] private TextMeshProUGUI secondPosition;
    
    [SerializeField] private TextMeshProUGUI firstPlayer;
    [SerializeField] private TextMeshProUGUI secondPlayer;
    
    [SerializeField] private TextMeshProUGUI firstTime;
    [SerializeField] private TextMeshProUGUI secondTime;
    
    // Start is called before the first frame update
    void Start()
    {
        int position = PlayerPrefs.GetInt("posicionJugador");
        String name = PlayerPrefs.GetString("Usuario");
        String bestLapTime = PlayerPrefs.GetString("mejorTiempo");
        
        currentPlayer = new PlayerData(name, position, bestLapTime);
        SaveLoadSystemData.SaveData(currentPlayer, pathFile, nameFile);

        dataFound = SaveLoadSystemData.LoadData(pathFile, nameFile);

        firstPosition.text = dataFound.GetPosition().ToString();
        firstPlayer.text = dataFound.GetName();
        firstTime.text = dataFound.GetBestLapTime();
        secondPosition.text = currentPlayer.GetPosition().ToString();
        secondPlayer.text = currentPlayer.GetName();
        secondTime.text = currentPlayer.GetBestLapTime();

    }

}
