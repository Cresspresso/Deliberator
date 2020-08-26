using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
///		<para>A set of doors which can be locked or unlocked by the <see cref="Dependable"/>.</para>
///		<para>Could be a single door or double doors. There is an unlimited number of doors.</para>
///		<para>See also:</para>
///		<para><see cref="V3_Door"/></para>
/// </summary>
/// 
/// <changelog>
///		<log author="Elijah Shadbolt" date="17/08/2020">
///			<para>Created this script as a replacement for <see cref="V2_DoorOpener"/> by Elijah and Lorenzo.</para>
///		</log>
/// </changelog>
/// 
[RequireComponent(typeof(Dependable))]
public class V3_DoorManager : MonoBehaviour
{
	private Dependable m_dependable;

	/// <summary>
	///		<para>Determines whether this door is Locked or not.</para>
	/// </summary>
	public Dependable dependable {
		get
		{
			PrepareDependable();
			return m_dependable;
		}
	}

	private void PrepareDependable()
	{
		if (!m_dependable)
		{
			m_dependable = GetComponent<Dependable>();
			if (!m_dependable)
			{
				Debug.LogError("ButtonHandle is null", this);
			}
			else
			{
				m_dependable.onChanged.AddListener(OnDependablePoweredChanged);
			}
		}
	}

#pragma warning disable CS0649
	[SerializeField]
	private bool m_openOnUnlocked = true;
#pragma warning restore CS0649
	public bool openOnUnlocked {
		get => m_openOnUnlocked;
		set => m_openOnUnlocked = value; /// should be set in Awake
	}



	private void OnDependablePoweredChanged(bool isUnlocked)
	{
		isLocked = !isUnlocked;
		if (isLocked)
		{
			V2_Utility.TryElseLog(this, InvokeLocked);
		}
		else
		{
			V2_Utility.TryElseLog(this, InvokeUnlocked);
		}
	}



	[Tooltip("Children of this DoorOpener.")]
	[SerializeField]
	private V3_Door[] m_doors = new V3_Door[1];
	public IEnumerable<V3_Door> doors => m_doors;

	[SerializeField]
	private float m_initialSoundDelay = 0.05f;



	public void OnOpening(V3_Door door)
	{

	}



	public void OnClosing(V3_Door door)
	{

	}



	public void OnOpened(V3_Door door)
	{
		if (m_doors.All(d => d.state == DoorState.Opened))
		{
			state = DoorState.Opened;
			V2_Utility.TryElseLog(this, InvokeOpened);
		}
	}



	public void OnClosed(V3_Door door)
	{
		if (m_doors.All(d => d.state == DoorState.Closed))
		{
			state = DoorState.Closed;
			V2_Utility.TryElseLog(this, InvokeClosed);
		}
	}



	private void Awake()
	{
		PrepareDependable();

		for (int i = 0; i < m_doors.Length; ++i)
		{
			var door = m_doors[i];

			if (door.manager)
			{
				Debug.LogError($"'{name}': Door '{door.name}' already has a manager '{door.manager.name}'", this);
			}
			door.manager = this;

			door.sounds.soundDelay = m_initialSoundDelay * i;
		}
	}



	public DoorState state { get; private set; } = DoorState.Closed;
	public bool isOpen => state == DoorState.Opened || state == DoorState.Opening;



	public bool isLocked { get; private set; }



	private void InvokeOpened()
	{

	}



	private void InvokeClosed()
	{

	}



	private void InvokeOpening()
	{

	}



	private void InvokeClosing()
	{

	}



	private void InvokeLocked()
	{
		TryToClose();
	}



	private void InvokeUnlocked()
	{
		if (openOnUnlocked)
		{
			TryToOpen(FindObjectOfType<V2_FirstPersonCharacterController>());
		}
	}



	public void PlayLockedAnim()
	{
		foreach (var door in m_doors)
		{
			door.PlayLockedAnim();
		}
	}



	public void TryToOpen(V2_FirstPersonCharacterController fpcc) => TryToOpen(fpcc.head.forward);



	public void TryToOpen(Vector3 fpccHeadForward)
	{
		if (isLocked)
		{
			PlayLockedAnim();
			return;
		}

		if (!isOpen)
		{
			state = DoorState.Opening;
			foreach (var door in m_doors)
			{
				door.TryToOpen(fpccHeadForward);
			}
			V2_Utility.TryElseLog(this, InvokeOpening);
		}
	}



	public void TryToClose()
	{
		if (isOpen)
		{
			state = DoorState.Closing;
			foreach (var door in m_doors)
			{
				door.TryToClose();
			}
			V2_Utility.TryElseLog(this, InvokeClosing);
		}
	}



	public void TryToToggle(Vector3 fpccHeadForward)
	{
		if (isOpen)
		{
			TryToClose();
		}
		else
		{
			TryToOpen(fpccHeadForward);
		}
	}
}
