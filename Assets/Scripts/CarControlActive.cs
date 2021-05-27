using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Vehicles.Car;

public class CarControlActive : MonoBehaviour
{
    public GameObject car;

    void Start()
    {
        if (car.GetComponent<carControllerVer4>().isPlayer)
        {
            car.GetComponent<carControllerVer4>().enabled = true;
            car.GetComponent<UIController>().enabled = true;
        }
        car.GetComponent<UIController>().enabled = false;
        car.GetComponent<PlayerManager>().enabled = true;
        car.GetComponent<AltCarAIController>().enabled = true;
    }
}