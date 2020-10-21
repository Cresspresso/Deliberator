using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
///		<para>
///			An item that can be used on screws to unlock a vent door.
///		</para>
///		<para>
///			Can be used to open one vent door, after which it gets broken.
///		</para>
/// </summary>
/// 
/// <changelog>
///		<log author="Elijah Shadbolt" date="13/10/2020">
///			<para>Created this script.</para>
///		</log>
/// </changelog>
/// 
[RequireComponent(typeof(V2_PickUpHandle))]
public class V4_Screwdriver : MonoBehaviour
{
	public V2_PickUpHandle pickupHandle { get; private set; }

#pragma warning disable CS0649

	[SerializeField]
	private GameObject m_visualsRegular;
	public GameObject visualsRegular => m_visualsRegular;

	[SerializeField]
	private GameObject m_visualsDamaged;
	public GameObject visualsDamaged => m_visualsDamaged;

#pragma warning restore CS0649

	public bool hasExpired { get; private set; } = false;

	private void Awake()
	{
		visualsRegular.SetActive(true);
		visualsDamaged.SetActive(false);

		pickupHandle = GetComponent<V2_PickUpHandle>();
	}

	public void Expire()
	{
		if (hasExpired) return;
		hasExpired = true;

		visualsDamaged.SetActive(true);
		visualsRegular.SetActive(false);

		Debug.LogWarning("TODO play breaking sound (e.g. metal clang)", this);

		V4_PlayerAnimator.instance.OnScrewdriverExpired(this);
	}
}
