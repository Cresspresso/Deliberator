using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Dependable))]
public class V3_GlassChamber : MonoBehaviour
{
	public Dependable dependable { get; private set; }

	[SerializeField]
	private Animator m_anim;
	public Animator anim => m_anim;

	[SerializeField]
	private GameObject m_doorColliderGO;
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
