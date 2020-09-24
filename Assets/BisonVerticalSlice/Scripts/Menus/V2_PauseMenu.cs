using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

/// <author>Elijah Shadbolt</author>
public class V2_PauseMenu : MonoBehaviour
{
	[SerializeField]
	private V2_MenuNavigation m_menuNavigation;
	public V2_MenuNavigation menuNavigation {
		get
		{
			if (!m_menuNavigation)
			{
				m_menuNavigation = GetComponent<V2_MenuNavigation>();
			}
			return m_menuNavigation;
		}
	}

	[SerializeField]
	private V2_CursorController m_cursorController;
	public V2_CursorController cursorController {
		get
		{
			if (!m_cursorController)
			{
				m_cursorController = FindObjectOfType<V2_CursorController>();
			}
			return m_cursorController;
		}
	}

	private V3_ReadableMenu m_readableMenu;
	public V3_ReadableMenu readableMenu
	{
		get
		{
			if(!m_readableMenu)
			{
				m_readableMenu = FindObjectOfType<V3_ReadableMenu>();
			}
			return m_readableMenu;
		}
	}

#pragma warning disable CS0649
	[SerializeField]
	private GameObject m_pauseMenuBackground;
#pragma warning restore CS0649
	public GameObject pauseMenuBackground => m_pauseMenuBackground;

#pragma warning disable CS0649
	[SerializeField]
	private GameObject m_pauseMenuPanel;
#pragma warning restore CS0649
	public GameObject pauseMenuPanel => m_pauseMenuPanel;

	public bool isPaused { get; private set; }

	public void Pause() => Pause(this.pauseMenuPanel);
	public void Pause(GameObject menuPanel)
	{
		//Debug.Log(readableMenu.reading);
		//if (readableMenu.reading == false) 
		//{
			isPaused = true;
			Time.timeScale = 0.0f;
			cursorController.enabled = false;
			pauseMenuBackground.SetActive(true);
			menuNavigation.Clear();
			menuNavigation.GoInto(menuPanel);
			AudioListener.pause = true;
		//}
	}

	public void Unpause()
	{
		isPaused = false;
		Time.timeScale = 1.0f;
		cursorController.enabled = true;
		pauseMenuBackground.SetActive(false);
		menuNavigation.Clear();
		AudioListener.pause = false;
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
