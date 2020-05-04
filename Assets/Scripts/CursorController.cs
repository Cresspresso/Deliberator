using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <author>Elijah Shadbolt</author>
public class CursorController : MonoBehaviour
{
	[SerializeField]
	private FirstPersonCharacterController m_fpcc;
	public FirstPersonCharacterController fpcc {
		get
		{
			if (!m_fpcc)
			{
				m_fpcc = GetComponent<FirstPersonCharacterController>();
				if (!m_fpcc)
				{
					m_fpcc = FindObjectOfType<FirstPersonCharacterController>();
				}
			}
			return m_fpcc;
		}
	}

	[SerializeField]
	private HandleController m_hc;
	public HandleController hc {
		get
		{
			if (!m_hc)
			{
				m_hc = GetComponentInChildren<HandleController>();
				if (!m_fpcc)
				{
					m_hc = FindObjectOfType<HandleController>();
				}
			}
			return m_hc;
		}
	}

	private void HideCursor(bool hideCursor)
	{
		fpcc.isLookInputEnabled = hideCursor;
		fpcc.isMoveInputEnabled = hideCursor;
		fpcc.isJumpInputEnabled = hideCursor;
		hc.enabled = hideCursor;

		Cursor.lockState = hideCursor ? CursorLockMode.Locked : CursorLockMode.None;
		Cursor.visible = !hideCursor;
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
