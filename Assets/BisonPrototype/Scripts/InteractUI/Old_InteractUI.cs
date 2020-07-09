using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Old_InteractUI : MonoBehaviour
{
	public CanvasScaler canvasScaler { get; private set; }

	public Old_PlayerHand playerHand;

	public bool onlyIfInteractable = false;

	public GameObject visuals;
	public Text[] textElements = new Text[1];
	private bool m_visible;
	public bool visible {
		get => m_visible;
		set
		{
			m_visible = value;
			visuals.SetActive(value);
		}
	}

	private void Awake()
	{
		canvasScaler = GetComponentInParent<CanvasScaler>();

		if (!playerHand)
		{
			playerHand = FindObjectOfType<Old_PlayerHand>();
		}
	}

	private void Start()
	{
		visible = false;
	}

	private void LateUpdate()
	{
		var transform = (RectTransform)this.transform;

		var interactable = onlyIfInteractable
			? playerHand.GetClosestInteractable()
			: playerHand.GetClosestHovered();

		if (interactable && interactable.showHoverInfo)
		{
			if (!visible)
			{
				visible = true;
			}


			// update text

			var re = playerHand.GetNotInteractableReason(interactable);

			if (textElements != null)
			{
				var text = re == null
					? interactable.hoverDescription
					: interactable.hoverNotInteractableDescription;

				foreach (var element in textElements)
				{
					if (element)
					{
						element.text = text;
					}
				}
			}


			// update position

			var worldPos = interactable.location.position;
			var screenPos = Camera.main.WorldToViewportPoint(worldPos) * canvasScaler.referenceResolution;
			transform.anchoredPosition = screenPos;
		}
		else
		{
			if (visible)
			{
				visible = false;
			}
		}
	}
}
