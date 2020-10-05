using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <changelog>
///		<log author="Elijah Shadbolt" date="05/10/2020">
///			<para>Created this script.</para>
///		</log>
/// </changelog>
/// 
public class V4_SparDb_VaultSafe : V3_SparRandomizerDatabase<V4_SparData_VaultSafe>
{

}

/// <changelog>
///		<log author="Elijah Shadbolt" date="05/10/2020">
///			<para>Created this class.</para>
///		</log>
/// </changelog>
/// 
public class V4_SparData_VaultSafe
{
	public V4_SparData_VaultSafe(IReadOnlyList<int> combination)
	{
		this.combination = combination;
	}

	public IReadOnlyList<int> combination { get; private set; }
}
