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
			if (m_handle)
			{
				// subscribe
				m_handle.onHoverEnter += OnHoverEnter;
				m_handle.onHoverExit += OnHoverExit;
			}
		}
	}

	private void Awake()
	{
		Find();
	}

	private void OnDestroy()
	{
		if (m_handle)
		{
			// unsubscribe
			m_handle.onHoverEnter -= OnHoverEnter;
			m_handle.onHoverExit -= OnHoverExit;
		}
	}

	public HandleController handleController { get; private set; }
	public bool isHovered { get; private set; }
	public event Action<ButtonHandle, HandleController> onClick;

	private void OnHoverEnter(Handle handle, HandleController handleController)
	{
		isHovered = true;
		this.handleController = handleController;
	}

	private void OnHoverExit(Handle handle, HandleController handleController)
	{
		isHovered = false;
		this.handleController = null;
	}

	public void InvokeClick()
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
		if (isHovered)
		{
			if (Input.GetButtonDown("Fire1"))
			{
				InvokeClick();
			}
		}
	}
}
