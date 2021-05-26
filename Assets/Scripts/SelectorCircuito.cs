using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectorCircuito : MonoBehaviour
{
    public void CargarMapa1()
    {
        StartCoroutine(EsperarCambioEscena(3));
    }

    public void CargarMapa2()
    {
        StartCoroutine(EsperarCambioEscena(4));
    }

    public void CargarMapa3()
    {
        StartCoroutine(EsperarCambioEscena(5));
    }

    public void CargarMapa4()
    {
        StartCoroutine(EsperarCambioEscena(6));
    }

    public IEnumerator EsperarCambioEscena(int escena)
    {
        yield return new WaitForSeconds(0.3f);
        SceneManager.LoadScene(escena, LoadSceneMode.Single);
    }
}
