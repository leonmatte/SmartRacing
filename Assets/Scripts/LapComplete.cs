using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class LapComplete : MonoBehaviour
{

	public GameObject LapCompleteTrig;
	public GameObject HalfLapTrig;

	public GameObject MinuteDisplay;
	public GameObject SecondDisplay;
	public GameObject MilliDisplay;

	public GameObject LapTimeBox;

	public GameObject LapCounter;
	public int VueltasHechas;

	void OnTriggerEnter()
	{
		VueltasHechas++;
		if (LapTimeManager.SecondCount <= 9)
		{
			SecondDisplay.GetComponent<TMP_Text>().text = "0" + LapTimeManager.SecondCount + ".";
		}
		else
		{
			SecondDisplay.GetComponent<TMP_Text>().text = "" + LapTimeManager.SecondCount + ".";
		}

		if (LapTimeManager.MinuteCount <= 9)
		{
			MinuteDisplay.GetComponent<TMP_Text>().text = "0" + LapTimeManager.MinuteCount + ".";
		}
		else
		{
			MinuteDisplay.GetComponent<TMP_Text>().text = "" + LapTimeManager.MinuteCount + ".";
		}

		MilliDisplay.GetComponent<TMP_Text>().text = "" + LapTimeManager.MilliCount;

		LapTimeManager.MinuteCount = 0;
		LapTimeManager.SecondCount = 0;
		LapTimeManager.MilliCount = 0;
		LapCounter.GetComponent<TMP_Text>().text = "" + VueltasHechas;

		HalfLapTrig.SetActive(true);
		LapCompleteTrig.SetActive(false);
	}

}