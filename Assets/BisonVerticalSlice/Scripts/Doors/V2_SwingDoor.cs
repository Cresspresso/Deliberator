using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <author>Elijah Shadbolt</author>
[RequireComponent(typeof(V2_ButtonHandle))]
public class V2_SwingDoor : MonoBehaviour
{
	public bool openOnAwake = false;
	public bool isLocked = false;
	public bool isOpen { get; private set; } = false;

	public Animator anim;
	public AudioSource lockedSound;
	public AudioSource openingSound;
	public AudioSource closingSound;

	public V2_HandleHoverInfo lockedHoverInfo = new V2_HandleHoverInfo("Locked");
	public V2_HandleHoverInfo openedHoverInfo = new V2_HandleHoverInfo("Close");
	public V2_HandleHoverInfo closedHoverInfo = new V2_HandleHoverInfo("Open");

	private V2_ButtonHandle m_buttonHandle;
	public V2_ButtonHandle buttonHandle { get { PrepareButtonHandle(); return m_buttonHandle; } }
	private void PrepareButtonHandle()
	{
		if (!m_buttonHandle)
		{
			m_buttonHandle = GetComponent<V2_ButtonHandle>();
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

	private void OnClick(V2_ButtonHandle buttonHandle, V2_HandleController handleController)
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
