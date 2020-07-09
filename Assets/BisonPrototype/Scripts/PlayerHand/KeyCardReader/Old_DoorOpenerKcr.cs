using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Old_KeyCardReader))]
public class Old_DoorOpenerKcr : MonoBehaviour
{
	public Old_KeyCardReader interactable { get; private set; }
	public Animator anim;

	private void Awake()
	{
		interactable = GetComponent<Old_KeyCardReader>();

		interactable.interactEventComponent.onInteract.AddListener(OnInteract);
	}

	private void OnDestroy()
	{
		interactable.interactEventComponent.onInteract.RemoveListener(OnInteract);
	}

	private void OnInteract(Old_InteractEventArgs eventArgs)
	{
		anim.SetBool("doorOpen", true);

		var am = FindObjectOfType<Old_AudioManager>();
		if (am) { am.PlaySound("labdoorOpen"); }
	}
}
