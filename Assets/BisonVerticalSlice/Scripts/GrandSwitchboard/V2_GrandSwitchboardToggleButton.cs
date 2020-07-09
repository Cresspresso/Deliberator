using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
/// <author>Elijah Shadbolt</author>
[RequireComponent(typeof(V2_ButtonHandle))]
public class V2_GrandSwitchboardToggleButton : MonoBehaviour
{
	public V2_ButtonHandle buttonHandle => GetComponent<V2_ButtonHandle>();
	public MeshRenderer meshRenderer => GetComponentInChildren<MeshRenderer>();
	public V2_GrandSwitchboard grandSwitchboard => GetComponentInParent<V2_GrandSwitchboard>();

	private bool m_isOn;
	public bool isOn {
		get => m_isOn;
		set
		{
			m_isOn = value;
			try
			{
				if (m_isOn)
				{
					meshRenderer.material.DOColor(onColor, "_BaseColor", duration);
					transform.DOLocalMoveY(-distance, duration);
				}
				else
				{
					meshRenderer.material.color = Color.white;
					meshRenderer.material.DOColor(offColor, "_BaseColor", duration);
					transform.DOLocalMoveY(0.0f, duration);
				}
			}
			finally
			{
				grandSwitchboard.OnButtonToggleChanged(coords, m_isOn);
			}
		}
	}
	public Vector2Int coords = Vector2Int.zero;

	public float duration = 0.5f;
	public float distance = 0.1f;
	public Color onColor = Color.cyan;
	private Color offColor;

	private void OnEnable()
	{
		buttonHandle.onClick -= OnClick;
		buttonHandle.onClick += OnClick;
		offColor = meshRenderer.material.GetColor("_BaseColor");
	}

	private void OnDisable()
	{
		if (buttonHandle)
		{
			buttonHandle.onClick -= OnClick;
		}
	}

	private void OnClick(V2_ButtonHandle buttonHandle, V2_HandleController handleController)
	{
		isOn = !isOn;
	}
}
