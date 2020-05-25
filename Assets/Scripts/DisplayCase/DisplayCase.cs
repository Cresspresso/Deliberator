using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayCase : MonoBehaviour
{
	public Animator anim { get; private set; }
	public Handle glassDoor;

	private void Awake()
	{
		anim = GetComponent<Animator>();
	}

	public void Open()
	{
		anim.SetTrigger("Open");
		glassDoor.enabled = false;
	}

	public void Close()
	{
		anim.SetTrigger("Close");
		glassDoor.enabled = true;
	}
}
