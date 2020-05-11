using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

/// <author>Elijah Shadbolt</author>
public class PauseMenu : MonoBehaviour
{
	[SerializeField]
	private MenuNavigation m_menuNavigation;
	public MenuNavigation menuNavigation {
		get
		{
			if (!m_menuNavigation)
			{
				m_menuNavigation = GetComponent<MenuNavigation>();
			}
			return m_menuNavigation;
		}
	}

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
	private GameObject m_pauseMenuBackground;
	public GameObject pauseMenuBackground => m_pauseMenuBackground;

	[SerializeField]
	private GameObject m_pauseMenuPanel;
	public GameObject pauseMenuPanel => m_pauseMenuPanel;

	public bool isPaused { get; private set; }

	public void Pause() => Pause(this.pauseMenuPanel);
	public void Pause(GameObject menuPanel)
	{
		isPaused = true;
		Time.timeScale = 0.0f;
		cursorController.enabled = false;
		pauseMenuBackground.SetActive(true);
		menuNavigation.Clear();
		menuNavigation.GoInto(menuPanel);
	}

	public void Unpause()
	{
		isPaused = false;
		Time.timeScale = 1.0f;
		cursorController.enabled = true;
		pauseMenuBackground.SetActive(false);
		menuNavigation.Clear();
	}

	private bool initialised = false;

	private void Awake()
	{
		if (!initialised)
		{
			initialised = true;
			Unpause();
		}
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			if (isPaused)
			{
				menuNavigation.GoBack();
			}
			else
			{
				Pause(pauseMenuPanel);
			}
		}

		if (isPaused && !menuNavigation.Any())
		{
			Unpause();
		}
	}
}
