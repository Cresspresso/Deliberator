
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///		Modified version of a third party script for having aspect ratio children of layout group.
/// </summary>
/// 
/// <original>
///		<author>Vapid-Linus</author>
///		<date>06/08/2013</date>
///		<a href="https://forum.unity.com/threads/solution-layoutelement-fit-parent-with-aspect-ratio.542212/">Forum</a>
///		<a href="https://gist.github.com/VapidLinus/d6d5f7cd62e5a8b961b63a3c46dff443">Source</a>
/// </original>
/// 
/// <changelog>
///		<log author="Elijah Shadbolt" date="21/09/2020">
///			<para>Imported this third party script.</para>
///			<para>Changed functionality so it expands height according to parent width, instead of fitting inside parent.</para>
///		</log>
/// </changelog>
/// 
[RequireComponent(typeof(RectTransform), typeof(LayoutElement), typeof(Image))]
[ExecuteAlways]
public class LayoutImageAspect : MonoBehaviour
{
	[SerializeField] private bool updateMin = false;
	[SerializeField] private bool updatePreferred = false;


	private bool isDirty = false;
	private RectTransform lastParent;
	private Vector2 lastParentSize;
	private Sprite lastSprite;

	private RectTransform rectTransform;
	private LayoutElement layoutElement;
	private Image image;

	public bool UpdateMin {
		get { return updateMin; }
		set
		{
			updateMin = value;
			isDirty = true;
		}
	}

	public bool UpdatePreferred {
		get { return updatePreferred; }
		set
		{
			updatePreferred = value;
			isDirty = true;
		}
	}

	private void OnEnable()
	{
		rectTransform = transform as RectTransform;
		layoutElement = GetComponent<LayoutElement>();
		image = GetComponent<Image>();

		isDirty = true;
	}

	private void Update()
	{
		if (lastParent != rectTransform.parent)
		{
			lastParent = rectTransform.parent as RectTransform;
			isDirty = true;
		}

		Vector2 parentSize = GetParentSize();

		// Mark as dirty if parent's size changes
		if (lastParentSize != parentSize)
		{
			lastParentSize = parentSize;
			isDirty = true;
		}

		Sprite sprite = image.sprite;
		if (lastSprite != sprite)
		{
			lastSprite = sprite;
			isDirty = true;
		}


		// Only recalculate layout size if something has changed
		if (!isDirty) return;
		isDirty = false;

		// MODIFIED CODE  Elijah Shadbolt
		if (image.sprite)
		{
			var rect = image.sprite.rect;
			float aspectRatio = rect.width / rect.height;
			SetSizes(parentSize.x, parentSize.x / aspectRatio);
		}
	}

#if UNITY_EDITOR
	private void OnValidate()
	{
		// Inspector fields have changed, mark as dirty
		isDirty = true;
	}
#endif

	private void SetSizes(float x, float y)
	{
		if (updateMin)
		{
			layoutElement.minWidth = x;
			layoutElement.minHeight = y;
		}
		if (updatePreferred)
		{
			layoutElement.preferredWidth = x;
			layoutElement.preferredHeight = y;
		}
	}

	private Vector2 GetParentSize()
	{
		var parent = rectTransform.parent as RectTransform;
		if (parent == null)
		{
			return Vector2.zero;
		}

		var layoutGroup = parent.GetComponent<LayoutGroup>();
		if (layoutGroup == null)
		{
			return parent.rect.size;
		}

		return parent.rect.size - new Vector2(layoutGroup.padding.horizontal, layoutGroup.padding.vertical);
	}
}