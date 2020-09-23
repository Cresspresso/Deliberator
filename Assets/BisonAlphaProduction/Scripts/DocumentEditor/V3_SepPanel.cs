using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(LayoutElement))]
public class V3_SepPanel : MonoBehaviour
{
	public V3_SeparatorDragHandle owner { get; private set; }
	public LayoutElement layoutElement { get; private set; }

	private float m_amount;
	public float amount
	{
		get => m_amount;
		set
		{
			m_amount = value;
			UpdateSize();
			m_amount = layoutElement.preferredWidth / containerSize;
		}
	}

	private void Awake()
	{
		layoutElement = GetComponent<LayoutElement>();
	}

	private float containerSize;

	public void Init(V3_SeparatorDragHandle owner)
	{
		this.owner = owner;
		containerSize = owner.container.rect.width;
		amount = 0.5f;
	}

	private void UpdateSize()
	{
		layoutElement.preferredWidth = Mathf.Max(layoutElement.minWidth, amount * containerSize);
	}

	private void Update()
	{
		if (containerSize != owner.container.rect.width)
		{
			containerSize = owner.container.rect.width;
			UpdateSize();
		}
	}
}
