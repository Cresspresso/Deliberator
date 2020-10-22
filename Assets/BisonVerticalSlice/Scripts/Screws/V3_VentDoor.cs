using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///		A door fixed to a wall by several screws.
/// </summary>
/// 
/// <changelog>
///		<log author="Elijah Shadbolt" date="21/09/2020">
///			<para>Created this script.</para>
///		</log>
///		<log author="Elijah Shadbolt" date="13/10/2020">
///			<para>
///				Made it so that when this vent door is opened,
///				it breaks the screwdriver that the player used to open it.
///			</para>
///		</log>
///		<log author="Elijah Shadbolt" date="13/10/2020">
///			<para>
///				Made it change to a player-no-clip layer when unlocked.
///			</para>
///		</log>
///		<log author="Elijah Shadbolt" date="22/10/2020">
///			<para>
///				Made it also work without screws.
///			</para>
///			<para>
///				Made it play animation of player pushing door.
///			</para>
///		</log>
/// </changelog>
/// 
[RequireComponent(typeof(Dependable))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(V2_PickUpHandle))]
public class V3_VentDoor : MonoBehaviour
{
	public Rigidbody rb { get; private set; }
	public V2_PickUpHandle pickupHandle { get; private set; }
	public Dependable dependable { get; private set; }

#pragma warning disable CS0649
	[SerializeField]
	private bool m_isEasyOpen = true;
	public bool isEasyOpen => m_isEasyOpen;

	[SerializeField]
	private float m_openImpulse = 5.0f;
	public float openImpulse => m_openImpulse;

	[SerializeField]
	private bool m_openForwards = false;
	public bool openForwards => m_openForwards;
#pragma warning restore CS0649

	public bool hasOpened { get; private set; } = false;

	private void Awake()
	{
		rb = GetComponent<Rigidbody>();
		
		pickupHandle = GetComponent<V2_PickUpHandle>();
		pickupHandle.enabled = false;
		pickupHandle.buttonHandle.onClick += OnClick;
		if (!isEasyOpen)
		{
			pickupHandle.buttonHandle.handle.enabled = false;
		}

		dependable = GetComponent<Dependable>();
		dependable.onChanged.AddListener(OnPoweredChanged);
	}

	private void OnClick(V2_ButtonHandle buttonHandle, V2_HandleController handleController)
	{
		if (dependable.hasFirstLiteral)
		{
			dependable.firstLiteral = true;

			if (!hasOpened)
			{
				V4_PlayerAnimator.instance.PushDoor();
			}
		}
	}

	void OnPoweredChanged(bool isPowered)
	{
		Debug.Log(name, this);
		if (hasOpened) return;

		if (isPowered)
		{
			hasOpened = true;
			rb.isKinematic = false;
			var impulse = transform.forward * (openImpulse * (openForwards ? 1 : -1));
			rb.AddForce(impulse, ForceMode.Impulse);
			pickupHandle.enabled = true;
			gameObject.layer = LayerMask.NameToLayer("NoClipHandle");

			if (!isEasyOpen)
			{
				pickupHandle.buttonHandle.handle.enabled = true;
				var item = V2_PickUpController.instance.currentPickedUpHandle;
				if (item)
				{
					var screwdriver = item.GetComponent<V4_Screwdriver>();
					if (screwdriver)
					{
						screwdriver.Expire();
					}
				}
			}
		}
	}
}
