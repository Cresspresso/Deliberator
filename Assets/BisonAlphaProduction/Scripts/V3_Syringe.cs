using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.VFX;
using DG.Tweening;

/// <author>Elijah Shadbolt</author>
/// <stage>Alpha Production</stage>
[RequireComponent(typeof(V2_ButtonHandle))]
public class V3_Syringe : MonoBehaviour
{
	private V2_ButtonHandle buttonHandle;
	public string nextSceneName = "";
	public float delay = 3.0f;
	public Transform effectsRoot;
	public float shrinkDuration = 0.5f;
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
		Invoke(nameof(LoadScene), delay);
		buttonHandle.handle.enabled = false;

		var gc = FindObjectOfType<V2_GroundhogControl>();
		if (gc)
		{
			gc.enabled = false;
		}

		transform.DOScale(0, shrinkDuration).SetEase(Ease.InCirc);

		var armManager = FindObjectOfType<V3_Arm_Manager>();
		if (armManager)
		{
			armManager.TriggerInject();
		}

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
