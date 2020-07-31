using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

/// <author>Elijah Shadbolt</author>
public class V2_HandleController : MonoBehaviour
{
	public LayerMask handleMask = ~0; //Bitwise NOT of 0 means every layer is selected 
	public LayerMask handleSphereMask = ~0;
	public float maxDistance = 5;

	private V2_Handle m_hoveredHandle;
	public V2_Handle hoveredHandle {
		get => m_hoveredHandle;
		private set
		{
			if (value == m_hoveredHandle) { return; }

			if (m_hoveredHandle)
			{
				// unsubscribe
				m_hoveredHandle.HandleControllerHoverExit(this);
				try
				{
					onHoverExit?.Invoke(this, m_hoveredHandle);
				}
				catch (Exception e)
				{
					Debug.LogException(e);
				}
			}

			m_hoveredHandle = value;

			if (m_hoveredHandle)
			{
				// subscribe
				m_hoveredHandle.HandleControllerHoverEnter(this);
				try
				{
					onHoverEnter?.Invoke(this, m_hoveredHandle);
				}
				catch (Exception e)
				{
					Debug.LogException(e);
				}
			}
		}
	}

	public event Action<V2_HandleController, V2_Handle> onHoverEnter;
	public event Action<V2_HandleController, V2_Handle> onHoverExit;

	private void OnDisable()
	{
		hoveredHandle = null;
	}

	public void InternalOnHandleDisabled(V2_Handle handle)
	{
		if (m_hoveredHandle == handle)
		{
			// unsubscribe
			if (m_hoveredHandle)
			{
				m_hoveredHandle.HandleControllerHoverExit(this);
				try
				{
					onHoverExit?.Invoke(this, m_hoveredHandle);
				}
				catch (Exception e)
				{
					Debug.LogException(e);
				}
			}

			m_hoveredHandle = null;
		}
	}

	private void RaycastForHandle()
	{
		RaycastHit hit;

		var ray = new Ray(transform.position, transform.forward);
		if (Physics.Raycast(
			ray,
			out hit,
			maxDistance,
			handleMask,
			QueryTriggerInteraction.Collide))
		{
			var otherHandle = hit.collider.GetComponentInParent<V2_Handle>();
			if (otherHandle && otherHandle.enabled)
			{
				hoveredHandle = otherHandle;
				return;
			}
		}
		if (Physics.Raycast(
			ray,
			out hit,
			maxDistance,
			handleSphereMask,
			QueryTriggerInteraction.Collide))
		{
			var otherHandle = hit.collider.GetComponentInParent<V2_Handle>();
			if (otherHandle && otherHandle.enabled)
			{
				Vector3 handlePosition = otherHandle.handlePoint
					? otherHandle.handlePoint.position
					: otherHandle.transform.position;

				var ray2 = new Ray(
					transform.position,
					Vector3.ClampMagnitude(handlePosition - transform.position, maxDistance)
					);

				if (Physics.Raycast(
					ray2,
					out hit,
					maxDistance,
					handleMask,
					QueryTriggerInteraction.Collide))
				{
					var secondHandle = hit.collider.GetComponentInParent<V2_Handle>();
					if (secondHandle == otherHandle)
					{
						hoveredHandle = otherHandle;
						return;
					}
				}
			}
		}
		hoveredHandle = null;
	}

	private void Update()
	{
		RaycastForHandle();
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawRay(transform.position, transform.forward * maxDistance);
	}
}
