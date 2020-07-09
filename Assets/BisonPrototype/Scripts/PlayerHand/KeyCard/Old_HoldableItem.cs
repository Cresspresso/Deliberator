using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Old_Item))]
[RequireComponent(typeof(Rigidbody))]
public class Old_HoldableItem : MonoBehaviour
{
	public Old_Item item { get; private set; }
	public Rigidbody rb { get; private set; }
	public Old_PlayerHoldLocation holdInfo { get; private set; }

	private void Awake()
	{
		item = GetComponent<Old_Item>();
		rb = GetComponent<Rigidbody>();

		item.itemEventComponent.onPickedUp.AddListener(OnPickedUp);
		item.itemEventComponent.onDropped.AddListener(OnDropped);
	}

	private void OnDestroy()
	{
		item.itemEventComponent.onPickedUp.RemoveListener(OnPickedUp);
		item.itemEventComponent.onDropped.RemoveListener(OnDropped);
	}

	private void OnPickedUp(Old_ItemEventArgs eventArgs)
	{
		rb.isKinematic = true;
		holdInfo = eventArgs.holder.GetComponent<Old_PlayerHoldLocation>();
		transform.SetParent(holdInfo.location);
		transform.localPosition = Vector3.zero;
	}

	private void OnDropped(Old_ItemEventArgs eventArgs)
	{
		try
		{
			var cameraTransform = eventArgs.holder.hand.cameraTransform;
			var vec = holdInfo.location.position - cameraTransform.position;
			var ray = new Ray(cameraTransform.position, vec);
			Debug.DrawRay(cameraTransform.position, vec, Color.green, 10.0f);

			var resultDistance = vec.magnitude;
			if (Physics.Raycast(
				ray,
				out var hit,
				resultDistance + 0.01f,
				eventArgs.holder.hand.obstacleMask,
				QueryTriggerInteraction.Ignore))
			{
				const float itemSizeRadius = 0.2f;
				resultDistance = hit.distance - itemSizeRadius;
				Debug.DrawRay(hit.point, Vector3.up, Color.red, 10.0f);
			}

			var dropPoint = ray.GetPoint(resultDistance);
			Debug.DrawRay(dropPoint, Vector3.up, Color.white, 10.0f);
			rb.transform.position = dropPoint;
		}
		finally
		{
			rb.isKinematic = false;
			holdInfo = null;
			transform.SetParent(null);
		}
	}
}
