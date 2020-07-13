using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <author>Elijah Shadbolt</author>
public class V2_HandleDescriptionDisplay : MonoBehaviour
{
	public Text[] textElements;
	public Image image;
	public GameObject visuals;
	private RectTransform rt;
	private Canvas canvas;
	private CanvasScaler canvasScaler;
	public Sprite idleSprite;
	public Sprite interactSprite;
	private bool subscribed = false;

	private V2_Handle currentHandle;

	private void Awake()
	{
		rt = visuals.GetComponent<RectTransform>();
		Debug.Assert(rt, this);

		canvas = GetComponentInParent<Canvas>();
		canvasScaler = canvas.GetComponent<CanvasScaler>();

		if (!subscribed)
		{
			visuals.SetActive(false);
			subscribed = true;
			var handleController = FindObjectOfType<V2_HandleController>();
			handleController.onHoverEnter += OnHoverEnter;
			handleController.onHoverExit += OnHoverExit;
		}
	}

	private void OnDestroy()
	{
		var handleController = FindObjectOfType<V2_HandleController>();
		if (handleController)
		{
			handleController.onHoverEnter -= OnHoverEnter;
			handleController.onHoverExit -= OnHoverExit;
		}
	}

	private void OnHoverEnter(V2_HandleController handleController, V2_Handle handle)
	{
		currentHandle = handle;
		handle.onHoverInfoChanged += OnHoverInfoChanged;
		OnHoverInfoChanged(handle, handle.hoverInfo);
		visuals.SetActive(true);
	}

	private void OnHoverExit(V2_HandleController handleController, V2_Handle handle)
	{
		currentHandle = null;
		handle.onHoverInfoChanged -= OnHoverInfoChanged;
		visuals.SetActive(false);
		image.sprite = idleSprite;
	}

	private void OnHoverInfoChanged(V2_Handle handle, V2_HandleHoverInfo hoverInfo)
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
