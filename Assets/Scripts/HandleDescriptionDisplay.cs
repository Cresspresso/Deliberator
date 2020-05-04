using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <author>Elijah Shadbolt</author>
public class HandleDescriptionDisplay : MonoBehaviour
{
	public Text[] textElements;
	private bool subscribed = false;

	private void Awake()
	{
		if (!subscribed)
		{
			gameObject.SetActive(false);
			subscribed = true;
			var handleController = FindObjectOfType<HandleController>();
			handleController.onHoverEnter += OnHoverEnter;
			handleController.onHoverExit += OnHoverExit;
		}
	}

	private void OnDestroy()
	{
		var handleController = FindObjectOfType<HandleController>();
		if (handleController)
		{
			handleController.onHoverEnter -= OnHoverEnter;
			handleController.onHoverExit -= OnHoverExit;
		}
	}

	private void OnHoverEnter(HandleController handleController, Handle handle)
	{
		handle.onDescriptionChanged += OnDescriptionChanged;
		OnDescriptionChanged(handle, handle.description);
		gameObject.SetActive(true);
	}

	private void OnHoverExit(HandleController handleController, Handle handle)
	{
		gameObject.SetActive(false);
		handle.onDescriptionChanged -= OnDescriptionChanged;
	}

	private void OnDescriptionChanged(Handle handle, string description)
	{
		if (textElements != null)
		{
			foreach (var textElement in textElements)
			{
				if (textElement)
				{
					textElement.text = description;
				}
			}
		}
	}
}
