using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class V2_ShoveAway : MonoBehaviour
{
	public V2_HandleHoverInfo nextInfo = new V2_HandleHoverInfo("Push Again", null);

	[SerializeField]
	private V2_ButtonHandle m_handle;
	public V2_ButtonHandle handle {
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
			m_handle = GetComponent<V2_ButtonHandle>();
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

	private void OnClick(V2_ButtonHandle bh, V2_HandleController handleController)
	{
		transform.position += transform.forward * 1.0f;
		bh.handle.hoverInfo = nextInfo;
	}
}
