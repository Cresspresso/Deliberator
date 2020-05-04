using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

/// <author>Elijah Shadbolt</author>
public class PauseMenu : MonoBehaviour
{
	[SerializeField]
	private CursorController m_cursorController;
	public CursorController cursorController {
		get
		{
			if (!m_cursorController)
			{
				m_cursorController = FindObjectOfType<CursorController>();
			}
			return m_cursorController;
		}
	}

	[SerializeField]
	private GameObject m_visuals;
	public GameObject visuals => m_visuals;

	private bool m_isPaused;
	public bool isPaused {
		get => m_isPaused;
		set
		{
			m_isPaused = value;
			Time.timeScale = m_isPaused ? 0.0f : 1.0f;
			cursorController.enabled = !m_isPaused;
			visuals.SetActive(m_isPaused);
		}
	}

	private bool subscribed = false;

	private void Awake()
	{
		if (!subscribed)
		{
			subscribed = true;
			isPaused = false;
		}
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			isPaused = !isPaused;
		}
	}
}
