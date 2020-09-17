using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(V2_ButtonHandle))]
public class V3_PaperClue : MonoBehaviour
{
	[TextArea(3, 8)]
	[SerializeField]
	private string m_content = "Placeholder";
	public string content { get => m_content; set => m_content = value; }

	private V2_ButtonHandle m_buttonHandle;
	public V2_ButtonHandle buttonHandle {
		get
		{
			PrepareButtonHandle();
			return m_buttonHandle;
		}
	}
	private void PrepareButtonHandle()
	{
		if (!m_buttonHandle)
		{
			m_buttonHandle = GetComponent<V2_ButtonHandle>();
			if (m_buttonHandle)
			{
				m_buttonHandle.onClick += OnClick;
			}
		}
	}

	private void Awake()
	{
		PrepareButtonHandle();
	}

	private void OnClick(V2_ButtonHandle buttonHandle, V2_HandleController handleController)
	{
		var rm = FindObjectOfType<V3_ReadableMenu>();
		rm.SetText(content);
		rm.Pause();
	}
}
