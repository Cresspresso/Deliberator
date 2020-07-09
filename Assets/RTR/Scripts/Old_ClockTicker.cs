using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Old_ClockTicker : MonoBehaviour
{
	public Old_GroundhogDay groundhogDay;
	public Text secondsText;
	public Text decimalText;

	private void Awake()
	{
		if (!groundhogDay)
		{
			groundhogDay = FindObjectOfType<Old_GroundhogDay>();
		}
		Debug.Assert(groundhogDay, "groundhogDay is null", this);
	}

	private void Update()
	{
		float t = groundhogDay.remainingTime;
		secondsText.text = string.Format("{0:00}", System.Math.Truncate(t));
		decimalText.text = string.Format("{0:00}", (t % 1) * 100);
	}
}
