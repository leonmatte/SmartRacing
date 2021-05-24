using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectorPersonaje : MonoBehaviour
{
    public GameObject[] personajes;
    public int personajeSeleccionado = 0;

    public void SiguientePersonaje()
    {
        personajes[personajeSeleccionado].SetActive(false);
        personajeSeleccionado = (personajeSeleccionado + 1) % personajes.Length;
        personajes[personajeSeleccionado].SetActive(true);
    }

    public void AnteriorPersonaje()
    {
        personajes[personajeSeleccionado].SetActive(false);
        personajeSeleccionado--;
        if(personajeSeleccionado < 0)
        {
            personajeSeleccionado += personajes.Length;
        }
        personajes[personajeSeleccionado].SetActive(true);
    }

    public void PersonajeSeleccionado()
    {
        PlayerPrefs.SetInt("personajeSeleccionado", personajeSeleccionado);
    }
}
