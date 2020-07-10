using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <author>Elijah Shadbolt</author>
[RequireComponent(typeof(V2_Handle))]
public class V2_ButtonHandle : MonoBehaviour
{
	private V2_Handle m_handle;
	public V2_Handle handle {
		get
		{
			Find();
			return m_handle;
		}
	}
	private void Find()
	{
		if (!m_handle)
		{
			m_handle = GetComponent<V2_Handle>();
		}
	}

	private void Awake()
	{
		Find();
	}

	public event Action<V2_ButtonHandle, V2_HandleController> onClick;
	public UnityEvent onClickEvent = new UnityEvent();

	public void InvokeClick(V2_HandleController handleController)
	{
		try
		{
			onClick?.Invoke(this, handleController);
		}
		catch (Exception e)
		{
			Debug.LogException(e);
		}

		Old_Utils.TryExceptLog(() => onClickEvent.Invoke());
	}

	private void Update()
	{
		if (handle.isHovered)
		{
			if (Input.GetButtonDown("Fire1"))
			{
				InvokeClick(handle.controller);
			}
		}
	}
}
