using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///		<para>Maps generator identifiers (<see langword="int"/> keys) to generated data (<typeparamref name="TValue"/> values).</para>
///		<para>
///			This abstract class is used in conjunction with <see cref="V3_Randomizer{TValue, TSparRandomizerDatabase}"/>,
///			to preserve data across the game's time-loop scene restart.
///		</para>
///		<para>Make a concrete script that inherits from this abstract class (a non-abstract, non-generic script that provides all type arguments).</para>
///		<para>The singleton instance of the derived script can be retrieved with <see cref="V3_SparGameObject.FindOrCreateComponent{T}"/>.</para>
///		<para>See also:</para>
///		<para><see cref="V3_SparGameObject"/></para>
///		<para><see cref="V3_Randomizer{TValue, TSparRandomizerDatabase}"/></para>
///		<para><see cref="V3_SparFingerprintRandomizerDatabase"/></para>
/// </summary>
/// <typeparam name="TValue">An element in the dictionary which can be retrieved by an <see langword="int"/> key.</typeparam>
/// 
/// <changelog>
///		<log author="Elijah Shadbolt" date="11/08/2020">
///			<para>Added comments.</para>
///		</log>
/// </changelog>
/// 
public abstract class V3_SparRandomizerDatabase<TValue> : MonoBehaviour
{
	/// <summary>
	///		<para>Maps generator identifiers (<see langword="int"/> keys) to generated data (<typeparamref name="TValue"/> values).</para>
	///		<para>See also:</para>
	///		<para><see cref="V3_Randomizer{TValue, TSparRandomizerDatabase}.generatorID"/></para>
	///		<para><see cref="V3_Randomizer{TValue, TSparRandomizerDatabase}.Generate"/></para>
	/// </summary>
	/// 
	/// <changelog>
	///		<log author="Elijah Shadbolt" date="11/08/2020">
	///			<para>Added comments.</para>
	///		</log>
	/// </changelog>
	/// 
	public Dictionary<int, TValue> dictionary { get; private set; } = new Dictionary<int, TValue>();
}
