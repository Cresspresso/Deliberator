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
/// </changelog>
/// 
[RequireComponent(typeof(Dependable))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(V2_Handle))]
public class V3_VentDoor : MonoBehaviour
{
	public Rigidbody rb { get; private set; }
	public V2_Handle handle { get; private set; }
	public Dependable dependable { get; private set; }

	public bool hasOpened { get; private set; } = false;

	private void Awake()
	{
		rb = GetComponent<Rigidbody>();

		handle = GetComponent<V2_Handle>();
		handle.enabled = false;

		dependable = GetComponent<Dependable>();
		dependable.onChanged.AddListener(OnPoweredChanged);
	}

	void OnPoweredChanged(bool isPowered)
	{
		if (hasOpened) return;

		if (isPowered)
		{
			hasOpened = true;
			rb.isKinematic = false;
			handle.enabled = true;

			var pickupHandle = V2_PickUpController.instance.currentPickedUpHandle;
			if (pickupHandle)
			{
				var screwdriver = pickupHandle.GetComponent<V4_Screwdriver>();
				if (screwdriver)
				{
					screwdriver.Expire();
				}
			}
		}
	}
}
