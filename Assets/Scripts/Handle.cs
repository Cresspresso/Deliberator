using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

/// <author>Elijah Shadbolt</author>
public class Handle : MonoBehaviour
{
	public HandleController controller { get; private set; }
	public event Action<Handle, HandleController> onHoverEnter;
	public event Action<Handle, HandleController> onHoverExit;
	public event Action<Handle, string> onDescriptionChanged;

	[SerializeField]
	private string m_description = "Touch";
	public string description {
		get => m_description;
		set
		{
			m_description = value;
			try
			{
				onDescriptionChanged?.Invoke(this, m_description);
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
}
