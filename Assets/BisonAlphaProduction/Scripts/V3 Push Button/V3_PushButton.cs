using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///		<para>
///			A button that outputs power to <see cref="Dependable"/>,
///			stays on for a short duration,
///			then stops outputting power.
///		</para>
///		<para>
///			The <see cref="Dependable"/>'s first literal (<see cref="Dependable.firstLiteral"/>) is changed by this script.
///		</para>
/// </summary>
/// 
/// <changelog>
///		<log author="Elijah Shadbolt" date="25/08/2020">
///			<para>Added this script.</para>
///			<para>Removed the UnityEvent listeners from the V3 Push Button prefab.</para>
///		</log>
/// </changelog>
/// 
[RequireComponent(typeof(Dependable))]
[RequireComponent(typeof(V2_ButtonHandle))]
public class V3_PushButton : MonoBehaviour
{
	public Dependable dependable { get; private set; }
	public V2_ButtonHandle buttonHandle { get; private set; }
	public Animator anim { get; private set; }

#pragma warning disable CS0649
	[SerializeField]
	private AudioSource m_audioSource;
#pragma warning restore CS0649
	public AudioSource audioSource => m_audioSource;



	private void Awake()
	{
		dependable = GetComponent<Dependable>();
		anim = GetComponent<Animator>();

		buttonHandle = GetComponent<V2_ButtonHandle>();
		buttonHandle.onClick += (buttonHandle, handleController) =>
		{
			dependable.firstLiteral = true;
			audioSource.Play();
			anim.SetTrigger("Active");
		};
	}

	public void AnimEventEnd()
	{
		dependable.firstLiteral = false;
	}
}
