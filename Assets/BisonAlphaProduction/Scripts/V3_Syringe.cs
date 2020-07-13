using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.VFX;
using DG.Tweening;

[RequireComponent(typeof(V2_ButtonHandle))]
public class V3_Syringe : MonoBehaviour
{
	private V2_ButtonHandle buttonHandle;
	public string nextSceneName = "";
	public float delay = 3.0f;
	public Transform effectsRoot;
	public AudioSource celebrationAudio;
	public VisualEffect vfx;

	private void Awake()
	{
		buttonHandle = GetComponent<V2_ButtonHandle>();
		buttonHandle.onClick += OnClick;

		Debug.Assert(!string.IsNullOrWhiteSpace(nextSceneName), this);

		effectsRoot.gameObject.SetActive(false);
	}

	private void OnClick(V2_ButtonHandle buttonHandle, V2_HandleController handleController)
	{
		if (effectsRoot)
		{
			effectsRoot.SetParent(null);
			effectsRoot.rotation = Quaternion.identity;
			effectsRoot.gameObject.SetActive(true);
		}

		if (celebrationAudio)
		{
			celebrationAudio.Play();
		}

		if (vfx)
		{
			vfx.Play();
		}

		buttonHandle.handle.enabled = false;

		transform.DOScale(0, delay * 0.5f).SetEase(Ease.InCirc);

		Invoke(nameof(LoadScene), delay);
	}

	private void OnDestroy()
	{
		if (buttonHandle)
		{
			buttonHandle.onClick -= OnClick;
		}
	}

	private void LoadScene()
	{
		SceneManager.LoadScene(nextSceneName);
	}
}
