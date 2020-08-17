using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///		<para>A button for opening/closing/toggling an unlocked door manager.</para>
///		<para>See also:</para>
///		<para><see cref="V3_DoorManager"/></para>
///		<para><see cref="V3_Door"/></para>
///		<para><see cref="V3_DoorSounds"/></para>
/// </summary>
/// 
/// <changelog>
///		<log author="Elijah Shadbolt" date="17/08/2020">
///			<para>Added this script.</para>
///		</log>
/// </changelog>
/// 
[RequireComponent(typeof(V2_ButtonHandle))]
public class V3_DoorOpener : MonoBehaviour
{
	public enum Mode
	{
		Toggle,
		Open,
		Close,
	}
	public Mode mode = Mode.Toggle;

	private Action<V3_DoorManager> GetAction(Vector3 fpccHeadForward)
	{
		switch (mode)
		{
			case Mode.Toggle: return door => door.TryToToggle(fpccHeadForward);
			case Mode.Open: return door => door.TryToOpen(fpccHeadForward);
			case Mode.Close: return door => door.TryToClose();
			default: throw new Exception("invalid enum value");
		}
	}



	[SerializeField]
	private V3_DoorManager[] m_doors = new V3_DoorManager[1];



	private V2_ButtonHandle m_buttonHandle;

	public V2_ButtonHandle buttonHandle {
		get
		{
			PrepareButtonHandle();
			return m_buttonHandle;
		}
	}

	private void PrepareButtonHandle()
	{
		if (!m_buttonHandle)
		{
			m_buttonHandle = GetComponent<V2_ButtonHandle>();
			if (!m_buttonHandle)
			{
				Debug.LogError("ButtonHandle is null", this);
			}
			else
			{
				m_buttonHandle.onClick -= OnClick;
				m_buttonHandle.onClick += OnClick;
			}
		}
	}

	private void OnClick(V2_ButtonHandle buttonHandle, V2_HandleController handleController)
	{
		var fpcc = handleController.GetComponentInParent<V2_FirstPersonCharacterController>();
		var action = GetAction(fpcc.head.forward);
		foreach (var door in m_doors) action(door);
	}

	private void Awake()
	{
		PrepareButtonHandle();
	}
}
