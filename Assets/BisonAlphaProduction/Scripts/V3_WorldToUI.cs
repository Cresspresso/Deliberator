using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class V3_WorldToUI : MonoBehaviour
{
	public Transform target = null;
	public RectTransform visuals;
	private RectTransform canvasRt;

	private void Awake()
	{
		canvasRt = (RectTransform)(GetComponentInParent<Canvas>().transform);
		visuals.gameObject.SetActive(false);
	}

	private void Hide()
	{
		visuals.gameObject.SetActive(false);
		visuals.anchoredPosition = canvasRt.sizeDelta * 0.5f;
	}

	private void LateUpdate()
	{
		if (!target || !target.gameObject.activeInHierarchy)
		{
			Hide();
		}
		else
		{
			var mp = AnchoredPositionFromWorld(target.position, Camera.main, canvasRt);
			if (mp.HasValue)
			{
				visuals.gameObject.SetActive(true);
				visuals.anchoredPosition = mp.Value;
			}
			else
			{
				Hide();
			}
		}
	}

	public static Vector2? AnchoredPositionFromWorld(Vector3 worldPoint, Camera camera, RectTransform canvasRt)
	{
		if (Vector3.Dot(worldPoint - camera.transform.position, camera.transform.forward) < 0)
		{
			return null;
		}

		Vector2 screenPos = camera.WorldToScreenPoint(worldPoint);

		screenPos.x /= Screen.width;
		screenPos.x *= canvasRt.sizeDelta.x;

		screenPos.y /= Screen.height;
		screenPos.y *= canvasRt.sizeDelta.y;

		return screenPos;
	}
}
