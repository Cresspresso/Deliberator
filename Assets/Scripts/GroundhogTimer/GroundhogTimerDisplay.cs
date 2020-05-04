using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <author>Elijah Shadbolt</author>
public class GroundhogTimerDisplay : MonoBehaviour
{
	public Text minutesText;
	public Text secondsText;
	public Text fracText;

	private void Awake()
	{
		var gd = FindObjectOfType<GroundhogControl>();
		gd.RemainingDurationChanged -= OnRemainingDurationChanged;
		gd.RemainingDurationChanged += OnRemainingDurationChanged;
	}

	private void OnRemainingDurationChanged(float remainingDuration)
	{
		minutesText.text = Mathf.FloorToInt(remainingDuration / 60).ToString("D2");
		secondsText.text = Mathf.FloorToInt((int)(remainingDuration % 60)).ToString("D2");
		fracText.text = Mathf.FloorToInt((int)((100 * remainingDuration) % 100)).ToString("D2");
	}

	private void OnDestroy()
	{
		var gd = FindObjectOfType<GroundhogControl>();
		if (gd)
		{
			gd.RemainingDurationChanged -= OnRemainingDurationChanged;
		}
	}
}
