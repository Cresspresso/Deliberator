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
			}
		}
	}

	private void Awake()
	{
		FindButtonHandle();
	}

	public AudioSource sound;

	private void OnClick(ButtonHandle buttonHandle, HandleController handleController)
	{
		pad.OnNumPadKeyPressed(type);

		if (sound)
		{
			sound.Play();
		}
	}
}
