using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///		<para>Visual details to display when a <see cref="V2_Handle"/> object is hovered over with the crosshair.</para>
/// </summary>
/// 
/// <changelog>
///		<log author="Elijah Shadbolt" date="01/10/2020">
///			<para>Added comments.</para>
///		</log>
/// </changelog>
/// 
[System.Serializable]
public struct V2_HandleHoverInfo
{
	/// <summary>
	/// Hover text.
	/// </summary>
	public string description;

	/// <summary>
	/// Crosshair sprite.
	/// </summary>
	public Sprite sprite;

	public V2_HandleHoverInfo(string description, Sprite sprite = null)
	{
		this.description = description;
		this.sprite = sprite;
	}
}

/// <summary>
///		<para>A GameObject that can be hovered over with the crosshair.</para>
///		<para>Represents an interactable object.</para>
/// </summary>
/// 
/// <changelog>
///		<log author="Elijah Shadbolt" date="01/10/2020">
///			<para>Added more detailed comments.</para>
///		</log>
/// </changelog>
/// 
public class V2_Handle : MonoBehaviour
{
	/// <summary>
	///		<para>The current <see cref="V2_HandleController"/> that is hovering over this object.</para>
	///		<para>Can be null.</para>
	/// </summary>
	/// 
	/// <changelog>
	///		<log author="Elijah Shadbolt" date="01/10/2020">
	///			<para>Added comments.</para>
	///		</log>
	/// </changelog>
	/// 
	public V2_HandleController controller { get; private set; }

	/// <summary>
	///		<para>True if this object is being hovered over by the player crosshair.</para>
	/// </summary>
	/// 
	/// <changelog>
	///		<log author="Elijah Shadbolt" date="01/10/2020">
	///			<para>Added comments.</para>
	///		</log>
	/// </changelog>
	/// 
	public bool isHovered { get; private set; }

	/// <summary>
	///		<para>An event that is fired when a <see cref="V2_HandleController"/> starts hovering over this object.</para>
	///		<para>The listener callback must take two parameters:</para>
	///		<para><see cref="V2_Handle"/>: This script instance.</para>
	///		<para><see cref="V2_HandleController"/>: The handle controller that started hovering this object.</para>
	/// </summary>
	/// 
	/// <changelog>
	///		<log author="Elijah Shadbolt" date="01/10/2020">
	///			<para>Added comments.</para>
	///		</log>
	/// </changelog>
	/// 
	public event Action<V2_Handle, V2_HandleController> onHoverEnter;

	/// <summary>
	///		<para>An event that is fired when a <see cref="V2_HandleController"/> stops hovering over this object.</para>
	///		<para>The listener callback must take two parameters:</para>
	///		<para><see cref="V2_Handle"/>: This script instance.</para>
	///		<para><see cref="V2_HandleController"/>: The handle controller that stopped hovering this object.</para>
	/// </summary>
	/// 
	/// <changelog>
	///		<log author="Elijah Shadbolt" date="01/10/2020">
	///			<para>Added comments.</para>
	///		</log>
	/// </changelog>
	/// 
	public event Action<V2_Handle, V2_HandleController> onHoverExit;

	/// <summary>
	///		<para>An event that is fired when the value of <see cref="hoverInfo"/> is changed.</para>
	///		<para>The listener callback must take two parameters:</para>
	///		<para><see cref="V2_Handle"/>: This script instance.</para>
	///		<para><see cref="V2_HandleHoverInfo"/>: The new value of <see cref="hoverInfo"/>.</para>
	/// </summary>
	/// 
	/// <changelog>
	///		<log author="Elijah Shadbolt" date="01/10/2020">
	///			<para>Added comments.</para>
	///		</log>
	/// </changelog>
	/// 
	public event Action<V2_Handle, V2_HandleHoverInfo> onHoverInfoChanged;

	/// <summary>
	///		<para>A point in world space represented by a <see cref="Transform"/>, usually a child of this GameObject.</para>
	///		<para>The point represents where the hover text should be displayed.</para>
	///		<para>Must not be null.</para>
	/// </summary>
	/// 
	/// <changelog>
	///		<log author="Elijah Shadbolt">
	///			<para>Added comments to this property in Alpha.</para>
	///		</log>
	///		<log author="Elijah Shadbolt" date="01/10/2020">
	///			<para>Added more detailed comments to this property.</para>
	///		</log>
	/// </changelog>
	/// 
	public Transform handlePoint => m_handlePoint;
#pragma warning disable CS0649
	[SerializeField]
	private Transform m_handlePoint;
#pragma warning restore CS0649



	/// <summary>
	///		<para>Info about what to display when this object is hovered over by a <see cref="V2_HandleController"/>.</para>
	/// </summary>
	/// 
	/// <changelog>
	///		<log author="Elijah Shadbolt" date="01/10/2020">
	///			<para>Added comments.</para>
	///		</log>
	/// </changelog>
	/// 
	public V2_HandleHoverInfo hoverInfo {
		get => m_hoverInfo;
		set
		{
			m_hoverInfo = value;
			try
			{
				onHoverInfoChanged?.Invoke(this, m_hoverInfo);
			}
			catch (Exception e)
			{
				Debug.LogError(e, this);
			}
		}
	}
#pragma warning disable CS0649
	[SerializeField]
	private V2_HandleHoverInfo m_hoverInfo = new V2_HandleHoverInfo("Touch", null);
#pragma warning restore CS0649

	/// <summary>
	///		<para>Internal method called by <see cref="V2_HandleController"/> when it starts hovering this object.</para>
	/// </summary>
	/// 
	/// <changelog>
	///		<log author="Elijah Shadbolt" date="01/10/2020">
	///			<para>Added comments.</para>
	///		</log>
	/// </changelog>
	/// 
	public void HandleControllerHoverEnter(V2_HandleController handleController)
	{
		controller = handleController;
		isHovered = true;
		try
		{
			onHoverEnter?.Invoke(this, handleController);
		}
		catch (Exception e)
		{
			Debug.LogError(e, this);
		}
	}

	/// <summary>
	///		<para>Internal method called by <see cref="V2_HandleController"/> when it stops hovering this object.</para>
	/// </summary>
	/// 
	/// <changelog>
	///		<log author="Elijah Shadbolt" date="01/10/2020">
	///			<para>Added comments.</para>
	///		</log>
	/// </changelog>
	/// 
	public void HandleControllerHoverExit(V2_HandleController handleController)
	{
		controller = handleController;
		isHovered = false;
		try
		{
			onHoverExit?.Invoke(this, handleController);
		}
		catch (Exception e)
		{
			Debug.LogError(e, this);
		}
		controller = null;
	}

	/// <summary>
	///		<para>Unity Message Method: <a href="https://docs.unity3d.com/ScriptReference/MonoBehaviour.Start.html"/></para>
	/// </summary>
	/// 
	/// <changelog>
	///		<log author="Elijah Shadbolt" date="01/10/2020">
	///			<para>Added more detailed comments.</para>
	///		</log>
	/// </changelog>
	/// 
	private void Start()
	{
		/// This method is meant to be empty.
		/// Its purpose is to show the "enabled" checkbox in the inspector.
	}

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
		/// If this handle is deactivated while it was being hovered,
		/// ensure the handle controller knows that it was disabled, and should stop hovering it.

		isHovered = false;
		if (controller)
		{
			controller.InternalOnHandleDisabled(this);
			controller = null;
		}
	}
}
