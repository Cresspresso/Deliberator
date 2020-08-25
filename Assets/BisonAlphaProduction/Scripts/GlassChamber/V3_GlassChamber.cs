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
/// </changelog>
/// 
[RequireComponent(typeof(Dependable))]
public class V3_GlassChamber : MonoBehaviour
{
	public Dependable dependable { get; private set; }



#pragma warning disable CS0649
	[SerializeField]
	private Animator m_anim;
#pragma warning restore CS0649
	public Animator anim => m_anim;



#pragma warning disable CS0649
	[SerializeField]
	private GameObject m_doorColliderGO;
#pragma warning restore CS0649
	public GameObject doorColliderGO => m_doorColliderGO;



	private void Awake()
	{
		dependable = GetComponent<Dependable>();
		dependable.onChanged.AddListener(OnPoweredChanged);
	}



	private void OnPoweredChanged(bool isPowered)
	{
		if (!enabled) return;
		if (!isPowered) return;

		anim.SetTrigger("Open");
		doorColliderGO.SetActive(false);

		enabled = false;
	}
}
