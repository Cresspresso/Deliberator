using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// sibling to Interactable
public class Old_SwingDoor : Old_Interactable
{
	public Animator anim;

	private bool m_isOpen;
	public bool isOpen {
		get => m_isOpen;
		set
		{
			m_isOpen = value;

			anim.SetBool("isOpen", value);

			isHoverInfoDirty = true;

			if (hasBeenInteracted)
			{
				var am = FindObjectOfType<Old_AudioManager>();
				if (am)
				{
					if (value)
					{
						am.PlaySound("doorOpen");
					}
					else
					{
						am.PlaySound("doorClose");
					}
				}
			}
		}
	}

	public void UpdateHoverInfo()
	{
		this.hoverDescription = hasBeenInteracted
			? (isLocked
				? "Locked"
				: (isOpen
					? "Close"
					: "Open"))
			: "Try Door";
	}

	public bool openOnAwake = false;
	public bool isLocked = false;

	private bool m_hasBeenInteracted = false;
	public bool hasBeenInteracted {
		get => m_hasBeenInteracted;
		private set
		{
			m_hasBeenInteracted = value;
			isHoverInfoDirty = true;
		}
	}

	private bool isHoverInfoDirty = false;

	private void Awake()
	{
		isOpen = openOnAwake;
	}

	private void LateUpdate()
	{
		if (isHoverInfoDirty)
		{
			UpdateHoverInfo();
			isHoverInfoDirty = false;
		}
	}

	protected override void OnInteract(Old_InteractEventArgs eventArgs)
	{
		hasBeenInteracted = true;
		if (isLocked)
		{
			anim.SetTrigger("TriedToOpenLocked");

			var am = FindObjectOfType<Old_AudioManager>();
			if (am) { am.PlaySound("doorLocked"); }
		}
		else
		{
			isOpen = !isOpen;
		}
	}
}
