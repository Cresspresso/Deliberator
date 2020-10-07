using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(V3_PaperClue))]
public class V4_VaultCombinationClue : MonoBehaviour
{
	public V3_PaperClue paperClue { get; private set; }

	public V3_VaultSafe safe => m_safe;
#pragma warning disable CS0649
	[SerializeField]
	private V3_VaultSafe m_safe;
#pragma warning restore CS0649

	private void Awake()
	{
		paperClue = GetComponent<V3_PaperClue>();
	}

	private void Start()
	{
		paperClue.content = string.Format(paperClue.content, string.Join("-", safe.combination));
	}
}
