using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

/// <author>Elijah Shadbolt</author>
[RequireComponent(typeof(V2_HandleController))]
public class V2_PickUpController : MonoBehaviour
{
	public V2_HandleController handleController { get; private set; }
	public CharacterController cc { get; private set; }

	public V2_PickUpHandle currentPickedUpHandle { get; private set; }

	public event Action<V2_PickUpController, V2_PickUpHandle> onPickedUp;
	public event Action<V2_PickUpController, V2_PickUpHandle> onDropped;

	public Transform handPoint;

	private void Awake()
	{
		handleController = GetComponent<V2_HandleController>();
		cc = GetComponentInParent<CharacterController>();
	}

	public void InternalOnPickedUp(V2_PickUpHandle sender)
	{
		currentPickedUpHandle = sender;
		onPickedUp?.Invoke(this, sender);
	}

	public void InternalOnDropped(V2_PickUpHandle sender)
	{
		if (currentPickedUpHandle == sender)
		{
			currentPickedUpHandle = null;
			onDropped?.Invoke(this, sender);
		}
	}
}
