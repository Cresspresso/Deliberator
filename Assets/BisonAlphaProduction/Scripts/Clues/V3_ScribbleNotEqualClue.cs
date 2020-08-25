using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class V3_SparData_ScribbleNotEqualClue
{
	public readonly int lhsVariableID;
	public readonly int rhsVariableID;

	public V3_SparData_ScribbleNotEqualClue(int lhsVariableID, int rhsVariableID)
	{
		this.lhsVariableID = lhsVariableID;
		this.rhsVariableID = rhsVariableID;
	}
}

public class V3_ScribbleNotEqualClue : V3_Randomizer<V3_SparData_ScribbleNotEqualClue, V3_SparDb_ScribbleNotEqualClue>
{
#pragma warning disable CS0649
	[SerializeField]
	private V3_ScribbleSequenceClueSet m_seqClueSet;
#pragma warning restore CS0649
	public V3_ScribbleSequenceClueSet seqClueSet => m_seqClueSet;



#pragma warning disable CS0649
	[SerializeField]
	private V3_TextureReplacer m_lhs;
#pragma warning restore CS0649
	public V3_TextureReplacer lhs => m_lhs;

#pragma warning disable CS0649
	[SerializeField]
	private V3_TextureReplacer m_rhs;
#pragma warning restore CS0649
	public V3_TextureReplacer rhs => m_rhs;



	protected override V3_SparData_ScribbleNotEqualClue Generate()
	{
		var availableVariableIDs = seqClueSet.generatedValue.variableIDs.ToList();

		int lhsVariableID = V2_Utility.ExtractRandomElement(availableVariableIDs);
		int rhsVariableID = V2_Utility.ExtractRandomElement(availableVariableIDs);
		Debug.Assert(availableVariableIDs.Count == 0, "Puzzle may not work with more than two variable clues.", this);

		return new V3_SparData_ScribbleNotEqualClue(lhsVariableID, rhsVariableID);
	}

	protected override bool PopulateOnAwake => false;

	protected override void Awake()
	{
		base.Awake();
		StartCoroutine(Co_Populate());
	}

	private IEnumerator Co_Populate()
	{
		yield return new WaitUntil(() => seqClueSet.isAlive);
		Populate();

		lhs.currentTextureIndex = generatedValue.lhsVariableID;
		rhs.currentTextureIndex = generatedValue.rhsVariableID;
	}
}
