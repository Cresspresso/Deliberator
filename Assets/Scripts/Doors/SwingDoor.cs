using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <author>Elijah Shadbolt</author>
[RequireComponent(typeof(ButtonHandle))]
public class SwingDoor : MonoBehaviour
{
	public bool openOnAwake = false;
	public bool isLocked = false;
	public bool isOpen { get; private set; } = false;

	public Animator anim;
	public AudioSource lockedSound;
	public AudioSource openingSound;
	public AudioSource closingSound;

	public HandleHoverInfo lockedHoverInfo = new HandleHoverInfo("Locked");
	public HandleHoverInfo openedHoverInfo = new HandleHoverInfo("Close");
	public HandleHoverInfo closedHoverInfo = new HandleHoverInfo("Open");

	private ButtonHandle m_buttonHandle;
	public ButtonHandle buttonHandle { get { PrepareButtonHandle(); return m_buttonHandle; } }
	private void PrepareButtonHandle()
	{
		if (!m_buttonHandle)
		{
			m_buttonHandle = GetComponent<ButtonHandle>();
			if (m_buttonHandle)
			{
				m_buttonHandle.onClick -= OnClick;
				m_buttonHandle.onClick += OnClick;
			}
			else
			{
				Debug.LogError("ButtonHandle is null", this);
			}
		}
	}

	private void Awake()
	{
		PrepareButtonHandle();
	}

	private void OnClick(ButtonHandle buttonHandle, HandleController handleController)
	{
		if (isLocked)
		{
			anim.SetTrigger("TriedToOpenLocked");

			buttonHandle.handle.hoverInfo = lockedHoverInfo;

			if (lockedSound) { lockedSound.Play(); }
		}
		else
		{
			isOpen = !isOpen;

			anim.SetBool("isOpen", isOpen);

			buttonHandle.handle.hoverInfo = isOpen ? openedHoverInfo : closedHoverInfo;

			// audio
			if (isOpen)
			{
				if (openingSound) { openingSound.Play(); }
			}
			else
			{
				if (closingSound) { closingSound.Play(); }
			}
		}
	}

	private void OnEnable()
	{
		isOpen = openOnAwake;
		anim.SetBool("isOpen", isOpen);
	}

	private void OnDisable()
	{
		if (m_buttonHandle)
		{
			m_buttonHandle.onClick -= OnClick;
		}
	}
}
