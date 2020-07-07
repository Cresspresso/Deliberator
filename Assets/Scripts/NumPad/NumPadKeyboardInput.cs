using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Handle))]
public class NumPadKeyboardInput : MonoBehaviour
{
	[SerializeField]
	private NumPad m_pad;
	public NumPad pad {
		get
		{
			FindNumPad();
			return m_pad;
		}
	}
	private void FindNumPad()
	{
		if (!m_pad)
		{
			m_pad = GetComponentInParent<NumPad>();
		}
	}

	private Handle m_handle;
	public Handle handle {
		get
		{
			FindHandle();
			return m_handle;
		}
	}
	private void FindHandle()
	{
		if (!m_handle)
		{
			m_handle = GetComponent<Handle>();
			if (m_handle)
			{
				m_handle.onHoverEnter += OnHoverEnter;
				m_handle.onHoverExit += OnHoverExit;
			}
		}
	}

	private void Awake()
	{
		FindHandle();
	}

	private void OnHoverEnter(Handle handle, HandleController handleController)
	{
		pad.OnHoverEnter(handleController);
	}

	private void OnHoverExit(Handle handle, HandleController handleController)
	{
		pad.OnHoverExit(handleController);
	}
}
