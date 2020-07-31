using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class V2_SigilUI : MonoBehaviour
{
	public Image mainImage;
	public Image secondImage;
	public GameObject helperVisuals;
	public float width = 600;
	[SerializeField]
	private List<Sprite> m_sigilSprites = new List<Sprite>();
	public IReadOnlyList<Sprite> sigilSprites => m_sigilSprites;
	public int currentSigilSpriteIndex { get; private set; } = 0;

	public void AddSigilSprite(Sprite sprite)
	{
		if (!sprite) { throw new System.ArgumentNullException(); }
		m_sigilSprites.Add(sprite);
		var count = m_sigilSprites.Count;
		if (count == 1)
		{
			currentSigilSpriteIndex = 0;
			mainImage.sprite = m_sigilSprites[currentSigilSpriteIndex];
		}
		else if (count >= 2)
		{
			helperVisuals.gameObject.SetActive(true);
			Transition(count - 1, true);
		}
	}

	public bool ContainsSigilSprite(Sprite sprite)
	{
		return m_sigilSprites.Contains(sprite);
	}

	private void Awake()
	{
		secondImage.gameObject.SetActive(false);
		helperVisuals.SetActive(false);

		if (m_sigilSprites.Count > 0)
		{
			mainImage.sprite = m_sigilSprites[currentSigilSpriteIndex];
		}
	}

	private bool isTweening = false;

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.E) && !Input.GetKeyDown(KeyCode.Q))
		{
			Transition(V2_Utility.Cycle(currentSigilSpriteIndex - 1, m_sigilSprites.Count), right: true);
		}
		else if (Input.GetKeyDown(KeyCode.Q))
		{
			Transition(V2_Utility.Cycle(currentSigilSpriteIndex + 1, m_sigilSprites.Count), right: false);
		}
	}

	private void Transition(int newSigilSpriteIndex, bool right)
	{
		if (isTweening)
		{
			return;
		}
		else if (m_sigilSprites.Count == 0)
		{
			return;
		}
		else if (m_sigilSprites.Count == 1)
		{
			isTweening = true;
			// shake the image
			DOTween.Sequence()
				.Append(DOTween.Shake(
					() => mainImage.rectTransform.anchoredPosition,
					(value) => mainImage.rectTransform.anchoredPosition = value,
					0.5f,
					new Vector3(200, 0)))
				.AppendCallback(() =>
				{
					isTweening = false;
					secondImage.gameObject.SetActive(false);
				});
		}
		else
		{
			var k = width;
			if (right) { k = -k; }

			secondImage.sprite = mainImage.sprite;
			secondImage.rectTransform.anchoredPosition = new Vector3(0, 0, 0);
			secondImage.gameObject.SetActive(true);

			currentSigilSpriteIndex = newSigilSpriteIndex;
			mainImage.sprite = sigilSprites[currentSigilSpriteIndex];
			mainImage.rectTransform.anchoredPosition = new Vector3(-k, 0, 0);

			isTweening = true;
			DOTween.Sequence()
				.Append(DOTween.To(
					() => mainImage.rectTransform.anchoredPosition.x,
					(value) => mainImage.rectTransform.anchoredPosition = new Vector2(value, 0),
					0,
					0.5f))
				.Insert(0, DOTween.To(
					() => secondImage.rectTransform.anchoredPosition.x,
					(value) => secondImage.rectTransform.anchoredPosition = new Vector2(value, 0),
					k,
					0.5f))
				.AppendCallback(() =>
				{
					isTweening = false;
					secondImage.gameObject.SetActive(false);
				});
		}
	}
}
