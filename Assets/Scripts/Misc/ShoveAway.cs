using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShoveAway : MonoBehaviour
{
	[SerializeField]
	private ButtonHandle m_handle;
	public ButtonHandle handle {
		get
		{
			Find();
			return m_handle;
		}
	}

	private void Find()
	{
		if (!m_handle)
		{
			m_handle = GetComponent<ButtonHandle>();
			if (m_handle)
			{
				// subscribe
				m_handle.onClick += OnClick;
			}
		}
	}

	private void Awake()
	{
		Find();
	}

	private void OnClick(ButtonHandle bh, HandleController handleController)
	{
		transform.position += transform.forward * 1.0f;
		bh.handle.description = "push again";
	}
}
