using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

[RequireComponent(typeof(V2_ButtonHandle))]
public class V2_SigilHandle : MonoBehaviour
{
	public V2_ButtonHandle button { get; private set; }
	public Sprite sigilPatternSprite;
	public VisualEffect visualEffect;
	public V2_SigilExplode explosionPrefab;

	private void Awake()
	{
		button = GetComponent<V2_ButtonHandle>();
		button.onClick += OnClick;
	}

	private void OnDestroy()
	{
		if (button)
		{
			button.onClick -= OnClick;
		}
	}

	private void OnClick(V2_ButtonHandle button, V2_HandleController controller)
	{
		FindObjectOfType<V2_SigilUI>().AddSigilSprite(sigilPatternSprite);
		ExplodeWithoutCollecting();
	}

	public void ExplodeWithoutCollecting()
	{
		var go = Instantiate(explosionPrefab.gameObject, transform.position, transform.rotation);
		visualEffect.Stop();
		visualEffect.transform.SetParent(go.transform);
		Destroy(gameObject);
	}
}
