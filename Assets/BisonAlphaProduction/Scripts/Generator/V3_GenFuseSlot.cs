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
		if (puc.currentPickedUpHandle && puc.currentPickedUpHandle.isActiveAndEnabled
			&& puc.currentPickedUpHandle.CompareTag(fuseTag))
		{
			theFuse = puc.currentPickedUpHandle;
			puc.currentPickedUpHandle.Drop();
			theFuse.buttonHandle.handle.enabled = false;
			theFuse.rb.isKinematic = true;

			if (!point) point = transform;
			theFuse.transform.SetParent(point);
			theFuse.transform.localPosition = Vector3.zero;
			theFuse.transform.localRotation = Quaternion.identity;

			buttonHandle.handle.enabled = false;

			OnFuseInserted();
		}
	}

	private void OnFuseInserted()
	{

	}
}
