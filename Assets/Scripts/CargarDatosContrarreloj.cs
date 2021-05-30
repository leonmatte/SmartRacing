using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CargarDatosContrarreloj : MonoBehaviour
{
    
    [SerializeField] private TextMeshProUGUI [] positionsText = new TextMeshProUGUI[2];
    private float lapTimePlayer;
    private float lapTimeIA;
    
    // Start is called before the first frame update
    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {
        lapTimePlayer = PlayerPrefs.GetFloat("mejorTiempoContrarreloj");
        positionsText[0].text = lapTimePlayer < 1000000
            ? $"{(int) lapTimePlayer / 60}:{(lapTimePlayer) % 60:00.000}"
            : "0:00.00";
        
        lapTimeIA = PlayerPrefs.GetFloat("mejorTiempoIA");
        positionsText[1].text = lapTimeIA < 1000000
            ? $"{(int) lapTimeIA / 60}:{(lapTimeIA) % 60:00.000}"
            : "0:00.00";;
    }
}
