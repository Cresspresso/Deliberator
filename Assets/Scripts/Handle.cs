using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct HandleHoverInfo
{
	public string description;
	public Sprite sprite;

	public HandleHoverInfo(string description, Sprite sprite = null)
	{
		this.description = description;
		this.sprite = sprite;
	}
}

/// <author>Elijah Shadbolt</author>
public class Handle : MonoBehaviour
{
	public HandleController controller { get; private set; }
	public event Action<Handle, HandleController> onHoverEnter;
	public event Action<Handle, HandleController> onHoverExit;
	public event Action<Handle, HandleHoverInfo> onHoverInfoChanged;

	[SerializeField]
	private HandleHoverInfo m_hoverInfo = new HandleHoverInfo("Touch", null);
	public HandleHoverInfo hoverInfo {
		get => m_hoverInfo;
		set
		{
			m_hoverInfo = value;
			try
			{
				onHoverInfoChanged?.Invoke(this, m_hoverInfo);
			}
			catch (Exception e)
			{
				Debug.LogError(e, this);
			}
		}
	}

	public void HandleControllerHoverEnter(HandleController handleController)
	{
		controller = handleController;
		try
		{
			onHoverEnter?.Invoke(this, handleController);
		}
		catch (Exception e)
		{
			Debug.LogError(e, this);
		}
	}

	public void HandleControllerHoverExit(HandleController handleController)
	{
		controller = handleController;
		try
		{
			onHoverExit?.Invoke(this, handleController);
		}
		catch (Exception e)
		{
			Debug.LogError(e, this);
		}
	}

	// to show enabled checkbox
	private void Start()
	{
		
	}
}
