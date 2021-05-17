using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CargarPersonaje : MonoBehaviour
{
    public GameObject[] personajePrefab;
    // public TMP_Text label;

    // Start is called before the first frame update
    void Start()
    {
        int personajeSeleccionado = PlayerPrefs.GetInt("personajeSeleccionado");
        GameObject prefab = personajePrefab[personajeSeleccionado];
        prefab.SetActive(true);
        //label.text = prefab.name;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
