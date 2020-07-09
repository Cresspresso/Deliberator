using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Old_NumPadLight : MonoBehaviour
{
	public Old_NumPadLock pad;
	public Light[] lights = new Light[1];
	public Color normalColor = Color.white;
	public Color correctColor = Color.green;
	public Color incorrectColor = Color.red;

	private const float maxTime = 1.0f;
	private float remainingTime = 0.0f;

	private void Awake()
	{
		pad.onCorrectSubmitted.AddListener(OnCorrectSubmitted);
		pad.onIncorrectSubmitted.AddListener(OnIncorrectSubmitted);
	}

	private void OnDestroy()
	{
		pad.onCorrectSubmitted.RemoveListener(OnCorrectSubmitted);
		pad.onIncorrectSubmitted.RemoveListener(OnIncorrectSubmitted);
	}

	private void Update()
	{
		if (remainingTime > 0.0f)
		{
			remainingTime -= Time.deltaTime;
			if (remainingTime <= 0.0f)
			{
				remainingTime = 0.0f;
				SetLightColor(normalColor);
			}
		}
	}

	private void SetLightColor(Color value)
	{
		foreach (var light in lights)
		{
			light.color = value;
		}
	}

	private void OnCorrectSubmitted()
	{
		SetLightColor(correctColor);
		remainingTime = maxTime;
	}

	private void OnIncorrectSubmitted()
	{
		SetLightColor(incorrectColor);
		remainingTime = maxTime;
	}
}
