using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <author>Elijah Shadbolt</author>
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(V2_ButtonHandle))]
public class V2_PickUpHandle : MonoBehaviour
{
	public V2_ButtonHandle buttonHandle { get; private set; }
	public Rigidbody rb { get; private set; }
	public Collider[] colliders { get; private set; }
	public V2_PickUpController controller { get; private set; } = null;
	public bool isPickedUp => controller;
	public event Action<V2_PickUpHandle, V2_PickUpController> onPickedUp;
	public event Action<V2_PickUpHandle, V2_PickUpController> onDropped;
	public string description = "Undescribable Object";

	private void Awake()
	{
		buttonHandle = GetComponent<V2_ButtonHandle>();
		buttonHandle.onClick += OnClick;
		rb = GetComponent<Rigidbody>();
		colliders = GetComponentsInChildren<Collider>();
	}

	private void OnClick(V2_ButtonHandle buttonHandle, V2_HandleController handleController)
	{
		controller = handleController.GetComponent<V2_PickUpController>();
		if (controller)
		{
			buttonHandle.enabled = false;
			controller.InternalOnPickedUp(this);
			rb.isKinematic = true;
			foreach (var c in colliders)
			{
				Physics.IgnoreCollision(controller.cc, c);
			}
			onPickedUp?.Invoke(this, controller);
		}
	}

	public void Drop()
	{
		const float hw = 0.7f;
		const float radius = 0.1f;
		var hc = controller.handleController;
		var ray = new Ray(hc.transform.position, hc.transform.forward);
		var maxDistance = Mathf.Max(radius, hc.maxDistance * hw);
		var hits = Physics.SphereCastAll(
			ray,
			radius,
			maxDistance,
			hc.handleMask,
			QueryTriggerInteraction.Ignore)
			.Where(hit => !colliders.Contains(hit.collider))
			.OrderBy(hit => hit.distance);
		if (hits.Any())
		{
			var hit = hits.First();
			var a = ray.GetPoint(Mathf.Max(radius, hit.distance));
			var b = hit.point + hit.normal * radius;
			rb.position = (a + b) / 2.0f;
		}
		else
		{
			rb.position = ray.GetPoint(maxDistance);
		}

		try
		{
			controller.InternalOnDropped(this);
			buttonHandle.enabled = true;
			rb.isKinematic = false;
			foreach (var c in colliders)
			{
				Physics.IgnoreCollision(controller.cc, c, false);
			}
			onDropped?.Invoke(this, controller);
		}
		finally
		{
			controller = null;
		}
	}

	private void Update()
	{
		if (isPickedUp)
		{
			if (Input.GetButtonDown("Fire2"))
			{
				Drop();
			}
		}
	}

	//private void FixedUpdate()
	//{
	//	if (isPickedUp)
	//	{
	//		rb.MovePosition(controller.handPoint.position);
	//		rb.MoveRotation(controller.handPoint.rotation);
	//	}
	//}

	private void LateUpdate()
	{
		if (isPickedUp)
		{
			transform.position = controller.handPoint.position;
			transform.rotation = controller.handPoint.rotation;
		}
	}
}
