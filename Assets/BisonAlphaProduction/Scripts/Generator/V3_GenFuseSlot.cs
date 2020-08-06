using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(V2_ButtonHandle))]
public class V3_GenFuseSlot : MonoBehaviour
{
	public Transform point;
	public V2_PickUpHandle theFuse { get; private set; } = null;
	public bool hasFuse => hasFuse;

	public V2_ButtonHandle buttonHandle { get; private set; }
	private V2_PickUpController puc;

	private void Awake()
	{
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
		TryInsert(puc.currentPickedUpHandle);
	}

	private bool TryInsert(V2_PickUpHandle puh)
	{
		if (puh && puh.isActiveAndEnabled && puh.CompareTag(fuseTag))
		{
			theFuse = puc.currentPickedUpHandle;
			puh.Drop();
			theFuse.buttonHandle.handle.enabled = false;
			theFuse.rb.isKinematic = true;

			theFuse.transform.SetParent(point);
			theFuse.transform.localPosition = Vector3.zero;
			theFuse.transform.localRotation = Quaternion.identity;

			buttonHandle.handle.enabled = false;

			OnFuseInserted();
			return true;
		}
		else
		{
			return false;
		}
	}

	private void OnFuseInserted()
	{
		GetComponent<Dependable>().firstLiteral = true;
	}
}
