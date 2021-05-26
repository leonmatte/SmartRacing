using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuOpciones : MonoBehaviour
{
    public GameObject menu;
    public void Salir()
    {
        //Salir del juego
        if (SceneManager.GetActiveScene().name == "Menu inicial")
        {
            StartCoroutine(EsperarSalir(0.3f));
        }
        else
        {
            Application.Quit();
        }
    }

    public void SeleccionarPersonaje()
    {
        //Cambiar a la pantalla donde se selecciona el personaje
        StartCoroutine(EsperarCambioEscena(2));
    }

    public void VolverAlMenuPrincipal()
    {
        //Volver al menú principal
        StartCoroutine(EsperarCambioEscena(0));
        Time.timeScale = 1f;
        //Iniciar audios
        AudioSource[] audios = FindObjectsOfType<AudioSource>();
        foreach (AudioSource a in audios)
        {
            a.Play();
        }
    }

    public void CargarOpciones()
    {
        if (SceneManager.GetActiveScene().name == "Menu inicial")
        {
            StartCoroutine(EsperarCambioEscenaEncima(1));
        }
        else 
        { 
        SceneManager.LoadScene(1, LoadSceneMode.Additive);
        }
    }

    public void CargarControles()
    {
        if (SceneManager.GetActiveScene().name == "Menu inicial")
        {
            StartCoroutine(EsperarCambioEscenaEncima(9));
        }
        else
        {
            SceneManager.LoadScene(9, LoadSceneMode.Additive);
        }
    }

    public void RetrocederOpciones()
    {
        if (SceneManager.GetActiveScene().name == "Menu inicial")
        {
            StartCoroutine(EsperarCambioEscena(0));
        }
        else if(SceneManager.GetActiveScene().name == "Mapa1")
        {
            SceneManager.UnloadSceneAsync(1);
        }
        else if (SceneManager.GetActiveScene().name == "Mapa2")
        {
            SceneManager.UnloadSceneAsync(1);
        }
        else if (SceneManager.GetActiveScene().name == "Mapa3")
        {
            SceneManager.UnloadSceneAsync(1);
        }
        else if (SceneManager.GetActiveScene().name == "Mapa4")
        {
            SceneManager.UnloadSceneAsync(1);
        }

    }

    public void RetrocederControles()
    {
        if (SceneManager.GetActiveScene().name == "Menu inicial")
        {
            StartCoroutine(EsperarCambioEscena(0));
        }
        else if (SceneManager.GetActiveScene().name == "Mapa1")
        {
            SceneManager.UnloadSceneAsync(9);
        }
        else if (SceneManager.GetActiveScene().name == "Mapa2")
        {
            SceneManager.UnloadSceneAsync(9);
        }
        else if (SceneManager.GetActiveScene().name == "Mapa3")
        {
            SceneManager.UnloadSceneAsync(9);
        }
        else if (SceneManager.GetActiveScene().name == "Mapa4")
        {
            SceneManager.UnloadSceneAsync(9);
        }

    }

    public IEnumerator EsperarCambioEscena(int escena)
    {
        yield return new WaitForSeconds(0.3f);
        SceneManager.LoadScene(escena, LoadSceneMode.Single);
    }

    public IEnumerator EsperarCambioEscenaEncima(int escena)
    {
        yield return new WaitForSeconds(0.3f);
        menu.SetActive(false);
        SceneManager.LoadScene(escena, LoadSceneMode.Additive);
    }

    public IEnumerator EsperarSalir(float tiempo)
    {
        yield return new WaitForSeconds(tiempo);
        Application.Quit();
    }

    public IEnumerator EsperarQuitarEscena(int escena)
    {
        yield return new WaitForSeconds(0.3f);
        SceneManager.UnloadSceneAsync(escena);
    }

}
