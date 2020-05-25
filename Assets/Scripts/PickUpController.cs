using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

/// <author>Elijah Shadbolt</author>
[RequireComponent(typeof(HandleController))]
public class PickUpController : MonoBehaviour
{
	public HandleController handleController { get; private set; }
	public CharacterController cc { get; private set; }

	public PickUpHandle currentPickedUpHandle { get; private set; }

	public event Action<PickUpController, PickUpHandle> onPickedUp;
	public event Action<PickUpController, PickUpHandle> onDropped;

	public Transform handPoint;

	private void Awake()
	{
		handleController = GetComponent<HandleController>();
		cc = GetComponentInParent<CharacterController>();
	}

	public void InternalOnPickedUp(PickUpHandle sender)
	{
		currentPickedUpHandle = sender;
		onPickedUp?.Invoke(this, sender);
	}

	public void InternalOnDropped(PickUpHandle sender)
	{
		if (currentPickedUpHandle == sender)
		{
			currentPickedUpHandle = null;
			onDropped?.Invoke(this, sender);
		}
	}
}
