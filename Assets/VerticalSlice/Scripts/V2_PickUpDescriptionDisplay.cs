using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <author>Elijah Shadbolt</author>
public class V2_PickUpDescriptionDisplay : MonoBehaviour
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
			var controller = FindObjectOfType<V2_PickUpController>();
			controller.onPickedUp += OnPickedUp;
			controller.onDropped += OnDropped;
		}
	}

	private void OnDestroy()
	{
		var controller = FindObjectOfType<V2_PickUpController>();
		if (controller)
		{
			controller.onPickedUp -= OnPickedUp;
			controller.onDropped -= OnDropped;
		}
	}

	private void OnPickedUp(V2_PickUpController controller, V2_PickUpHandle handle)
	{
		visuals.SetActive(true);
		textElement.text = handle.description;
	}

	private void OnDropped(V2_PickUpController controller, V2_PickUpHandle handle)
	{
		visuals.SetActive(false);
	}
}
