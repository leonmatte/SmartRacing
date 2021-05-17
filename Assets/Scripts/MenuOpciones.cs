using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuOpciones : MonoBehaviour
{

    public void Salir()
    {
        //Salir del juego
        Application.Quit();
    }
    
    public void SeleccionarPersonaje()
    {
        //Cambiar a la pantalla donde se selecciona el personaje
        SceneManager.LoadScene(1,  LoadSceneMode.Single);
    }

    //Función provisional
    public void VolverAPersonajes()
    {
        //Volver a la selección de personaje
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }

}
