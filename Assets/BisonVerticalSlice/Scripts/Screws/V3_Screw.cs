using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///		Something that can be released by a screwdriver.
/// </summary>
/// 
/// <changelog>
///		<log author="Elijah Shadbolt" date="21/09/2020">
///			<para>Created this script.</para>
///		</log>
/// </changelog>
/// 
[RequireComponent(typeof(Dependable))]
[RequireComponent(typeof(V2_ButtonHandle))]
public class V3_Screw : MonoBehaviour
{
	public Dependable dependable { get; private set; }
	public V2_ButtonHandle buttonHandle { get; private set; }

	public bool hasUnscrewed { get; private set; } = false;

	private void Awake()
	{
		dependable = GetComponent<Dependable>();
		buttonHandle = GetComponent<V2_ButtonHandle>();
		buttonHandle.onClick += OnClick;
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
		if (hasUnscrewed) return;

		var puc = handleController.GetComponent<V2_PickUpController>();
		Debug.Assert(puc, this);
		if (puc)
		{
			if (puc.currentPickedUpHandle)
			{
				if (puc.currentPickedUpHandle.CompareTag("Screwdriver"))
				{
					hasUnscrewed = true;
					dependable.firstLiteral = true;
					dependable.ReEvaluate();
					gameObject.SetActive(false);
				}
			}
		}
	}
}
