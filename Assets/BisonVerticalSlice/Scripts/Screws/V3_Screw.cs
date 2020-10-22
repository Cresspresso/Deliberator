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
///		<log author="Elijah Shadbolt" date="13/10/2020">
///			<para>Edited so that player's screwdriver must not be broken for this to be unscrewed by it.</para>
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
			var puHandle = puc.currentPickedUpHandle;
			if (puHandle)
			{
				var screwdriver = puHandle.GetComponent<V4_Screwdriver>();
				if (screwdriver)
				{
					if (screwdriver.hasExpired)
					{
						Debug.LogWarning("TODO play invalid sound (e.g. metal clang)", this);
					}
					else
					{
						if (V4_PlayerAnimator.instance.PerformRightHandAction(() =>
						{
							dependable.firstLiteral = true;
							dependable.ReEvaluate();
							gameObject.SetActive(false);
						}))
						{
							hasUnscrewed = true;
							gameObject.GetComponent<AudioSource>().Play();
						}
					}
				}
			}
		}
	}
}
