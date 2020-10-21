using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
///		<para>See also, the "Engine1" prefab.</para>
///		<para>The <see cref="Dependable.condition"/> should be an AND group of V3_GenFuseSlot Dependencies.</para>
///		<para>Each element of the AND group must either be set directly to the Dependency or NOT the Dependency.</para>
/// </summary>
/// 
/// <changelog>
///		<log author="Elijah Shadbolt" date="25/08/2020">
///			<para>Added this script.</para>
///		</log>
///		<log author="Elijah Shadbolt" date="05/10/2020">
///			<para>Added randomly generated combination.</para>
///		</log>
///		<log author="Elijah Shadbolt" date="21/10/2020">
///			<para>Made it only disable meddling after combination is generated.</para>
///		</log>
/// </changelog>
/// 
[RequireComponent(typeof(Dependable))]
public class V3_Generator : MonoBehaviour
{
	public V3_GenPuzzleSolved manager { get; private set; }
	public int ID { get; private set; }

	private Dependable m_dependable;
	public Dependable dependable {
		get
		{
			PrepareDependable();
			return m_dependable;
		}
	}

	private IReadOnlyList<V3_GenFuseSlot> m_slots;
	public IReadOnlyList<V3_GenFuseSlot> slots {
		get
		{
			PrepareDependable();
			return m_slots;
		}
	}

	private void PrepareDependable()
	{
		if (!m_dependable)
		{
			m_dependable = GetComponent<Dependable>();
			m_dependable.onChanged.AddListener(OnPoweredChanged);

			m_slots = m_dependable.GetDependencyComponents<V3_GenFuseSlot>();
		}
	}

#pragma warning disable CS0649

	public V3_KeyCardReader_Sprites lockSprites => m_lockSprites;
	[SerializeField]
	private V3_KeyCardReader_Sprites m_lockSprites;


	public int numFusesRequired => m_numFusesRequired;
	[SerializeField]
	private int m_numFusesRequired = 4;

#pragma warning restore CS0649

	public const int NumSlots = 7;
	public V4_SparData_Generator generatedValue { get; private set; }
	public bool isAlive { get; private set; }

	public bool IsFuseRequiredToBeOn(int i)
		=> (generatedValue.combination & (1 << i)) == (1 << i);

	public void Init(V3_GenPuzzleSolved manager, int ID)
	{
		this.manager = manager;
		this.ID = ID;
		this.generatedValue = manager.generatedValue.map[ID];


		// set up the dependency NOT operators.
		Debug.Assert(dependable.condition.root.type == Bison.BoolExpressions.Serializable.ExpressionType.Group);
		var groupCopy = dependable.condition.arrays.groupArray[dependable.condition.root.index];
		Debug.Assert(groupCopy.operandSequence.Count == NumSlots);
		Debug.Assert(groupCopy.type == Bison.BoolExpressions.Serializable.GroupType.And);
		for (var i = 0; i < NumSlots; ++i)
		{
			var keyCopy = groupCopy.operandSequence[i];
			keyCopy.type = IsFuseRequiredToBeOn(i)
				? Bison.BoolExpressions.Serializable.ExpressionType.Dependency
				: Bison.BoolExpressions.Serializable.ExpressionType.Not;
			groupCopy.operandSequence[i] = keyCopy;
		}
		dependable.condition.arrays.groupArray[dependable.condition.root.index] = groupCopy;


		this.isAlive = true;

		OnPoweredChanged(dependable.isPowered);
	}



	private void Awake()
	{
		PrepareDependable();
	}

	private void OnPoweredChanged(bool isPowered)
	{
		if (!isAlive) return;

		if (isPowered)
		{
			lockSprites.ShowUnlockedImage();

			DisableMeddling();
		}
		else
		{
			lockSprites.ShowLockedImage();
		}
	}

	/// Prevents the player from changing this generator's fuses anymore.
	public void DisableMeddling()
	{
		foreach (var slot in slots)
		{
			slot.DisableMeddling();
		}
	}
}
