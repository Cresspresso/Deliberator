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
		LockedLabel,
	}
	public Mode mode = Mode.Toggle;

	private void Apply(V3_DoorManager door, Vector3 fpccHeadForward)
	{
		switch (mode)
		{
			case Mode.Toggle:
				door.TryToToggle(fpccHeadForward);
				break;

			case Mode.Open:
				door.TryToOpen(fpccHeadForward);
				break;

			case Mode.Close:
				door.TryToClose();
				break;

			case Mode.LockedLabel:
				{
					if (door.isLocked)
					{
						door.PlayLockedAnim();
					}
					/// else do nothing
				}
				break;

			default:
				throw new Exception("invalid enum value");
		}
	}



#pragma warning disable CS0649
	[SerializeField]
	private V3_DoorManager m_door;
#pragma warning restore CS0649
	public V3_DoorManager door => m_door;



#pragma warning disable CS0649
	[SerializeField]
	private V2_HandleHoverInfo m_lockedInfo = new V2_HandleHoverInfo("Locked");
#pragma warning restore CS0649
	public V2_HandleHoverInfo lockedInfo => m_lockedInfo;

#pragma warning disable CS0649
	[SerializeField]
	private V2_HandleHoverInfo m_openInfo = new V2_HandleHoverInfo("Open");
#pragma warning restore CS0649
	public V2_HandleHoverInfo openInfo => m_openInfo;

#pragma warning disable CS0649
	[SerializeField]
	private V2_HandleHoverInfo m_closeInfo = new V2_HandleHoverInfo("Close");
#pragma warning restore CS0649
	public V2_HandleHoverInfo closeInfo => m_closeInfo;



	private void Update()
	{
		var handle = buttonHandle.handle;
		if (door.isLocked)
		{
			handle.enabled = true;
			handle.hoverInfo = lockedInfo;
		}
		else
		{
			switch (mode)
			{
				default:
				case Mode.Toggle:
					{
						handle.enabled = true;
						handle.hoverInfo = door.isOpen ? closeInfo : openInfo;
					}
					break;

				case Mode.Open:
					{
						if (door.isOpen)
						{
							handle.enabled = false;
						}
						else
						{
							handle.enabled = true;
							handle.hoverInfo = openInfo;
						}
					}
					break;

				case Mode.Close:
					{
						if (door.isOpen)
						{
							handle.enabled = true;
							handle.hoverInfo = closeInfo;
						}
						else
						{
							handle.enabled = false;
						}
					}
					break;

				case Mode.LockedLabel:
					{
						handle.enabled = false;
					}
					break;
			}
		}
	}



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
		Apply(door, fpcc.head.forward);
	}

	private void Awake()
	{
		PrepareButtonHandle();

		if (!door)
		{
			Debug.LogError("Door is null", this);
		}
	}
}
