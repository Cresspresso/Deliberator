using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class V3_SeparatorDragHandle : MonoBehaviour,
	IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler
{
	[SerializeField]
	private RectTransform m_container;
	public RectTransform container => m_container;

	[SerializeField]
	private RectTransform m_canvasRectTransform;
	public RectTransform canvasRectTransform => m_canvasRectTransform;

	[SerializeField]
	private RectTransform m_handle;
	public RectTransform handle => m_handle;

	[SerializeField]
	private V3_SepPanel m_firstPanel;
	public V3_SepPanel firstPanel => m_firstPanel;

	[SerializeField]
	private V3_SepPanel m_secondPanel;
	public V3_SepPanel secondPanel => m_secondPanel;

	[SerializeField]
	private float m_extension = 100;
	public float extension => m_extension;

	[SerializeField]
	private float m_animSpeed = 3;
	public float animSpeed => m_animSpeed;

	// in range 0..1
	private float amount = 0;

	private bool isHovered = false;

	private bool isDragging = false;

	private void Awake()
	{
		if (!m_canvasRectTransform)
		{
			m_canvasRectTransform = GetComponentInParent<Canvas>().transform as RectTransform;
		}

		if (!m_container)
		{
			m_container = transform.parent as RectTransform;
		}

		UpdateHandle();
	}

	private void Start()
	{
		firstPanel.Init(this);
		secondPanel.Init(this);
	}

	private void Update()
	{
		bool isDirty = false;
		bool isShow = isHovered || isDragging;
		if (isShow && amount < 1)
		{
			amount = Mathf.MoveTowards(amount, 1, animSpeed * Time.unscaledDeltaTime);
			isDirty = true;
		}
		else if (!isShow && amount > 0)
		{
			amount = Mathf.MoveTowards(amount, 0, animSpeed * Time.unscaledDeltaTime);
			isDirty = true;
		}

		if (isDirty)
		{
			UpdateHandle();
		}
	}

	private void UpdateHandle()
	{
		float size = amount * extension;
		handle.sizeDelta = new Vector2(size, handle.sizeDelta.y);

		if (handle.gameObject.activeSelf)
		{
			if (size <= 0)
			{
				handle.gameObject.SetActive(false);
			}
		}
		else
		{
			if (size > 0)
			{
				handle.gameObject.SetActive(true);
			}
		}
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Left)
		{
			isHovered = true;
		}
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Left)
		{
			isHovered = false;
		}
	}

	public void OnPointerDown(PointerEventData eventData)
	{
	}

	public void OnPointerUp(PointerEventData eventData)
	{
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		isDragging = true;
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		isDragging = false;
	}

	public void OnDrag(PointerEventData eventData)
	{
		float delta = eventData.delta.x / container.rect.width;
		firstPanel.amount = Mathf.Clamp01(firstPanel.amount + delta);
		secondPanel.amount = 1.0f - firstPanel.amount;
	}
}
