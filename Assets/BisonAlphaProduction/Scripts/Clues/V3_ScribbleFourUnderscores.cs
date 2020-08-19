using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class V3_ScribbleFourUnderscores : V3_Randomizer<int, V3_ScribbleFourUnderscoresSparDb>
{
	[SerializeField]
	private Texture2D[] m_textures = new Texture2D[1];
	public IReadOnlyList<Texture2D> textures => m_textures;

	protected override int Generate()
	{
		return Random.Range(0, textures.Count);
	}

	private void Start()
	{
		GetComponent<Renderer>().material.SetTexture("_BaseColorMap", textures[generatedValue]);
	}
}
