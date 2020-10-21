using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///		<para>A place to put a generator fuse.</para>
///		<para>One bit of a bitset lock to power something.</para>
/// </summary>
/// 
/// <changelog>
///		<log author="Elijah Shadbolt" date="24/08/2020">
///			<para>Added the ability to take fuses out after they are inserted.</para>
///		</log>
/// </changelog>
/// 
[RequireComponent(typeof(V2_ButtonHandle))]
[RequireComponent(typeof(Dependable))]
public class V3_GenFuseSlot : MonoBehaviour
{
	public Dependable dependable { get; private set; }

	public Transform point;
	public V2_PickUpHandle theFuse { get; private set; } = null;
	public bool hasFuse => theFuse;

	public V2_ButtonHandle buttonHandle { get; private set; }
	private V2_PickUpController puc;

	private void Awake()
	{
		dependable = GetComponent<Dependable>();
		buttonHandle = GetComponent<V2_ButtonHandle>();
		puc = FindObjectOfType<V2_PickUpController>();
	}

	private void Start()
	{
		if (!point) point = transform;
		TryInsert(point.GetComponentInChildren<V2_PickUpHandle>());
	}

	private void OnEnable()
	{
		buttonHandle.onClick += OnClick;
		puc.onPickedUp += OnSomethingPickedUp;
		puc.onDropped += OnSomethingDropped;
	}

	private void OnDisable()
	{
		if (buttonHandle) buttonHandle.onClick -= OnClick;
		if (puc)
		{
			puc.onPickedUp -= OnSomethingPickedUp;
			puc.onDropped -= OnSomethingDropped;
		}
	}

	private void OnSomethingDropped(V2_PickUpController puc, V2_PickUpHandle puHandle)
	{
		buttonHandle.enabled = false;
	}

	private const string fuseTag = "GeneratorFuse";

	private void OnSomethingPickedUp(V2_PickUpController puc, V2_PickUpHandle puHandle)
	{
		if (puHandle.CompareTag(fuseTag))
		{
			buttonHandle.enabled = true;
		}
	}

	private void OnClick(V2_ButtonHandle buttonHandle, V2_HandleController handleController)
	{
		var puc = handleController.GetComponent<V2_PickUpController>();
		if (!puc)
		{
			Debug.LogError("pick up controller not found", this);
		}
		else
		{
			var h = puc.currentPickedUpHandle;
			if (h)
			{
				var item = h.GetComponent<V4_RightHandItem>();
				if (item && item.itemType == V4_PlayerAnimator.ItemType.Fuse)
				{
					V4_PlayerAnimator.instance.PerformRightHandAction(() =>
					{
						TryInsert(puc.currentPickedUpHandle);
					});
				}
			}
		}
	}

	private bool TryInsert(V2_PickUpHandle puHandle)
	{
		if (puHandle)
		{
			Debug.Assert(puHandle.CompareTag(fuseTag), "unexpected pickupHandle type", this);
			theFuse = puHandle;
			puHandle.Drop();
			theFuse.rb.isKinematic = true;

			theFuse.transform.SetParent(point);
			theFuse.transform.localPosition = Vector3.zero;
			theFuse.transform.localRotation = Quaternion.identity;

			dependable.firstLiteral = true;

			puHandle.onPickedUp += OnTheFusePickedUp;
			InvokeFuseInserted();
			return true;
		}
		else
		{
			return false;
		}
	}

	private void InvokeFuseInserted()
	{
	}

	private void InvokeFuseExtracted()
	{
	}

	private void OnTheFusePickedUp(V2_PickUpHandle puHandle, V2_PickUpController puc)
	{
		if (theFuse == puHandle)
		{
			theFuse = null;
			dependable.firstLiteral = false;
			InvokeFuseExtracted();
		}
	}

	public void DisableMeddling()
	{
		buttonHandle.handle.enabled = false;
		if (theFuse)
		{
			theFuse.buttonHandle.handle.enabled = false;
		}
	}
}
