using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

/// <changelog>
///		<log author="Elijah Shadbolt" date="22/09/2020">
///			<para>Added singleton instance property.</para>
///		</log>
///		<log author="Elijah Shadbolt" date="29/09/2020">
///			<para>Removed setting `AudioListener.pause` because we need music in the menu.</para>
///		</log>
/// </changelog>
/// 
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
	private GameObject m_deactivateWhenPaused;
#pragma warning restore CS0649
	public GameObject deactivateWhenPaused => m_deactivateWhenPaused;

#pragma warning disable CS0649
	[SerializeField]
	private GameObject m_pauseMenuPanel;
#pragma warning restore CS0649
	public GameObject pauseMenuPanel => m_pauseMenuPanel;

	public bool isPaused { get; private set; }

	public static V2_PauseMenu instance => V2_Singleton<V2_PauseMenu>.instance;



	private static bool GetPauseButtonDown()
	{
//#if UNITY_EDITOR
		return Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.BackQuote) || Input.GetKeyDown(KeyCode.Tilde);
//#else
		//return Input.GetKeyDown(KeyCode.Escape);
//#endif
	}



	public void Pause() => Pause(this.pauseMenuPanel);
	public void Pause(GameObject menuPanel)
	{
		isPaused = true;
		Time.timeScale = 0.0f;
		cursorController.enabled = false;
		pauseMenuBackground.SetActive(true);
		deactivateWhenPaused.SetActive(false);
		menuNavigation.Clear();
		menuNavigation.GoInto(menuPanel);
		//AudioListener.pause = true;
	}

	public void Unpause()
	{
		isPaused = false;
		Time.timeScale = 1.0f;
		cursorController.enabled = true;
		pauseMenuBackground.SetActive(false);
		deactivateWhenPaused.SetActive(true);
		menuNavigation.Clear();
		//AudioListener.pause = false;
	}

	private void Awake()
	{
		V2_Singleton<V2_PauseMenu>.OnAwake(this, V2_SingletonDuplicateMode.Ignore);
		Unpause();
	}

	private void Update()
	{
		if (GetPauseButtonDown() && readableMenu.reading == false)
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
