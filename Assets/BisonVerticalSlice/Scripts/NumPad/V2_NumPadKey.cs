using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(V2_ButtonHandle))]
public class V2_NumPadKey : MonoBehaviour
{
	[SerializeField]
	private V2_NumPadKeyType m_type = V2_NumPadKeyType.Num0;
	public V2_NumPadKeyType type => m_type;

	public AudioSource sound;

	[SerializeField]
	private V2_NumPad m_pad;
	public V2_NumPad pad {
		get
		{
			if (!m_pad)
			{
				m_pad = GetComponentInParent<V2_NumPad>();
			}
			return m_pad;
		}
	}

	private V2_ButtonHandle m_buttonHandle;
	public V2_ButtonHandle buttonHandle {
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
			m_buttonHandle = GetComponent<V2_ButtonHandle>();
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

	private void OnHoverEnter(V2_Handle handle, V2_HandleController handleController)
	{
		pad.OnHoverEnter(handleController);
	}

	private void OnHoverExit(V2_Handle handle, V2_HandleController handleController)
	{
		pad.OnHoverExit(handleController);
	}

	private void OnClick(V2_ButtonHandle buttonHandle, V2_HandleController handleController)
	{
		pad.OnNumPadKeyPressed(type);

		if (sound)
		{
			sound.Play();
		}
	}
}
