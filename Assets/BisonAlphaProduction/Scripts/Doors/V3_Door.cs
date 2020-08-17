using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///		<para>The animation state of a <see cref="V3_Door"/> or a <see cref="V3_DoorOpener"/>.</para>
/// </summary>
/// 
/// <changelog>
///		<log author="Elijah Shadbolt" date="17/08/2020">
///			<para>Added this type.</para>
///		</log>
/// </changelog>
/// 
public enum DoorState
{
	Closed,
	Opening,
	Opened,
	Closing,
}

/// <summary>
///		<para>Door that opens away from the player (inwards or outwards).</para>
///		<para>Must be a child of a <see cref="V3_DoorManager"/>.</para>
///		<para>Set it as an element in the <see cref="V3_DoorManager.m_doors"/> array.</para>
///		<para>Rotates this <see cref="Transform"/> around its Y axis.</para>
///		<para>Requires a <see cref="Rigidbody"/> component with:</para>
///		<list type="bullet">
///			<item><see cref="Rigidbody.isKinematic"/> = <see langword="true"/></item>
///			<item><see cref="Rigidbody.interpolation"/> = <see cref="RigidbodyInterpolation.Interpolate"/></item>
///		</list>
/// </summary>
/// 
/// <changelog>
///		<log author="Elijah Shadbolt" date="17/08/2020">
///			<para>Created this script as a replacement for <see cref="V2_DoorOpener"/> by Elijah and Lorenzo.</para>
///		</log>
/// </changelog>
/// 
[RequireComponent(typeof(Rigidbody))]
public class V3_Door : MonoBehaviour
{
	private Rigidbody m_rb;

	public Rigidbody rb {
		get
		{
			PrepareRigidbody();
			return m_rb;
		}
	}

	private void PrepareRigidbody()
	{
		if (!m_rb)
		{
			m_rb = GetComponent<Rigidbody>();
			if (!m_rb)
			{
				Debug.LogError("Rigidbody is null", this);
			}
		}
	}



	private V3_DoorSounds m_sounds;

	public V3_DoorSounds sounds {
		get
		{
			PrepareSounds();
			return m_sounds;
		}
	}

	private void PrepareSounds()
	{
		if (!m_sounds)
		{
			m_sounds = GetComponentInChildren<V3_DoorSounds>();
			if (!m_sounds)
			{
				Debug.LogError("Sounds is null", this);
			}
		}
	}



	public V3_DoorManager manager { get; set; }



	/// <summary>
	///		<para>The current animation state of this <see cref="V3_Door"/>.</para>
	/// </summary>
	/// 
	/// <changelog>
	///		<log author="Elijah Shadbolt" date="17/08/2020">
	///			<para>Added this property.</para>
	///		</log>
	/// </changelog>
	/// 
	public DoorState state { get; private set; } = DoorState.Closed;

	public bool isOpen => state == DoorState.Opened || state == DoorState.Opening;



	/// <summary>
	///		<para>Which way a door is opening.</para>
	/// </summary>
	/// 
	/// <changelog>
	///		<log author="Elijah Shadbolt" date="17/08/2020">
	///			<para>Added this type.</para>
	///		</log>
	/// </changelog>
	/// 
	public enum Direction
	{
		None, /// Closed
		Clockwise,
		Anticlockwise,
	}



	/// <summary>
	///		<para>Which way this door is opening (clockwise or anticlockwise).</para>
	/// </summary>
	/// 
	/// <changelog>
	///		<log author="Elijah Shadbolt" date="17/08/2020">
	///			<para>Added this property.</para>
	///		</log>
	/// </changelog>
	/// 
	public Direction direction { get; private set; } = Direction.None;



	/// <summary>
	/// Current angle of door
	/// </summary>
	private float m_angle = 0;
	public float angle {
		get => m_angle;
		private set
		{
			m_angle = Mathf.Clamp(value, angleAnticlockwise, angleClockwise);
		}
	}



	[Tooltip("When looking forwards (along its local Z axis),"
		+ " is the hinge on the left side of the door?")]
	[SerializeField]
	private bool isHingeOnLeft = false;

	public enum OpeningMode { AwayFromPlayer, TowardsPlayer, Clockwise, Anticlockwise }
	[SerializeField]
	private OpeningMode openingMode = OpeningMode.AwayFromPlayer;

	[SerializeField]
	float m_angleClockwise = 90;
	public float angleClockwise => m_angleClockwise;

	[SerializeField]
	float m_angleAnticlockwise = -90;
	public float angleAnticlockwise => m_angleAnticlockwise;

	[SerializeField]
	float m_speed = 200;
	public float speed => m_speed;



	[SerializeField]
	float m_lockedAnimDuration = 0.5f;
	public float lockedAnimDuration => m_lockedAnimDuration;

	[SerializeField]
	float m_lockedAnimAngleAmplitude = 0.5f;
	public float lockedAnimAngleAmplitude => m_lockedAnimAngleAmplitude;

	[SerializeField]
	private AnimationCurve m_lockedAnimAngleCurve = new AnimationCurve(
		new Keyframe(0, 0),
		new Keyframe(0.25f, 1),
		new Keyframe(0.5f, -1),
		new Keyframe(0.75f, 1),
		new Keyframe(1, 0));
	public AnimationCurve lockedAnimAngleCurve => m_lockedAnimAngleCurve;

	/// <summary>
	///		<para>How far through the locked animation we are.</para>
	///		<para>
	///			Value in range [0, 1]
	///			where 1 means the end (stopped)
	///			and 0 means the start.
	///		</para>
	/// </summary>
	private float lockedAnimLerpValue { get; set; } = 1.0f;



	private void Awake()
	{
		PrepareRigidbody();
		PrepareSounds();
	}



	private void FixedUpdate()
	{
		if (lockedAnimLerpValue < 1.0f)
		{
			lockedAnimLerpValue += Time.fixedDeltaTime / lockedAnimDuration;
			if (lockedAnimLerpValue >= 1.0f)
			{
				lockedAnimLerpValue = 1.0f;
				angle = 0.0f;
			}
			else
			{
				angle = lockedAnimAngleAmplitude * (isHingeOnLeft ? -1 : 1)
					* lockedAnimAngleCurve.Evaluate(lockedAnimLerpValue);
			}
		}

		switch (state)
		{
			case DoorState.Closed:
			case DoorState.Opened:
			default:
				break;

			case DoorState.Opening:
			case DoorState.Closing:
				{
					float desiredAngle;
					switch (direction)
					{
						case Direction.Anticlockwise:
							desiredAngle = angleAnticlockwise;
							break;
						case Direction.Clockwise:
							desiredAngle = angleClockwise;
							break;
						case Direction.None:
						default:
							desiredAngle = 0.0f;
							break;
					}

					angle = Mathf.MoveTowards(angle, desiredAngle, speed * Time.fixedDeltaTime);

					if (Mathf.Abs(angle - desiredAngle) < 0.001f)
					{
						if (state == DoorState.Opening)
						{
							state = DoorState.Opened;
							V2_Utility.TryElseLog(manager, () => manager.OnOpened(this));
							V2_Utility.TryElseLog(this, () => InvokeOpened());
						}
						else
						{
							state = DoorState.Closed;
							V2_Utility.TryElseLog(manager, () => manager.OnClosed(this));
							V2_Utility.TryElseLog(this, () => InvokeClosed());
						}
					}
				}
				break;
		}

		Transform parent = rb.transform.parent;
		Quaternion prot = parent ? parent.rotation : Quaternion.identity;
		var quat = prot * Quaternion.Euler(0, angle, 0);
		rb.MoveRotation(quat);
	}



	/// <summary>
	///		<para>Called by <see cref="V3_DoorManager"/>.</para>
	/// </summary>
	public void TryToClose()
	{
		if (isOpen)
		{
			StopLockedAnim();
			state = DoorState.Closing;
			direction = Direction.None;
			V2_Utility.TryElseLog(manager, () => manager.OnClosing(this));
			V2_Utility.TryElseLog(this, () => InvokeClosing());
		}
	}



	/// <summary>
	///		<para>Called by <see cref="V3_DoorManager"/>.</para>
	/// </summary>
	public void TryToOpen(Vector3 fpccHeadForward) => TryToOpen(GetDesiredDirection(fpccHeadForward));



	/// <summary>
	///		<para>Called by <see cref="V3_DoorManager"/>.</para>
	/// </summary>
	public void TryToOpen(Direction direction)
	{
		if (!isOpen)
		{
			StopLockedAnim();
			state = DoorState.Opening;
			this.direction = direction;
			V2_Utility.TryElseLog(manager, () => manager.OnOpening(this));
			V2_Utility.TryElseLog(this, () => InvokeOpening());
		}
	}



	private Direction GetDirectionAwayFromPlayer(Vector3 fpccHeadForward)
	{
		if (Vector3.Dot(fpccHeadForward, transform.forward) < 0)
		{
			return isHingeOnLeft ? Direction.Clockwise : Direction.Anticlockwise;
		}
		else
		{
			return isHingeOnLeft ? Direction.Anticlockwise : Direction.Clockwise;
		}
	}



	public Direction GetDesiredDirection(Vector3 fpccHeadForward)
	{
		switch (openingMode)
		{
			case OpeningMode.AwayFromPlayer:
			default:
				return GetDirectionAwayFromPlayer(fpccHeadForward);

			case OpeningMode.TowardsPlayer:
				return GetDirectionAwayFromPlayer(-fpccHeadForward);

			case OpeningMode.Clockwise:
				return Direction.Clockwise;

			case OpeningMode.Anticlockwise:
				return Direction.Anticlockwise;
		}
	}



	private void InvokeOpening()
	{
		sounds.PlaySound(sounds.openingSound);
	}



	private void InvokeClosing()
	{
		sounds.PlaySound(sounds.closingSound);
	}



	private void InvokeOpened()
	{
	}



	private void InvokeClosed()
	{
	}



	/// <summary>
	///		<para>Called by <see cref="V3_DoorManager"/>.</para>
	/// </summary>
	public void PlayLockedAnim()
	{
		lockedAnimLerpValue = 0.0f;
		sounds.PlaySound(sounds.lockedSound);
	}



	public void StopLockedAnim()
	{
		if (lockedAnimLerpValue < 1.0f)
		{
			lockedAnimLerpValue = 1.0f;
			angle = 0.0f;
		}
	}
}
