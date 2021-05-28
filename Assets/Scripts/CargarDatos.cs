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

    [SerializeField] private TextMeshProUGUI [] positionsText = new TextMeshProUGUI[5];

    [SerializeField] private TextMeshProUGUI [] playersText = new TextMeshProUGUI[5];

    [SerializeField] private TextMeshProUGUI [] timesText = new TextMeshProUGUI[5];

    private PlayerData[] playersData;
    
    // Start is called before the first frame update
    void Start()
    {
        String name = PlayerPrefs.GetString("Usuario");
        String map = PlayerPrefs.GetString("mapa");
        String bestLapTime = "00:" + PlayerPrefs.GetString("mejorTiempo");
        TimeSpan bestLapTimeTimeSpan = TimeSpan.Parse(bestLapTime);
        double bestLapTimeDouble = bestLapTimeTimeSpan.TotalMilliseconds;

        nameFile += map;

        playersData = SaveLoadSystemData.LoadData(pathFile, nameFile);
        if (playersData == null)
        {
            playersData = CreateFirstTimeData();
        }

        for (int i = playersData.Length - 1; i >= 0; i--)
        {
            String bestLapTimePlayerArray = playersData[i].GetBestLapTime();
            TimeSpan bestLapTimePlayerArrayTimeSpan = TimeSpan.Parse(bestLapTimePlayerArray);
            double bestLapTimePlayerArrayDouble = bestLapTimePlayerArrayTimeSpan.TotalMilliseconds;

            if (bestLapTimeDouble < bestLapTimePlayerArrayDouble)
            {
                PlayerData playerDataAux = playersData[i];
                playersData[i] = new PlayerData(name, bestLapTime);

                int newPositionArray = i + 1;
                if (newPositionArray < playersData.Length)
                {
                    playersData[newPositionArray] = playerDataAux;
                }
            }
        }

        SaveLoadSystemData.SaveData(playersData, pathFile, nameFile);

        for (int i = 0; i < playersData.Length; i++)
        {
            
            positionsText[i].text = (i + 1).ToString();
            playersText[i].text = playersData[i].GetName();
            timesText[i].text = playersData[i].GetBestLapTime();
        }
    }

    PlayerData[] CreateFirstTimeData()
    {
        PlayerData[] playersData = new PlayerData[5];
        for (int i = 0; i < playersData.Length; i++)
        {
            playersData[i] = new PlayerData("AAA", "23:59:59,999");
        }

        return playersData;
    }

}
