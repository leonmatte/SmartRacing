using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectorCircuito : MonoBehaviour
{
    public void CargarMapa1()
    {
        SceneManager.LoadScene(3, LoadSceneMode.Single);
    }

    public void CargarMapa2()
    {
        SceneManager.LoadScene(4, LoadSceneMode.Single);
    }

    public void CargarMapa3()
    {
        SceneManager.LoadScene(5, LoadSceneMode.Single);
    }

    public void CargarMapa4()
    {
        SceneManager.LoadScene(6, LoadSceneMode.Single);
    }
}
