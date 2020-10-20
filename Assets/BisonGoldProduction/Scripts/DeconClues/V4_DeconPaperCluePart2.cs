using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
///		<para>
///			Make sure that the <see cref="V3_PaperClue.content"/> has format expressions `{0}` and `{1}` in it.
///		</para>
/// </summary>
/// 
/// <changelog>
///		<log author="Elijah Shadbolt" date="19/10/2020">
///			<para>Created this script.</para>
///		</log>
/// </changelog>
/// 
[RequireComponent(typeof(V3_PaperClue))]
public class V4_DeconPaperCluePart2 : MonoBehaviour
{
	public V3_PaperClue paperClue { get; private set; }

#pragma warning disable CS0649

	[SerializeField]
	private V4_DeconPaperClue m_firstPart;
	public V4_DeconPaperClue firstPart => m_firstPart;

#pragma warning restore CS0649

	private void Awake()
	{
		paperClue = GetComponent<V3_PaperClue>();

		StartCoroutine(Co());

		IEnumerator Co()
		{
			yield return new WaitUntil(() => firstPart.isAlive);

			var nthmap = new string[]
			{
				"first",
				"second",
				"third",
				"fourth",
			};
			var nth = nthmap[firstPart.unusedIndex];

			paperClue.content = string.Format(paperClue.content, firstPart.unusedDigit, nth);
		}
	}
}
