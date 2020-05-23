using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

[RequireComponent(typeof(ButtonHandle))]
public class SigilHandle : MonoBehaviour
{
	public ButtonHandle button { get; private set; }
	public VisualEffect visualEffect;
	public SigilExplode explosionPrefab;

	private void Awake()
	{
		button = GetComponent<ButtonHandle>();
		button.onClick += OnClick;
	}

	private void OnDestroy()
	{
		if (button)
		{
			button.onClick -= OnClick;
		}
	}

	private void OnClick(ButtonHandle button, HandleController controller)
	{
		var go = Instantiate(explosionPrefab.gameObject, transform.position, transform.rotation);
		visualEffect.Stop();
		visualEffect.transform.SetParent(go.transform);
		Destroy(gameObject);
	}
}
