using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
///		<para>
///			A vertical separator between two UI panels,
///			which the player can drag to change the width of the panels.
///		</para>
///		<para></para>
/// </summary>
/// 
/// <changelog>
///		<log author="Elijah Shadbolt" date="28/09/2020">
///			<para>Added comments.</para>
///			<para>Suppressed warning CS0649</para>
///		</log>
/// </changelog>
/// 
public class V3_SeparatorDragHandle : MonoBehaviour,
	IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler
{
#pragma warning disable CS0649

	/// <summary>
	///		<para>The direct parent of this <see cref="RectTransform"/> and the two panels.</para>
	/// </summary>
	/// 
	[SerializeField]
	private RectTransform m_container;
	public RectTransform container => m_container;

	/// <summary>
	///		<para>The <see cref="RectTransform"/> of the <see cref="Canvas"/> that this <see cref="RectTransform"/> is a child or sub-child of.</para>
	/// </summary>
	/// 
	[SerializeField]
	private RectTransform m_canvasRectTransform;
	public RectTransform canvasRectTransform => m_canvasRectTransform;

	/// <summary>
	///		<para>A graphic to reveal when the user hovers over this <see cref="RectTransform"/>.</para>
	/// </summary>
	/// 
	[SerializeField]
	private RectTransform m_handle;
	public RectTransform handle => m_handle;

	/// <summary>
	///		<para>For a vertical separator, the left column panel.</para>
	/// </summary>
	/// 
	[SerializeField]
	private V3_SepPanel m_firstPanel;
	public V3_SepPanel firstPanel => m_firstPanel;

	/// <summary>
	///		<para>For a vertical separator, the right column panel.</para>
	/// </summary>
	/// 
	[SerializeField]
	private V3_SepPanel m_secondPanel;
	public V3_SepPanel secondPanel => m_secondPanel;

	/// <summary>
	///		<para>How large the handle graphic expands to.</para>
	/// </summary>
	/// 
	[SerializeField]
	private float m_extension = 100;
	public float extension => m_extension;

	/// <summary>
	///		<para>How fast the handle graphic expands and contracts.</para>
	/// </summary>
	/// 
	[SerializeField]
	private float m_animSpeed = 3;
	public float animSpeed => m_animSpeed;

#pragma warning restore CS0649

	/// <summary>
	///		<para>In the range [0, 1].</para>
	///		<para>The current 'position' of the separator between the two panels.</para>
	///		<para>A value of 0 means the first panel is small and the second panel is large.</para>
	///		<para>A value of 1 means the first panel is large and the second panel is small.</para>
	/// </summary>
	/// 
	private float amount = 0;

	/// <summary>
	///		<para>See also:</para>
	///		<para><see cref="OnPointerEnter(PointerEventData)"/></para>
	///		<para><see cref="OnPointerExit(PointerEventData)"/></para>
	/// </summary>
	/// 
	private bool isHovered = false;

	/// <summary>
	///		<para>See also:</para>
	///		<para><see cref="OnBeginDrag(PointerEventData)"/></para>
	///		<para><see cref="OnEndDrag(PointerEventData)"/></para>
	/// </summary>
	/// 
	private bool isDragging = false;



	/// <summary>
	///		<para>Unity Message Method: <a href="https://docs.unity3d.com/ScriptReference/MonoBehaviour.Awake.html"/></para>
	/// </summary>
	/// 
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

	/// <summary>
	///		<para>Unity Message Method: <a href="https://docs.unity3d.com/ScriptReference/MonoBehaviour.Start.html"/></para>
	/// </summary>
	/// 
	private void Start()
	{
		firstPanel.Init(this);
		secondPanel.Init(this);
	}

	/// <summary>
	///		<para>Unity Message Method: <a href="https://docs.unity3d.com/ScriptReference/MonoBehaviour.Update.html"/></para>
	/// </summary>
	/// 
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

	/// <summary>
	///		<para>Updates the size of the UI element based on the current state of this script.</para>
	/// </summary>
	/// 
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

	/// <summary>
	///		<para>Unity UI Event Trigger interface override.</para>
	///		<para><see cref="IPointerEnterHandler"/></para>
	///		<para><a href="https://docs.unity3d.com/Packages/com.unity.ugui@1.0/manual/SupportedEvents.html"/></para>
	/// </summary>
	/// 
	public void OnPointerEnter(PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Left)
		{
			isHovered = true;
		}
	}

	/// <summary>
	///		<para>Unity UI Event Trigger interface override.</para>
	///		<para><see cref="IPointerExitHandler"/></para>
	///		<para><a href="https://docs.unity3d.com/Packages/com.unity.ugui@1.0/manual/SupportedEvents.html"/></para>
	/// </summary>
	/// 
	public void OnPointerExit(PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Left)
		{
			isHovered = false;
		}
	}

	/// <summary>
	///		<para>Unity UI Event Trigger interface override.</para>
	///		<para><see cref="IPointerDownHandler"/></para>
	///		<para><a href="https://docs.unity3d.com/Packages/com.unity.ugui@1.0/manual/SupportedEvents.html"/></para>
	/// </summary>
	/// 
	public void OnPointerDown(PointerEventData eventData)
	{
	}

	/// <summary>
	///		<para>Unity UI Event Trigger interface override.</para>
	///		<para><see cref="IPointerUpHandler"/></para>
	///		<para><a href="https://docs.unity3d.com/Packages/com.unity.ugui@1.0/manual/SupportedEvents.html"/></para>
	/// </summary>
	/// 
	public void OnPointerUp(PointerEventData eventData)
	{
	}

	/// <summary>
	///		<para>Unity UI Event Trigger interface override.</para>
	///		<para><see cref="IBeginDragHandler"/></para>
	///		<para><a href="https://docs.unity3d.com/Packages/com.unity.ugui@1.0/manual/SupportedEvents.html"/></para>
	/// </summary>
	/// 
	public void OnBeginDrag(PointerEventData eventData)
	{
		isDragging = true;
	}


	/// <summary>
	///		<para>Unity UI Event Trigger interface override.</para>
	///		<para><see cref="IEndDragHandler"/></para>
	///		<para><a href="https://docs.unity3d.com/Packages/com.unity.ugui@1.0/manual/SupportedEvents.html"/></para>
	/// </summary>
	/// 
	public void OnEndDrag(PointerEventData eventData)
	{
		isDragging = false;
	}

	/// <summary>
	///		<para>Unity UI Event Trigger interface override.</para>
	///		<para><see cref="IDragHandler"/></para>
	///		<para><a href="https://docs.unity3d.com/Packages/com.unity.ugui@1.0/manual/SupportedEvents.html"/></para>
	/// </summary>
	/// 
	public void OnDrag(PointerEventData eventData)
	{
		float delta = eventData.delta.x / container.rect.width;
		firstPanel.amount = Mathf.Clamp01(firstPanel.amount + delta);
		secondPanel.amount = 1.0f - firstPanel.amount;
	}
}
