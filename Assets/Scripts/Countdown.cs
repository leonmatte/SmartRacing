using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Countdown : MonoBehaviour
{
    public GameObject countDown;
    public AudioSource getReady;
    public AudioSource goAudio;
    public GameObject[] cars = new GameObject[5];

    private readonly carControllerVer4[] _carControllers = new carControllerVer4[5];
    private readonly AltCarAIController[] _carAIControllers = new AltCarAIController[5];

    private Text _text;
    private Text _text1;
    private Text _text2;

    void Awake()
    {
        _text2 = countDown.GetComponent<Text>();
        _text1 = countDown.GetComponent<Text>();
        _text = countDown.GetComponent<Text>();

        for (int i = 0; i < 4; i++)
        {
            _carControllers[i] = cars[i].GetComponent<carControllerVer4>();
            _carAIControllers[i] = cars[i].GetComponent<AltCarAIController>();
        }
    }

    void Start()
    {
        StartCoroutine(CountStart());
    }

    IEnumerator CountStart()
    {
        CarControllerSwitch(false);
        yield return new WaitForSeconds(0.3f);
        _text.text = "3";
        getReady.Play();
        countDown.SetActive(true);
        yield return new WaitForSeconds(1);
        countDown.SetActive(false);
        _text1.text = "2";
        getReady.Play();
        countDown.SetActive(true);
        yield return new WaitForSeconds(1);
        countDown.SetActive(false);
        _text2.text = "1";
        getReady.Play();
        countDown.SetActive(true);
        yield return new WaitForSeconds(1);
        countDown.SetActive(false);
        goAudio.Play();
        CarControllerSwitch(true);
    }

    void CarControllerSwitch(bool active)
    {
        switch (active)
        {
            case true:
            {
                foreach (carControllerVer4 controller in _carControllers)
                {
                    controller.enabled = true;
                }

                foreach (AltCarAIController aiController in _carAIControllers)
                {
                    aiController.enabled = true;
                }

                break;
            }
            case false:
            {
                foreach (carControllerVer4 controller in _carControllers)
                {
                    controller.enabled = false;
                }

                foreach (AltCarAIController aiController in _carAIControllers)
                {
                    aiController.enabled = false;
                }

                break;
            }
        }
    }
}