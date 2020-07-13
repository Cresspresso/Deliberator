﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class V3_HandleInfoDisplay : MonoBehaviour
{
	public Image cursorImage;
	public Sprite idleSprite;
	public Sprite interactSprite;

	public Text textElement;
	public V3_WorldToUI follower;

	private V2_HandleController handleController;

	private void Awake()
	{
		Debug.Assert(follower, this);
		handleController = FindObjectOfType<V2_HandleController>();
		Debug.Assert(handleController, this);
		handleController.onHoverEnter += OnHoverEnter;
		handleController.onHoverExit += OnHoverExit;
	}

	private void Start()
	{
		if (cursorImage)
		{
			cursorImage.sprite = idleSprite;
		}
	}

	private void OnHoverEnter(V2_HandleController handleController, V2_Handle handle)
	{
		handle.onHoverInfoChanged += OnHoverInfoChanged;

		if (handle.handlePoint && handle.handlePoint.gameObject.activeInHierarchy)
		{
			follower.target = handle.handlePoint;
		}
		else
		{
			follower.target = handle.transform;
		}

		OnHoverInfoChanged(handle, handle.hoverInfo);
	}

	private void OnHoverInfoChanged(V2_Handle handle, V2_HandleHoverInfo hoverInfo)
	{
		if (cursorImage)
		{
			cursorImage.sprite = hoverInfo.sprite ? hoverInfo.sprite : interactSprite;
		}

		if (textElement)
		{
			textElement.text = hoverInfo.description;
		}
	}

	private void OnHoverExit(V2_HandleController handleController, V2_Handle handle)
	{
		if (cursorImage)
		{
			cursorImage.sprite = idleSprite;
		}

		if (handle)
		{
			handle.onHoverInfoChanged -= OnHoverInfoChanged;
		}
		follower.target = null;
	}

	private void OnDestroy()
	{
		if (handleController)
		{
			handleController.onHoverEnter -= OnHoverEnter;
			handleController.onHoverEnter -= OnHoverExit;
			if (handleController.hoveredHandle)
			{
				handleController.hoveredHandle.onHoverInfoChanged -= OnHoverInfoChanged;
			}
		}
	}
}
