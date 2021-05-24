using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPausa : MonoBehaviour
{
    public GameObject PauseMenuUI;
    public GameObject Informacion;
    public GameObject DireccionContraria;

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            Pause();
            Cursor.visible = true;
        }

        void Pause()
        {
            PauseMenuUI.SetActive(true);
            DireccionContraria.SetActive(false);
            Informacion.SetActive(false);
            Time.timeScale = 0f;
        }

    }

    public void Resume()
    {
        PauseMenuUI.SetActive(false);
        Informacion.SetActive(true);
        Time.timeScale = 1f;
        Cursor.visible = false;
    }

}
