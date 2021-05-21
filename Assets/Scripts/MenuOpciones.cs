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
        Application.Quit();
    }

    public void SeleccionarPersonaje()
    {
        //Cambiar a la pantalla donde se selecciona el personaje
        SceneManager.LoadScene(2,  LoadSceneMode.Single);
    }

    public void VolverAlMenuPrincipal()
    {
        //Volver al menú principal
        SceneManager.LoadScene(0, LoadSceneMode.Single);
        Time.timeScale = 1f;
    }

    public void CargarOpciones()
    {
        menu.SetActive(false);
        SceneManager.LoadScene(1, LoadSceneMode.Additive);
    }

    public void RetrocederOpciones()
    {
        if (SceneManager.GetActiveScene().name == "Menu inicial")
        {
            SceneManager.LoadScene(0, LoadSceneMode.Single);
        }
        else if(SceneManager.GetActiveScene().name == "Mapa2")
        {
            SceneManager.UnloadSceneAsync(1);
        }
        
    }

}
