using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///		<para>Manages what the player can interact with.</para>
///		<para>Determines what interactable object the player's crosshair is hovering over.</para>
///		<para>
///			An object that is interactable is called a handle,
///			and must have a <see cref="V2_Handle"/> script on its root GameObject.
///		</para>
/// </summary>
/// 
/// <changelog>
///		<log author="Elijah Shadbolt" date="01/10/2020">
///			<para>Added more detailed comments.</para>
///		</log>
/// </changelog>
/// 
public class V2_HandleController : MonoBehaviour
{
	/// <summary>
	///		<para>What layers are the handles on?</para>
	///		<para>The default value (~0) bitwise NOT of 0 means "every layer is selected".</para>
	/// </summary>
	/// 
	/// <changelog>
	///		<log author="Elijah Shadbolt" date="01/10/2020">
	///			<para>Added more detailed comments.</para>
	///		</log>
	/// </changelog>
	/// 
	public LayerMask handleMask = ~0;

	/// <summary>
	///		<para>What layers are the handles' extra colliders on?</para>
	///		<para>Such colliders are used to detect if the player is vaguely looking somewhere close to the handle.</para>
	/// </summary>
	/// 
	/// <changelog>
	///		<log author="Elijah Shadbolt" date="01/10/2020">
	///			<para>Added comments.</para>
	///		</log>
	/// </changelog>
	/// 
	public LayerMask handleSphereMask = ~0;

	/// <summary>
	///		<para>How far can the player reach to hover over and interact with handles?</para>
	/// </summary>
	/// 
	/// <changelog>
	///		<log author="Elijah Shadbolt" date="01/10/2020">
	///			<para>Added comments.</para>
	///		</log>
	/// </changelog>
	/// 
	public float maxDistance = 5;

	/// <summary>
	///		<para>The current handle that is being hovered over by the player.</para>
	///		<para>Can be null.</para>
	/// </summary>
	/// 
	/// <changelog>
	///		<log author="Elijah Shadbolt" date="01/10/2020">
	///			<para>Added comments.</para>
	///		</log>
	/// </changelog>
	/// 
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
	private V2_Handle m_hoveredHandle;

	/// <summary>
	///		<para>The current direction in world space that the player's crosshair is looking.</para>
	/// </summary>
	/// 
	/// <changelog>
	///		<log author="Elijah Shadbolt" date="01/10/2020">
	///			<para>Added comments.</para>
	///		</log>
	/// </changelog>
	/// 
	public Vector3 currentRayDirection => transform.forward;

	/// <summary>
	///		<para>A point in world space that the player's crosshair is hovering over.</para>
	///		<para>If the player is not looking at a handle, this is set to {transform.position}.</para>
	///		<para>This property is updated in the <see cref="Update"/> method.</para>
	/// </summary>
	/// 
	/// <changelog>
	///		<log author="Elijah Shadbolt" date="01/10/2020">
	///			<para>Added comments.</para>
	///		</log>
	/// </changelog>
	/// 
	public Vector3 currentHitPoint { get; private set; }

	/// <summary>
	///		<para>An event that is fired when a <see cref="V2_Handle"/> starts being hovered over by this script.</para>
	///		<para>The listener callback must take two parameters:</para>
	///		<para><see cref="V2_HandleController"/>: This script instance.</para>
	///		<para><see cref="V2_Handle"/>: The handle that this controller started hovering over.</para>
	/// </summary>
	/// 
	/// <changelog>
	///		<log author="Elijah Shadbolt" date="01/10/2020">
	///			<para>Added comments.</para>
	///		</log>
	/// </changelog>
	/// 
	public event Action<V2_HandleController, V2_Handle> onHoverEnter;

	/// <summary>
	///		<para>An event that is fired when a <see cref="V2_Handle"/> is no longer being hovered over by this script.</para>
	///		<para>The listener callback must take two parameters:</para>
	///		<para><see cref="V2_HandleController"/>: This script instance.</para>
	///		<para><see cref="V2_Handle"/>: The handle that this controller stopped hovering over.</para>
	/// </summary>
	/// 
	/// <changelog>
	///		<log author="Elijah Shadbolt" date="01/10/2020">
	///			<para>Added comments.</para>
	///		</log>
	/// </changelog>
	/// 
	public event Action<V2_HandleController, V2_Handle> onHoverExit;

	/// <summary>
	///		<para>Unity Message Method: <a href="https://docs.unity3d.com/ScriptReference/MonoBehaviour.OnDisable.html"/></para>
	/// </summary>
	/// 
	/// <changelog>
	///		<log author="Elijah Shadbolt" date="01/10/2020">
	///			<para>Added comments.</para>
	///		</log>
	/// </changelog>
	/// 
	private void OnDisable()
	{
		/// If this controller was disabled, but its handle was not,
		/// ensure that the handle knows that this was disabled,
		/// and therefore it is no longer being hovered.

		hoveredHandle = null;
	}

	/// <summary>
	///		<para>
	///			Called by <see cref="V2_Handle.OnDisable"/>
	///			when the currently hovered handle is disabled
	///			by something outside of our control.
	///		</para>
	/// </summary>
	/// 
	/// <changelog>
	///		<log author="Elijah Shadbolt" date="01/10/2020">
	///			<para>Added comments.</para>
	///		</log>
	/// </changelog>
	/// 
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

	/// <summary>
	///		<para>
	///			Detects if the player is now hovering over a different handle.
	///			If so, this method sends events about the facts that
	///			the old handle is no longer being hovered,
	///			and the new handle is now being hovered.
	///		</para>
	/// </summary>
	/// 
	/// <changelog>
	///		<log author="Elijah Shadbolt" date="01/10/2020">
	///			<para>Added comments.</para>
	///		</log>
	/// </changelog>
	/// 
	private void RaycastForHandle()
	{
		RaycastHit hit;

		var ray = new Ray(transform.position, transform.forward);

		/// If the player is looking directly at a handle...
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
				/// Set properties. Check for changes and if so send events.
				hoveredHandle = otherHandle;
				currentHitPoint = hit.point;
				return;
			}
		}

		/// If the player is looking in the vague direction of a handle...
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
				/// Now raycast directly to the centre of the handle.
				
				Vector3 handlePosition = otherHandle.handlePoint
					? otherHandle.handlePoint.position
					: otherHandle.transform.position;

				var ray2 = new Ray(
					transform.position,
					Vector3.ClampMagnitude(handlePosition - transform.position, maxDistance)
					);

				/// If (^ above condition) and 
				///  the player could theoretically look directly at the centre of the handle
				///  with no obstacles in the way...
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
						/// Set properties. Check for changes and if so send events.
						hoveredHandle = otherHandle;
						currentHitPoint = hit.point;
						return;
					}
				}
			}
		}

		/// The player is not looking at any handle, or it is too far away.
		/// Set properties. Check for changes and if so send events.
		currentHitPoint = transform.position;
		hoveredHandle = null;
	}

	/// <summary>
	///		<para>Unity Message Method: <a href="https://docs.unity3d.com/ScriptReference/MonoBehaviour.Update.html"/></para>
	/// </summary>
	/// 
	/// <changelog>
	///		<log author="Elijah Shadbolt" date="01/10/2020">
	///			<para>Added comments.</para>
	///		</log>
	/// </changelog>
	/// 
	private void Update()
	{
		RaycastForHandle();
	}

	/// <summary>
	///		<para>Unity Message Method: <a href="https://docs.unity3d.com/ScriptReference/MonoBehaviour.OnDrawGizmosSelected.html"/></para>
	///		<para>Shows geometry in the Scene view of the Unity Editor.</para>
	/// </summary>
	/// 
	/// <changelog>
	///		<log author="Elijah Shadbolt" date="01/10/2020">
	///			<para>Added comments.</para>
	///		</log>
	/// </changelog>
	/// 
	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawRay(transform.position, transform.forward * maxDistance);
	}
}
