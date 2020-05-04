using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

/// <author>Elijah Shadbolt</author>
public class HandleController : MonoBehaviour
{
	public LayerMask handleMask = ~0;
	public float maxDistance = 5;

	private Handle m_hoveredHandle;
	public Handle hoveredHandle {
		get => m_hoveredHandle;
		set
		{
			if (m_hoveredHandle == value) { return; }

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

	public event Action<HandleController, Handle> onHoverEnter;
	public event Action<HandleController, Handle> onHoverExit;

	private void RaycastForHandle()
	{
		if (Physics.Raycast(
			origin: transform.position,
			direction: transform.forward,
			out var hit,
			maxDistance: maxDistance,
			layerMask: handleMask,
			QueryTriggerInteraction.Collide))
		{
			var otherHandle = hit.collider.GetComponentInParent<Handle>();
			if (otherHandle)
			{
				hoveredHandle = otherHandle;
				return;
			}
		}
		hoveredHandle = null;
	}

	private void Update()
	{
		RaycastForHandle();
	}

	private void OnDisable()
	{
		hoveredHandle = null;
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawRay(transform.position, transform.forward * maxDistance);
	}
}
