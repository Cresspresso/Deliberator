using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(ButtonHandle))]
public class NumPadKey : MonoBehaviour
{
	[SerializeField]
	private NumPadKeyType m_type = NumPadKeyType.Num0;
	public NumPadKeyType type => m_type;

	public AudioSource sound;

	[SerializeField]
	private NumPad m_pad;
	public NumPad pad {
		get
		{
			if (!m_pad)
			{
				m_pad = GetComponentInParent<NumPad>();
			}
			return m_pad;
		}
	}

	private ButtonHandle m_buttonHandle;
	public ButtonHandle buttonHandle {
		get
		{
			FindButtonHandle();
			return m_buttonHandle;
		}
	}
	private void FindButtonHandle()
	{
		if (!m_buttonHandle)
		{
			m_buttonHandle = GetComponent<ButtonHandle>();
			if (m_buttonHandle)
			{
				m_buttonHandle.onClick += OnClick;
				m_buttonHandle.handle.onHoverEnter += OnHoverEnter;
				m_buttonHandle.handle.onHoverExit += OnHoverExit;
			}
		}
	}

	private void Awake()
	{
		FindButtonHandle();
	}

	private void OnHoverEnter(Handle handle, HandleController handleController)
	{
		pad.OnHoverEnter(handleController);
	}

	private void OnHoverExit(Handle handle, HandleController handleController)
	{
		pad.OnHoverExit(handleController);
	}

	private void OnClick(ButtonHandle buttonHandle, HandleController handleController)
	{
		pad.OnNumPadKeyPressed(type);

		if (sound)
		{
			sound.Play();
		}
	}
}
