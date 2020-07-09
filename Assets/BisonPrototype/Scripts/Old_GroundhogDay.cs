using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Old_GroundhogDay : MonoBehaviour
{
	// Inspector Fields

	public float duration = 20.0f;
	public float startDelay = 1.0f;
	public float timeLoopRestartDelay = 1.0f;
	public float rewindAnimDuration = 1.0f;


	// Non Serialized Fields

	private float m_elapsedTime;
	public bool isRewinding { get; private set; } = false;
	[HideInInspector]
	public bool isGameEnding = false;


	// Properties

	public float elapsedTime => Mathf.Clamp(m_elapsedTime, 0.0f, duration);
	public float remainingTime => Mathf.Clamp(duration - m_elapsedTime, 0.0f, duration);
	public bool hasEnded => m_elapsedTime >= duration;


	private static Old_GroundhogDay m_instance;
	public static Old_GroundhogDay instance {
		get
		{
			if (!m_instance) { m_instance = FindObjectOfType<Old_GroundhogDay>(); }
			return m_instance;
		}
	}


	// Unity Methods

	private void Start()
	{
		m_elapsedTime = -startDelay;
	}

	private void OnDestroy()
	{
		if (instance == this) { m_instance = null; }
	}

	public void RestartTimeLoop()
	{
		if (m_elapsedTime < duration)
		{
			m_elapsedTime = duration;
		}
	}

	private void LateUpdate()
	{
		m_elapsedTime += Time.deltaTime;
		if (isRewinding)
		{
			if (m_elapsedTime >= duration + timeLoopRestartDelay + rewindAnimDuration)
			{
				SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
			}
		}
		else
		{
			if (!isGameEnding && m_elapsedTime >= duration + timeLoopRestartDelay)
			{
				isGameEnding = true;
				isRewinding = true;
			}
		}
	}
}
