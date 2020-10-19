using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///		<para>A glass chamber display case that can be opened.</para>
/// </summary>
/// 
/// <changelog>
///		<log author="Elijah Shadbolt" date="25/08/2020">
///			<para>Added this script.</para>
///		</log>
///		<log author="Elijah Shadbolt" date="19/10/2020">
///			<para>Added visual effect.</para>
///		</log>
/// </changelog>
/// 
[RequireComponent(typeof(Dependable))]
public class V3_GlassChamber : MonoBehaviour
{
	public Dependable dependable { get; private set; }



#pragma warning disable CS0649

	[SerializeField]
	private Animator m_anim;
	public Animator anim => m_anim;



	[SerializeField]
	private GameObject m_doorColliderGO;
	public GameObject doorColliderGO => m_doorColliderGO;



	[SerializeField]
	private GameObject m_vfx;
	public GameObject vfx => m_vfx;

#pragma warning restore CS0649



	private void Awake()
	{
		dependable = GetComponent<Dependable>();
		dependable.onChanged.AddListener(OnPoweredChanged);

		if (vfx)
		{
			vfx.SetActive(false);
		}
	}



	private void OnPoweredChanged(bool isPowered)
	{
		if (!enabled) return;
		if (!isPowered) return;

		anim.SetTrigger("Open");
		doorColliderGO.SetActive(false);

		if (vfx)
		{
			vfx.SetActive(true);
		}

		enabled = false;
	}
}
