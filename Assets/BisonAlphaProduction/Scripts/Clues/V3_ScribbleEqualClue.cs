using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class V3_SparData_ScribbleEqualClue
{
	public readonly int variableID;

	public V3_SparData_ScribbleEqualClue(int variableID)
	{
		this.variableID = variableID;
	}
}

public class V3_ScribbleEqualClue : V3_Randomizer<V3_SparData_ScribbleEqualClue, V3_SparDb_ScribbleEqualClue>
{
#pragma warning disable CS0649
	[SerializeField]
	private V3_ScribbleSequenceClueSet m_seqClueSet;
#pragma warning restore CS0649
	public V3_ScribbleSequenceClueSet seqClueSet => m_seqClueSet;

#pragma warning disable CS0649
	[SerializeField]
	private V3_TextureReplacer m_variable;
#pragma warning restore CS0649
	public V3_TextureReplacer variable => m_variable;

#pragma warning disable CS0649
	[SerializeField]
	private V3_TextureReplacer m_digit;
#pragma warning restore CS0649
	public V3_TextureReplacer digit => m_digit;



	protected override V3_SparData_ScribbleEqualClue Generate()
	{
		var availableVariableIDs = seqClueSet.generatedValue.variableIDs.ToList();
		
		return new V3_SparData_ScribbleEqualClue(
			variableID: V2_Utility.ExtractRandomElement(availableVariableIDs) 
		);
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

		variable.currentTextureIndex = generatedValue.variableID;

		var map = seqClueSet.generatedValue.mapPasscodeDigitFromVariableID;
		digit.currentTextureIndex = map[generatedValue.variableID];
	}
}
