﻿using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

/// <author>Elijah Shadbolt</author>
public class V2_CursorController : MonoBehaviour
{
	[SerializeField]
	private V2_FirstPersonCharacterController m_fpcc;
	public V2_FirstPersonCharacterController fpcc {
		get
		{
			if (!m_fpcc)
			{
				m_fpcc = GetComponent<V2_FirstPersonCharacterController>();
				if (!m_fpcc)
				{
					m_fpcc = FindObjectOfType<V2_FirstPersonCharacterController>();
				}
			}
			return m_fpcc;
		}
	}

	[SerializeField]
	private V2_HandleController m_hc;
	public V2_HandleController hc {
		get
		{
			if (!m_hc)
			{
				m_hc = GetComponentInChildren<V2_HandleController>();
				if (!m_fpcc)
				{
					m_hc = FindObjectOfType<V2_HandleController>();
				}
			}
			return m_hc;
		}
	}

	[SerializeField]
	private V3_Crouch m_crouch;
	public V3_Crouch crouch {
		get
		{
			if (!m_crouch)
			{
				m_crouch = GetComponent<V3_Crouch>();
				if (!m_crouch)
				{
					m_crouch = FindObjectOfType<V3_Crouch>();
				}
			}
			return m_crouch;
		}
	}

	private void HideCursor(bool hideCursor)
	{
		fpcc.isInputEnabled = hideCursor;
		hc.enabled = hideCursor;
		UncheckedSetCursorHidden(hideCursor);
	}

	public static void UncheckedSetCursorHidden(bool hidden)
	{
		Cursor.lockState = hidden ? CursorLockMode.Locked : CursorLockMode.None;
		Cursor.visible = !hidden;
	}

	private void Awake()
	{
		HideCursor(true);
	}

	private void OnEnable()
	{
		HideCursor(true);
	}

	private void OnDisable()
	{
		HideCursor(false);
	}
}
