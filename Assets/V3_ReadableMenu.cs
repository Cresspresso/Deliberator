﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Contains Readable Menu Components and functions.
/// </summary>
/// <author>Lorenzo Sae-Phoo Zemp</author>
public class V3_ReadableMenu : MonoBehaviour
{
	//stores readable menu panel object
#pragma warning disable CS0649
	[SerializeField]
	private GameObject m_readableMenuPanel;
#pragma warning restore CS0649
	public GameObject readableMenuPanel => m_readableMenuPanel;

	//stores textmeshpro object
#pragma warning disable CS0649
	[SerializeField]
	private TextMeshProUGUI m_readableTextMeshPro;
#pragma warning restore CS0649
	public TextMeshProUGUI readableText => m_readableTextMeshPro;

	//get set and get cursor controller
	private V2_CursorController m_cursorController;
	private V2_CursorController cursorController
	{
		get
		{
			if (!m_cursorController)
			{
				m_cursorController = FindObjectOfType<V2_CursorController>();
			}
			return m_cursorController;
		}
	}

	/// <summary>
	/// Sets text mesh pro text 
	/// </summary>
	/// <param name="_newText"></param>
	public void SetText(string _newText)
	{
		readableText.text = _newText;
	}

	/// <summary>
	/// Follows V2_PauseMenu Unpause function but is specifically for readables UI
	/// </summary>
	public void Unpause()
	{
		Time.timeScale = 1.0f;
		cursorController.enabled = true;
		readableMenuPanel.SetActive(false);
		readableText.text = "";
	}

	/// <summary>
	/// Follows V2_PauseMenu pause function but is specifically for readables UI
	/// </summary>
	public void Pause()
	{
		Time.timeScale = 0.0f;
		cursorController.enabled = false;
		readableMenuPanel.SetActive(true);
	}
}
