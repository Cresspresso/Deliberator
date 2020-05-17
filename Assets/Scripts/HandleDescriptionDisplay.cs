using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <author>Elijah Shadbolt</author>
public class HandleDescriptionDisplay : MonoBehaviour
{
	public Text[] textElements;
	public Image image;
	public GameObject visuals;
	public Sprite idleSprite;
	public Sprite interactSprite;
	private bool subscribed = false;

	private void Awake()
	{
		if (!subscribed)
		{
			visuals.SetActive(false);
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
		handle.onHoverInfoChanged += OnHoverInfoChanged;
		OnHoverInfoChanged(handle, handle.hoverInfo);
		visuals.SetActive(true);
	}

	private void OnHoverExit(HandleController handleController, Handle handle)
	{
		handle.onHoverInfoChanged -= OnHoverInfoChanged;
		visuals.SetActive(false);
		image.sprite = idleSprite;
	}

	private void OnHoverInfoChanged(Handle handle, HandleHoverInfo hoverInfo)
	{
		if (image)
		{
			image.sprite = hoverInfo.sprite ? hoverInfo.sprite : interactSprite;
		}

		if (textElements != null)
		{
			foreach (var textElement in textElements)
			{
				if (textElement)
				{
					textElement.text = hoverInfo.description;
				}
			}
		}
	}
}
