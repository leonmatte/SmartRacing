using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuAjustes : MonoBehaviour
{
    public Toggle Toggle;
    public TMP_Dropdown resolutionDropdown;
    Resolution[] resolutions;

    public TMP_Dropdown dropdown_calidad;
    private int calidad;

    //Slider que controlan volumen audios
    public Slider SliderVolumen;
    public float SliderValueVolumen;

    // Start is called before the first frame update
    void Start()
    {
        //Calidad de juego con guardado
        
        calidad = PlayerPrefs.GetInt("numeroDeCalidad", 3);
        dropdown_calidad.value = calidad;
        AjustarCalidad();

        //Resolucíon del juego y pantalla completa
        
        if (Screen.fullScreen)
        {
            Toggle.isOn = true;
        }
        else
        {
            Toggle.isOn = false;
        }
        RevisarResolucion();

        //Controlar volumen
        SliderVolumen.value = PlayerPrefs.GetFloat("VolumenAudio", 0.5f);
        AudioListener.volume = SliderVolumen.value;
    }

    //Calidad de juego con guardado
    public void AjustarCalidad()
    {
        QualitySettings.SetQualityLevel(dropdown_calidad.value);
        PlayerPrefs.SetInt("numeroDeCalidad", dropdown_calidad.value);
        calidad = dropdown_calidad.value;
    }

    //Pantalla completa 
    
    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }

    public void RevisarResolucion()
    {
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();
        int currentResolutionIndex = 0;
        
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (Screen.fullScreen && resolutions[i].width == Screen.currentResolution.width && 
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        resolutionDropdown.value = PlayerPrefs.GetInt("numeroResolucion", 0);
    }

    //Resolucíon del juego
    public void CambiarResolucion(int indiceResolucion)
    {
        PlayerPrefs.SetInt("numeroResolucion", resolutionDropdown.value);

        Resolution resolution = resolutions[indiceResolucion];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    //Controlar volumen musica con sliderMusica
    public void ChangeSliderVolume(float valorVolumen)
    {
        SliderValueVolumen = valorVolumen;
        PlayerPrefs.SetFloat("VolumenAudio", SliderValueVolumen);
        AudioListener.volume = SliderVolumen.value;
    }

}
