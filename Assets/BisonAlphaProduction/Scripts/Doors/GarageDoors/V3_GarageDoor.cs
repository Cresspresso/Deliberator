using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Dependable))]
public class V3_GarageDoor : MonoBehaviour
{
	public Dependable dependable { get; private set; }

	[SerializeField]
	private Animator m_anim;
	public Animator anim => m_anim;

	private void Awake()
	{
		dependable = GetComponent<Dependable>();
		dependable.onChanged.AddListener((isPowered) =>
		{
			if (isOpen) return;
			if (!isPowered) return;

			Open();
		});
	}

	public bool isOpen => dependable.firstLiteral;

	public void Open()
	{
		if (isOpen) return;

		dependable.firstLiteral = true;
		anim.enabled = true;
	}

	public void AnimEventStop()
	{
		anim.enabled = false;
	}
}
