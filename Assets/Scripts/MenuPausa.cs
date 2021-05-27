using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPausa : MonoBehaviour
{
    public GameObject PauseMenuUI;
    public GameObject Informacion;
    public GameObject DireccionContraria;
    public AudioSource musica;

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
            //Parar audios
            AudioSource[] audios = FindObjectsOfType<AudioSource>();
            foreach (AudioSource a in audios)
            {
                a.Pause();
            }
        }

    }

    public void Resume()
    {
        PauseMenuUI.SetActive(false);
        Informacion.SetActive(true);
        Time.timeScale = 1f;
        Cursor.visible = false;
        //Iniciar audios
        AudioSource[] audios = FindObjectsOfType<AudioSource>();
        foreach (AudioSource a in audios)
        {
            a.volume = 0;
            a.Play();
        }
    }

    public void volveReproducir()
    {
        musica.volume = 1;
    }

}
