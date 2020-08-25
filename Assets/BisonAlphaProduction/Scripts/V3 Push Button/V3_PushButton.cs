using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Dependable))]
[RequireComponent(typeof(V2_ButtonHandle))]
public class V3_PushButton : MonoBehaviour
{
	public Dependable dependable { get; private set; }
	public V2_ButtonHandle buttonHandle { get; private set; }
	public Animator anim { get; private set; }

	[SerializeField]
	private AudioSource m_audioSource;
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
