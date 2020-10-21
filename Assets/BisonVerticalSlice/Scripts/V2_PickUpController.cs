﻿using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using TSingleton = V2_Singleton<V2_PickUpController>;

/// <summary>
///		Manages what the player is currently holding in their hand.
/// </summary>
/// 
/// <changelog>
///		<log author="Elijah Shadbolt" date="13/10/2020">
///			<para>Added Singleton instance property.</para>
///		</log>
///		<log author="Elijah Shadbolt" date="21/10/2020">
///			<para>Added a poor fix for left hand torch.</para>
///		</log>
/// </changelog>
/// 
[RequireComponent(typeof(V2_HandleController))]
public class V2_PickUpController : MonoBehaviour
{
	public V2_HandleController handleController { get; private set; }
	public CharacterController cc { get; private set; }

	public V2_PickUpHandle currentPickedUpHandle { get; private set; }
	public V2_PickUpHandle currentPickedUpHandleLeft { get; private set; }

	public event Action<V2_PickUpController, V2_PickUpHandle> onPickedUp;
	public event Action<V2_PickUpController, V2_PickUpHandle> onDropped;

	public event Action<V2_PickUpController, V2_PickUpHandle> onPickedUpLeft;
	public event Action<V2_PickUpController, V2_PickUpHandle> onDroppedLeft;

	public Transform handPoint;

	public static V2_PickUpController instance => TSingleton.instance;

	private void Awake()
	{
		TSingleton.OnAwake(this, V2_SingletonDuplicateMode.Ignore);

		handleController = GetComponent<V2_HandleController>();
		cc = GetComponentInParent<CharacterController>();
	}

	public void InternalOnPickedUp(V2_PickUpHandle sender)
	{
		if (sender.isLeftHandItem)
		{
			if (currentPickedUpHandleLeft)
			{
				currentPickedUpHandleLeft.Drop();
			}

			currentPickedUpHandleLeft = sender;
			onPickedUpLeft?.Invoke(this, sender);
		}
		else
		{
			if (currentPickedUpHandle)
			{
				currentPickedUpHandle.Drop();
			}

			currentPickedUpHandle = sender;
			onPickedUp?.Invoke(this, sender);
		}
	}

	public void InternalOnDropped(V2_PickUpHandle sender)
	{
		if (currentPickedUpHandle == sender)
		{
			currentPickedUpHandle = null;
			onDropped?.Invoke(this, sender);
		}
		
		if (currentPickedUpHandleLeft == sender)
		{
			currentPickedUpHandleLeft = null;
			onDroppedLeft?.Invoke(this, sender);
		}
	}
}
