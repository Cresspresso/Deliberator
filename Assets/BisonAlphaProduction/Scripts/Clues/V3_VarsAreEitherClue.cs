using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class V3_SparData_VarsAreEitherClue
{
	public readonly int lhsVariableID;
	public readonly int rhsVariableID;

	public V3_SparData_VarsAreEitherClue(int lhsVariableID, int rhsVariableID)
	{
		this.lhsVariableID = lhsVariableID;
		this.rhsVariableID = rhsVariableID;
	}
}

public class V3_VarsAreEitherClue : V3_Randomizer<V3_SparData_VarsAreEitherClue, V3_SparDb_VarsAreEitherClue>
{
#pragma warning disable CS0649
	[SerializeField]
	private V3_ScribbleSequenceClueSet m_seqClueSet;
#pragma warning restore CS0649
	public V3_ScribbleSequenceClueSet seqClueSet => m_seqClueSet;



#pragma warning disable CS0649
	[SerializeField]
	private string[] m_variableNames = new string[3] { "X", "Z", "h" };
#pragma warning restore CS0649
	public IReadOnlyList<string> variableNames => m_variableNames;



	protected override V3_SparData_VarsAreEitherClue Generate()
	{
		var availableVariableIDs = seqClueSet.generatedValue.variableIDs.ToList();

		int lhsVariableID = V2_Utility.ExtractRandomElement(availableVariableIDs);
		int rhsVariableID = V2_Utility.ExtractRandomElement(availableVariableIDs);
		Debug.Assert(availableVariableIDs.Count == 0, "Puzzle may not work with more than two variable clues.", this);

		return new V3_SparData_VarsAreEitherClue(lhsVariableID, rhsVariableID);
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

		var map = seqClueSet.generatedValue.mapPasscodeDigitFromVariableID;
		int lhs = generatedValue.lhsVariableID;
		int rhs = generatedValue.rhsVariableID;
		int digit1 = map[lhs];
		int digit2 = map[rhs];
		string var1 = variableNames[lhs];
		string var2 = variableNames[rhs];
		string str = $"{var1} is either {digit1} or {digit2}\nand\n{var2} is either {digit1} or {digit2}";
		GetComponent<V3_PaperClue>().content = str;
	}
}
