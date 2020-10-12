using Bison.BoolExpressions.Serializable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///		<para>
///			Like a latch in electronics, with a SET input and RESET input, either HIGH (isPowered == true) or LOW (isPowered == false).
///		</para>
///		<para>
///			Outputs HIGH if SET is HIGH, then stays HIGH until RESET is HIGH.
///		</para>
///		<para>
///			Outputs LOW if RESET is HIGH, then stays LOW until SET is HIGH.
///		</para>
///		<para>
///			SETUP: Change the Dependable Condition Root to Literal false.
///		</para>
/// </summary>
/// 
///	<changelog>
///		<log author="Elijah Shadbolt" date="12/10/2020">
///			<para>
///				Created this script.
///			</para>
///		</log>
///	</changelog>
///	
[RequireComponent(typeof(Dependable))]
public class V4_DependLatch : MonoBehaviour
{
	private Dependable m_dependable;
	public Dependable dependable {
		get
		{
			PrepareDependable();
			return m_dependable;
		}
	}
	private void PrepareDependable()
	{
		if (!m_dependable)
		{
			m_dependable = GetComponent<Dependable>();
		}
	}

	[SerializeField]
	private Dependable m_inputSet;
	public Dependable inputSet => m_inputSet;

	[Tooltip("Optional")]
	[SerializeField]
	private Dependable m_inputReset = null;
	public Dependable inputReset => m_inputReset;

	private void Awake()
	{
		PrepareDependable();

		if (inputSet)
		{ 
			inputSet.onChanged.AddListener(OnSetChanged);
		}
		else
		{
			Debug.LogError("inputSet is required but it was null", this);
		}

		if (inputReset)
		{
			inputReset.onChanged.AddListener(OnResetChanged);
		}
	}

	private void OnSetChanged(bool isPowered)
	{
		if (isPowered)
		{
			V2_Utility.TryElseLog(() =>
			{
				dependable.firstLiteral = true;
			});
		}
	}

	private void OnResetChanged(bool isPowered)
	{
		if (isPowered && !inputSet.isPowered)
		{
			V2_Utility.TryElseLog(() =>
			{
				dependable.firstLiteral = false;
			});
		}
	}
}
