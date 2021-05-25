using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    public void VolverAlMenu()
    {
        StartCoroutine(EsperarCambioEscena(0.3f, 0));
    }

    public IEnumerator EsperarCambioEscena(float tiempo, int escena)
    {
        yield return new WaitForSeconds(tiempo);
        SceneManager.LoadScene(escena, LoadSceneMode.Single);
    }
}
