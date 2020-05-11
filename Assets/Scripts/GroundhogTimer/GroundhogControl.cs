using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <author>Elijah Shadbolt</author>
public class GroundhogControl : MonoBehaviour
{
	[SerializeField]
	private float m_remainingDuration = 10.0f;

	public float remainingDuration {
		get => m_remainingDuration;
		set
		{
			m_remainingDuration = Mathf.Max(value, 0.0f);
			RemainingDurationChanged?.Invoke(m_remainingDuration);
		}
	}
	public event Action<float> RemainingDurationChanged;

	public bool hasFinished { get; private set; } = false;
	public event Action Finished;

	private void Update()
	{
		remainingDuration -= Time.deltaTime;
		if (remainingDuration <= 0.0f)
		{
			Finish();
		}
	}

	private void Finish()
	{
		hasFinished = true;

		try
		{
			Finished?.Invoke();
		}
		catch (Exception e)
		{
			Debug.LogException(e);
		}

		var cap = GetComponent<RewindCapture>();
		if (cap)
		{
			cap.PresentRewindCutscene();
		}
		else
		{
			Debug.LogError("RewindCapture not found", this);
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		}
	}

	public void PlayerDied()
	{
		Finish();
	}
}
