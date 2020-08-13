using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
///		<para>A safe which the player can open by entering in the correct combination of integers.</para>
///		<para>
///			Requires an <see cref="Animator"/> which starts disabled in the inspector.
///			It will be enabled and start playing when the safe is opened.
///		</para>
///		<para>See also:</para>
///		<para><see cref="V3_VaultSafeHud"/></para>
/// </summary>
/// 
/// <changelog>
///		<log author="Elijah Shadbolt" date="13/08/2020">
///			<para>Added comments.</para>
///		</log>
/// </changelog>
/// 
[RequireComponent(typeof(V2_ButtonHandle))]
public class V3_VaultSafe : MonoBehaviour
{
	public V2_ButtonHandle buttonHandle { get; private set; }
	public V3_VaultSafeHud hud { get; private set; }

	public Animator anim;

	public const int maxValue = 99;

	[SerializeField]
	public int[] combination = new int[3] { 13, 52, 1 };

	public int[] currentValues { get; private set; } // returns reference to owned array

	public bool isOpened { get; private set; } = false;



	private void Awake()
	{
		buttonHandle = GetComponent<V2_ButtonHandle>();

		buttonHandle.onClick += OnClick;

		hud = FindObjectOfType<V3_VaultSafeHud>();

		currentValues = new int[combination.Length];
	}

	private void OnDestroy()
	{
		if (buttonHandle)
		{
			buttonHandle.onClick -= OnClick;
		}
	}

	private void OnClick(V2_ButtonHandle buttonHandle, V2_HandleController handleController)
	{
		if (isOpened) return;

		var puc = handleController.GetComponent<V2_PickUpController>();
		if (puc && puc.currentPickedUpHandle)
		{
			puc.currentPickedUpHandle.Drop();
		}

		hud.Show(this);
	}

	public void OnLeaveHud()
	{
		if (!isOpened)
		{
			buttonHandle.handle.enabled = true;
		}
	}

	public void Open()
	{
		isOpened = true;
		buttonHandle.handle.enabled = false;
		anim.enabled = true;
	}
}
