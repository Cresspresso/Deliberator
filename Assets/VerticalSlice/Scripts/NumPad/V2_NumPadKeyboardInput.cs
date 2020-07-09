using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(V2_Handle))]
public class V2_NumPadKeyboardInput : MonoBehaviour
{
	[SerializeField]
	private V2_NumPad m_pad;
	public V2_NumPad pad {
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
			m_pad = GetComponentInParent<V2_NumPad>();
		}
	}

	private V2_Handle m_handle;
	public V2_Handle handle {
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
			m_handle = GetComponent<V2_Handle>();
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

	private void OnHoverEnter(V2_Handle handle, V2_HandleController handleController)
	{
		pad.OnHoverEnter(handleController);
	}

	private void OnHoverExit(V2_Handle handle, V2_HandleController handleController)
	{
		pad.OnHoverExit(handleController);
	}
}
