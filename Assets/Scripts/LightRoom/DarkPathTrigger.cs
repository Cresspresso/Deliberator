using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkPathTrigger : PathTrigger
{
	public GameObject visuals;
	public DarkPathTrigger previous { get; private set; }

	protected override void Start()
	{
		base.Start();

		visuals.SetActive(false);

		if (transform.parent)
		{
			int i = transform.GetSiblingIndex();
			if (i > 0)
			{
				var sibling = transform.parent.GetChild(i - 1);
				previous = sibling.GetComponent<DarkPathTrigger>();
			}
		}
	}

	public override bool IsValidCollider(Collider other)
	{
		if (!base.IsValidCollider(other)) { return false; }
		var player = other.GetComponentInParent<FirstPersonCharacterController>();
		if (!player) { return false; }
		var pickUpController = player.GetComponentInChildren<PickUpController>();
		if (!pickUpController) { return false; }
		var pickUpHandle = pickUpController.currentPickedUpHandle;
		if (!pickUpHandle) { return false; }
		if (!pickUpHandle.CompareTag("Ultra Violet Flashight Pickup")) { return false; }
		return true;
	}

	protected override void OnValidTriggerEnter(Collider first)
	{
		if (!previous || !previous.isActiveAndEnabled
			|| previous.isVisible)
		{
			base.OnValidTriggerEnter(first);
		}
	}

	public bool isVisible { get; private set; } = false;
	public void SetVisible(bool value)
	{
		isVisible = value;
		visuals.SetActive(value);
	}
}
