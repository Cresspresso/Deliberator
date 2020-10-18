using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
///		<para>A Handle that can be picked up by the player, carried around, and dropped.</para>
/// </summary>
/// 
/// <changelog>
///		<log author="Elijah Shadbolt" date="29/09/2020">
///			<para>Added comments.</para>
///			<para>Added inspector field for `radius`.</para>
///			<para>Added <see cref="OnDrawGizmosSelected"/> to show `radius`.</para>
///		</log>
///		<log author="Elijah Shadbolt" date="29/09/2020">
///			<para>Drop the item in front of the player instead of on their head.</para>
///		</log>
/// </changelog>
/// 
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

#pragma warning disable CS0649
	[Tooltip("When the player drops this item, how big is it (for placing on tables, etc)?")]
	[SerializeField]
	private float m_radius = 0.1f;
	public float radius => m_radius;
#pragma warning restore CS0649

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
			rb.transform.SetParent(controller.handPoint);
			rb.transform.position = controller.handPoint.position;
			rb.transform.rotation = controller.handPoint.rotation;
			foreach (var c in colliders)
			{
				c.enabled = false;
				//Physics.IgnoreCollision(controller.cc, c);
			}
			onPickedUp?.Invoke(this, controller);
		}
	}

	public void Drop()
	{
		if (!controller) { return; }

		foreach (var c in colliders)
		{
			c.enabled = true;
			//Physics.IgnoreCollision(controller.cc, c, false);
		}
		transform.SetParent(null);

		const float hw = 0.7f;
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
			float minDistance = V2_FirstPersonCharacterController.instance.cc.radius - 0.01f;
			if (hit.distance <= minDistance)
			{
				transform.position = ray.GetPoint(minDistance);
				Debug.DrawRay(transform.position, Vector3.up, Color.blue, 3.0f);
			}
			else if (hit.distance - radius < minDistance)
			{
				var end = hit.distance + radius;
				var avg = 0.5f * (end + minDistance);
				transform.position = ray.GetPoint(avg);
				Debug.DrawRay(transform.position, Vector3.up, Color.green, 3.0f);

				//
				// origin    min      hit.distance    hit.point
				// |---------|--------|-------------/-|
				//        |-----------|-----------|
				//        d-radius    d           d+radius
				//
			}
			else
			{
				transform.position = ray.GetPoint(hit.distance);
				Debug.DrawRay(transform.position, Vector3.up, Color.red, 3.0f);
			}
		}
		else
		{
			transform.position = ray.GetPoint(maxDistance);
		}

		rb.isKinematic = false;

		try
		{
			controller.InternalOnDropped(this);
			buttonHandle.enabled = true;
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
			if (Input.GetButtonDown("Fire2") && !V2_PauseMenu.instance.isPaused)
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

	//private void LateUpdate()
	//{
	//	if (isPickedUp)
	//	{
	//		transform.position = controller.handPoint.position;
	//		transform.rotation = controller.handPoint.rotation;
	//	}
	//}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.green;
		Gizmos.DrawWireSphere(transform.position, radius);
	}
}
