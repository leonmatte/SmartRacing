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

	public GameObject[] Cars = new GameObject[4];

	public String[] car_scores = new string[4];

	public String car_score;


	void OnTriggerEnter()
	{
		VueltasHechas += 1;
		if (LapTimeManager.SecondCount <= 9)
		{
			SecondDisplay.GetComponent<TMP_Text>().text = "0" + LapTimeManager.SecondCount + ".";
			String seconds = "0"+LapTimeManager.SecondCount+".";
			car_score.Insert(1, seconds);
			Debug.Log(car_score);
		}
		else
		{
			SecondDisplay.GetComponent<TMP_Text>().text = "" + LapTimeManager.SecondCount + ".";
			//car_score += "" + LapTimeManager.SecondCount + ".";
		}

		if (LapTimeManager.MinuteCount <= 9)
		{
			MinuteDisplay.GetComponent<TMP_Text>().text = "0" + LapTimeManager.MinuteCount + ".";
		//	car_score += "0" + LapTimeManager.MinuteCount + ".";
		}
		else
		{
			MinuteDisplay.GetComponent<TMP_Text>().text = "" + LapTimeManager.MinuteCount + ".";
			//car_score += "" + LapTimeManager.MinuteCount + ".";
		}

		Debug.Log(VueltasHechas);
		Debug.Log(car_score);

		MilliDisplay.GetComponent<TMP_Text>().text = "" + LapTimeManager.MilliCount;
		//car_score = "" + LapTimeManager.MilliCount.ToString;

		LapTimeManager.MinuteCount = 0;
		LapTimeManager.SecondCount = 0;
		LapTimeManager.MilliCount = 0;
		LapCounter.GetComponent<Text>().text = "" + VueltasHechas;

		LapCompleteTrig.SetActive(false);
		HalfLapTrig.SetActive(true);
	}

}