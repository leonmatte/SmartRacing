using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CambioEscenas : MonoBehaviour
{
    public void SeleccionModoDeJuego()
    {
        StartCoroutine(EsperarCambioEscena(0.3f, 7));
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
