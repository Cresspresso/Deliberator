using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///		<para>A panel to go along with a <see cref="V3_SeparatorDragHandle"/>.</para>
/// </summary>
/// 
/// <changelog>
///		<log author="Elijah Shadbolt" date="28/09/2020">
///			<para>Added comments.</para>
///		</log>
/// </changelog>
/// 
[RequireComponent(typeof(LayoutElement))]
public class V3_SepPanel : MonoBehaviour
{
	public LayoutElement layoutElement { get; private set; }
	public V3_SeparatorDragHandle owner { get; private set; }

	/// <summary>
	///		<para>In the range [0, 1].</para>
	///		<para>Represents how much of the container this panel should occupy.</para>
	/// </summary>
	/// 
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
	private float m_amount;

	/// <summary>
	///		<para>Unity Message Method: <a href="https://docs.unity3d.com/ScriptReference/MonoBehaviour.Awake.html"/></para>
	/// </summary>
	/// 
	private void Awake()
	{
		layoutElement = GetComponent<LayoutElement>();
	}

	/// <summary>
	/// The size of the parent container <see cref="RectTransform"/>, in the direction the handle can be dragged.
	/// </summary>
	/// 
	private float containerSize;

	/// <summary>
	/// Called by a <see cref="V3_SeparatorDragHandle"/>.
	/// </summary>
	/// 
	public void Init(V3_SeparatorDragHandle owner)
	{
		this.owner = owner;
		containerSize = owner.container.rect.width;
		amount = 0.5f;
	}

	/// <summary>
	/// Updates the size of this panel based on the current pseudo-position of the separator handle.
	/// </summary>
	/// 
	private void UpdateSize()
	{
		layoutElement.preferredWidth = Mathf.Max(layoutElement.minWidth, amount * containerSize);
	}

	/// <summary>
	///		<para>Unity Message Method: <a href="https://docs.unity3d.com/ScriptReference/MonoBehaviour.Update.html"/></para>
	/// </summary>
	/// 
	private void Update()
	{
		if (containerSize != owner.container.rect.width)
		{
			containerSize = owner.container.rect.width;
			UpdateSize();
		}
	}
}
