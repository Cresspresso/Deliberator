using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <author>Elijah Shadbolt</author>
public class PickUpDescriptionDisplay : MonoBehaviour
{
	public Text textElement;
	public GameObject visuals;
	private bool subscribed = false;

	private void Awake()
	{
		if (!subscribed)
		{
			visuals.SetActive(false);
			subscribed = true;
			var controller = FindObjectOfType<PickUpController>();
			controller.onPickedUp += OnPickedUp;
			controller.onDropped += OnDropped;
		}
	}

	private void OnDestroy()
	{
		var controller = FindObjectOfType<PickUpController>();
		if (controller)
		{
			controller.onPickedUp -= OnPickedUp;
			controller.onDropped -= OnDropped;
		}
	}

	private void OnPickedUp(PickUpController controller, PickUpHandle handle)
	{
		visuals.SetActive(true);
		textElement.text = handle.description;
	}

	private void OnDropped(PickUpController controller, PickUpHandle handle)
	{
		visuals.SetActive(false);
	}
}
