using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class V3_TextureReplacer : MonoBehaviour
{
	private Renderer m_theRenderer;
	public Renderer theRenderer {
		get
		{
			if (!m_theRenderer)
			{
				m_theRenderer = GetComponent<Renderer>();
				if (!m_theRenderer)
				{
					Debug.LogError("Renderer is null. Must have a renderer attached.", this);
				}
			}
			return m_theRenderer;
		}
	}



	[SerializeField]
	private Texture2D[] m_textures = new Texture2D[10];
	public IReadOnlyList<Texture2D> textures => m_textures;



	[SerializeField]
	private int m_currentTextureIndex = 0;
	public int currentTextureIndex {
		get => m_currentTextureIndex;
		set
		{
			m_currentTextureIndex = value;
			theRenderer.material.SetTexture("_BaseColorMap", m_textures[value]);
		}
	}



	private void Awake()
	{
		currentTextureIndex = currentTextureIndex;
	}
}
