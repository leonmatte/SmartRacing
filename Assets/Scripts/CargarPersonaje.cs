using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CargarPersonaje : MonoBehaviour
{
    public GameObject[] personajePrefab;
    public GameObject[] camaraFollower;
    // public TMP_Text label;

    // Start is called before the first frame update
    void Start()
    {
        int personajeSeleccionado = PlayerPrefs.GetInt("personajeSeleccionado");
        GameObject prefab = personajePrefab[personajeSeleccionado];
        GameObject camara = camaraFollower[personajeSeleccionado];
        camara.SetActive(true);

        //Desactivar el rat�n
        Cursor.visible = false;

        //Activa al coche como jugador
        prefab.GetComponent<carControllerVer4>().isPlayer = true;
        //Desactiva el script de la IA
        prefab.GetComponent<AltCarAIController>().enabled = false;

        //label.text = prefab.name;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
