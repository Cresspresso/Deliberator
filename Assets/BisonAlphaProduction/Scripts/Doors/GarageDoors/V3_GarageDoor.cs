using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///		<para>
///			A garage door that opens vertically.
///		</para>
///		<para>No fancy garage door opening antics (just straight up, no spiral).</para>
///		<para>
///			The <see cref="Dependable"/>'s first literal (<see cref="Dependable.firstLiteral"/>)
///			will be changed to true, to indicate this door is open.
///		</para>
///		<para>
///			The <see cref="Dependable.condition"/> should be of the form ((Literal[0]) || (external dependencies expression)).
///		</para>
/// </summary>
/// 
/// <changelog>
///		<log author="Elijah Shadbolt" date="25/08/2020">
///			<para>Added this script.</para>
///		</log>
/// </changelog>
/// 
[RequireComponent(typeof(Dependable))]
public class V3_GarageDoor : MonoBehaviour
{
	public Dependable dependable { get; private set; }

#pragma warning disable CS0649
	[SerializeField]
	private Animator m_anim;
#pragma warning restore CS0649
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
