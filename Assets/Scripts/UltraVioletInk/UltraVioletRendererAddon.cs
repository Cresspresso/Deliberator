using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

/// <summary>
/// Updates the properties of the UltraVioletUnlit shader before the frame is rendered.
/// </summary>
/// <author>Elijah Shadbolt</author>
public class UltraVioletRendererAddon : MonoBehaviour
{
	public UltraVioletLight uviLight;
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
			uviLight = FindObjectOfType<UltraVioletLight>();
		}

		if (uviLight)
		{
			var mat = rend.material;
			mat.SetVector("uviLightPosition", uviLight.transform.position);
			mat.SetVector("uviLightDirection", uviLight.transform.forward);
			mat.SetFloat("uviSpotAngle", uviLight.spotAngle);
			mat.SetFloat("uviInnerSpotAngleFraction", uviLight.innerSpotAnglePercent * 0.00999f);
		}
	}
}
