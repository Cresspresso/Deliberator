using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Old_KeyCardReader))]
public class Old_GroundhogKcr : MonoBehaviour
{
	public Old_KeyCardReader interactable { get; private set; }

	public Old_GroundhogDay groundhogDay;

	private void Awake()
	{
		interactable = GetComponent<Old_KeyCardReader>();

		if (!groundhogDay)
		{
			groundhogDay = FindObjectOfType<Old_GroundhogDay>();
		}
		Debug.Assert(groundhogDay, "groundhogDay is null", this);

		interactable.interactEventComponent.onInteract.AddListener(OnInteract);
	}

	private void OnDestroy()
	{
		interactable.interactEventComponent.onInteract.RemoveListener(OnInteract);
	}

	private void OnInteract(Old_InteractEventArgs eventArgs)
	{
		groundhogDay.RestartTimeLoop();
	}
}
