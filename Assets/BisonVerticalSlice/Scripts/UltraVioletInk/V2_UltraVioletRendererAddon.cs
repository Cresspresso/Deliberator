using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

/// <summary>
/// Updates the properties of the UltraVioletUnlit shader before the frame is rendered.
/// </summary>
/// <author>Elijah Shadbolt</author>
public class V2_UltraVioletRendererAddon : MonoBehaviour
{
	public static V2_UltraVioletLight uviLight;
	private Renderer m_renderer;
	public Renderer rend {
		get
		{
			if (!m_renderer) { m_renderer = GetComponent<Renderer>(); }
			return m_renderer;
		}
	}

	private int subscriptions = 0;

	private void OnEnable()
	{
		Subscribe();
	}

	private void OnDisable()
	{
		Unsubscribe();
	}

	public void Subscribe()
	{
		if (subscriptions == 0)
		{
			RenderPipelineManager.beginFrameRendering += OnBeginFrameRendering;
		}
		++subscriptions;
	}

	public void Unsubscribe()
	{
		--subscriptions;
		if (subscriptions == 0)
		{
			RenderPipelineManager.beginFrameRendering -= OnBeginFrameRendering;
		}
	}

	private void OnBeginFrameRendering(ScriptableRenderContext context, Camera[] cameras)
	{
		if (!uviLight)
		{
			uviLight = FindObjectOfType<V2_UltraVioletLight>();
		}

#if UNITY_EDITOR
		bool has = false;
#else
		bool has = (bool)uviLight;
#endif
		var mat = rend.material;
		mat.SetVector("uviLightPosition", has ? uviLight.transform.position : transform.TransformPoint(-1.0f * Vector3.forward));
		mat.SetVector("uviLightDirection", has ? uviLight.transform.forward : transform.forward);
		mat.SetFloat("uviSpotAngle", has ? uviLight.spotAngle : 30.0f);
		mat.SetFloat("uviInnerSpotAngleFraction", has ? uviLight.innerSpotAnglePercent * 0.00999f : 0.0f);
		mat.SetFloat("uviRange", has ? uviLight.range : 0.0f);
	}
}
