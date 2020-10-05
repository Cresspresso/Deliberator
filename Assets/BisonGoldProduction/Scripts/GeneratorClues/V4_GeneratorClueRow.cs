using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///		<para>A clue for the combination of fuses to activate a <see cref="V3_Generator"/>.</para>
/// </summary>
/// 
/// <changelog>
///		<log author="Elijah Shadbolt" date="05/10/2020">
///			<para>Created this script.</para>
///		</log>
/// </changelog>
/// 
public class V4_GeneratorClueRow : MonoBehaviour
{
#pragma warning disable CS0649
	public IReadOnlyList<Renderer> renderers => m_renderers;
	[SerializeField]
	private List<Renderer> m_renderers = new List<Renderer>();

	[SerializeField]
	private Material m_materialOff;
	public Material materialOff => m_materialOff;

	[SerializeField]
	private Material m_materialOn;
	public Material materialOn => m_materialOn;

	public V3_Generator generator { get => m_generator; set => m_generator = value; }
	[SerializeField]
	private V3_Generator m_generator;

#pragma warning restore CS0649

	private void Start()
	{
		StartCoroutine(Co());
		IEnumerator Co()
		{
			yield return new WaitUntil(() => generator.isAlive);
			for (var i = 0; i < renderers.Count; ++i)
			{
				renderers[i].material = generator.IsFuseRequiredToBeOn(i) ? materialOn : materialOff;
			}
		}
	}
}
