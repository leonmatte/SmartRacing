using UnityEngine;
using UnityEngine.UI;

public class CountDown : MonoBehaviour
{
    //Cars and controllers
    public GameObject[] cars = new GameObject[5];
    private readonly carControllerVer4[] _carControllers = new carControllerVer4[5];
    private readonly AltCarAIController[] _carAIControllers = new AltCarAIController[5];

    //COUNTDOWN
    [SerializeField] private Text countdownText;
    private bool _countDownActive;
    private int currentTime = 3;
    private int secondsCountdown = 3;
    private bool isCountDownFinish = false;
    public AudioSource GoAudio;
    public AudioSource GetReady;

    private void Awake()
    {
        for (int i = 0; i < 5; i++)
        {
            _carControllers[i] = cars[i].GetComponent<carControllerVer4>();
            _carAIControllers[i] = cars[i].GetComponent<AltCarAIController>();
        }

        _countDownActive = true;
    }

    void Start()
    {
        CarControllerSwitch(false);
        StartTimer();
    }

    void Update()
    {
        if (!isCountDownFinish)
        {
            StartCountDown();
        }
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
                    if (!controller.isPlayer)
                    {
                        AltCarAIController aiController = controller.gameObject.GetComponent<AltCarAIController>();
                        aiController.enabled = true;
                    }
                }

                break;
            }
            case false:
            {
                foreach (carControllerVer4 controller in _carControllers)
                {
                    controller.enabled = false;

                    AltCarAIController aiController = controller.gameObject.GetComponent<AltCarAIController>();
                    aiController.enabled = false;
                }

                break;
            }
        }
    }


    private void StartTimer()
    {
        Invoke("UpdateTimer", 1f);
    }

    private void UpdateTimer()
    {
        secondsCountdown--;
        Invoke("UpdateTimer", 1f);
    }

    public void StartCountDown()
    {
        currentTime = secondsCountdown;
        if (countdownText != null)
        {
            countdownText.text = currentTime.ToString("0");

            if (currentTime <= 3)
            {
                /*
                GetReady.volume = 20;
                GetReady.Play();
                GetReady.enabled = false;
                */
            }

            if (currentTime <= 2)
            {
                //Sonidos??
            }

            if (currentTime <= 1)
            {
                //Sonidos??
            }

            if (currentTime <= 0)
            {
                /*
                GoAudio.volume = 20;
                GoAudio.Play();
                GoAudio.enabled = false;
                */
                countdownText.text = "Go!";
            }

            if (currentTime <= -1)
            {
                isCountDownFinish = true;
                CancelInvoke();
                countdownText.enabled = false;
                CarControllerSwitch(true);
            }
        }
    }
}