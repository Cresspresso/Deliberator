using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
///		<para>
///			Make sure that the <see cref="V3_PaperClue.content"/> has a format expression `{0}` in it.
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
public class V4_DeconPaperClue : MonoBehaviour
{
	public V3_PaperClue paperClue { get; private set; }

#pragma warning disable CS0649

	[SerializeField]
	private V3_INumPadLockRandomizer m_nplr;
	public V3_INumPadLockRandomizer nplr => m_nplr;

	public int unusedIndex { get; private set; }
	public char unusedDigit { get; private set; }
	public bool isAlive { get; private set; } = false;

#pragma warning restore CS0649

	private void Awake()
	{
		paperClue = GetComponent<V3_PaperClue>();

		StartCoroutine(Co());

		IEnumerator Co()
		{
			yield return new WaitUntil(() => nplr.isAlive);
			var code = nplr.numpadLock.passcode;

			unusedIndex = Random.Range(0, code.Length);
			unusedDigit = code[unusedIndex];

			var seq = V2_Utility.ListFromRange(code.Length)
				.Select(i => i == unusedIndex ? "something" : code[i].ToString());
			var sub = string.Join(", ", seq);
			paperClue.content = string.Format(paperClue.content, sub);

			isAlive = true;
		}
	}
}
