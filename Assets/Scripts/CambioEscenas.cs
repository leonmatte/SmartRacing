using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CambioEscenas : MonoBehaviour
{
    public void SeleccionModoDeJuego()
    {
        StartCoroutine(EsperarCambioEscena(7));
    }
    public void VolverAlMenu()
    {
        StartCoroutine(EsperarCambioEscena(0));
    }

    public void SeleccionMapa()
    {
        StartCoroutine(EsperarCambioEscena(8));
    }

    public void Contrarreloj()
    {
        StartCoroutine(EsperarCambioEscena(10));
    }

    public IEnumerator EsperarCambioEscena(int escena)
    {
        yield return new WaitForSeconds(0.3f);
        SceneManager.LoadScene(escena, LoadSceneMode.Single);
    }

}
