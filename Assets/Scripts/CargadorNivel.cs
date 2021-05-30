using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CargadorNivel : MonoBehaviour
{
    public GameObject PantallaDeCarga;
    public Slider SliderCarga;
    public void Contrarreloj()
    {
        StartCoroutine(CargarAsync(10));
        Cursor.visible = false;
    }

    IEnumerator CargarAsync(int escena)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(escena);
        PantallaDeCarga.SetActive(true);

        while (!operation.isDone)
        {
            float Progreso = Mathf.Clamp01(operation.progress / .9f);
            SliderCarga.value = Progreso;
            yield return null;
        }
    }
}
