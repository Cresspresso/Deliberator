using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct V2_HandleHoverInfo
{
	public string description;
	public Sprite sprite;

	public V2_HandleHoverInfo(string description, Sprite sprite = null)
	{
		this.description = description;
		this.sprite = sprite;
	}
}

/// <author>Elijah Shadbolt</author>
public class V2_Handle : MonoBehaviour
{
	public V2_HandleController controller { get; private set; }
	public bool isHovered { get; private set; }
	public event Action<V2_Handle, V2_HandleController> onHoverEnter;
	public event Action<V2_Handle, V2_HandleController> onHoverExit;
	public event Action<V2_Handle, V2_HandleHoverInfo> onHoverInfoChanged;

	/// <author>Elijah Shadbolt</author>
	/// <stage>Alpha Production</stage>
#pragma warning disable CS0649
	[SerializeField]
	private Transform m_handlePoint;
	public Transform handlePoint => m_handlePoint;
#pragma warning restore CS0649




	[SerializeField]
	private V2_HandleHoverInfo m_hoverInfo = new V2_HandleHoverInfo("Touch", null);
	public V2_HandleHoverInfo hoverInfo {
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

	public void HandleControllerHoverEnter(V2_HandleController handleController)
	{
		controller = handleController;
		isHovered = true;
		try
		{
			onHoverEnter?.Invoke(this, handleController);
		}
		catch (Exception e)
		{
			Debug.LogError(e, this);
		}
	}

	public void HandleControllerHoverExit(V2_HandleController handleController)
	{
		controller = handleController;
		isHovered = false;
		try
		{
			onHoverExit?.Invoke(this, handleController);
		}
		catch (Exception e)
		{
			Debug.LogError(e, this);
		}
		controller = null;
	}

	// to show enabled checkbox
	private void Start()
	{
		
	}

	private void OnDisable()
	{
		isHovered = false;
		if (controller)
		{
			controller.InternalOnHandleDisabled(this);
			controller = null;
		}
	}
}
