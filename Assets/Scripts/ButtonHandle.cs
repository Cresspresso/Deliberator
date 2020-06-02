using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <author>Elijah Shadbolt</author>
[RequireComponent(typeof(Handle))]
public class ButtonHandle : MonoBehaviour
{
	private Handle m_handle;
	public Handle handle {
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
			m_handle = GetComponent<Handle>();
		}
	}

	private void Awake()
	{
		Find();
	}

	public event Action<ButtonHandle, HandleController> onClick;

	public void InvokeClick(HandleController handleController)
	{
		try
		{
			onClick?.Invoke(this, handleController);
		}
		catch (Exception e)
		{
			Debug.LogException(e);
		}
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
