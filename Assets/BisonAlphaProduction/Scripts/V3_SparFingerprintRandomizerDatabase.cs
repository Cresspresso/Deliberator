using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///		<para>Map of <see langword="int"/> keys to <see langword="string"/> values.</para>
///		<para>Each <see langword="int"/> key corresponds to the generator ID of a <see cref="V3_FingerprintNumpadRandomizer"/> instance.</para>
///		<para>
///			Each generated value is created by <see cref="V3_FingerprintNumpadRandomizer.Generate"/>,
///			and is used to set <see cref="V2_NumPadLock.passcode"/>.
///		</para>
///		<para>See also:</para>
///		<para><see cref="V3_FingerprintNumpadRandomizer.Generate"/></para>
///		<para><see cref="V3_SparRandomizerDatabase{TValue}"/></para>
///		<para><see cref="V3_SparGameObject"/></para>
/// </summary>
/// 
/// <changelog>
///		<log author="Elijah Shadbolt" date="11/08/2020">
///			<para>Added comments.</para>
///		</log>
/// </changelog>
/// 
public class V3_SparFingerprintRandomizerDatabase : V3_SparRandomizerDatabase<string>
{
}
