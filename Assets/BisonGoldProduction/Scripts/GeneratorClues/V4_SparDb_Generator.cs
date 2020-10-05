using System.Collections.Generic;

/// <summary>
///		<para>See also:</para>
///		<para><see cref="V3_GenPuzzleSolved"/></para>
/// </summary>
/// <changelog>
///		<log author="Elijah Shadbolt" date="05/10/2020">
///			<para>Created this script.</para>
///		</log>
/// </changelog>
/// 
public class V4_SparDb_GeneratorManager : V3_SparRandomizerDatabase<V4_SparData_GeneratorManager>
{
}

/// <changelog>
///		<log author="Elijah Shadbolt" date="05/10/2020">
///			<para>Created this class.</para>
///		</log>
/// </changelog>
/// 
public class V4_SparData_GeneratorManager
{
	/// <summary>
	/// Index correlates to index into <see cref="V3_GenPuzzleSolved.generators"/>.
	/// </summary>
	public IReadOnlyList<V4_SparData_Generator> map { get; private set; }

	public V4_SparData_GeneratorManager(IReadOnlyList<V4_SparData_Generator> map)
	{
		this.map = map;
	}
}

/// <changelog>
///		<log author="Elijah Shadbolt" date="05/10/2020">
///			<para>Created this class.</para>
///		</log>
/// </changelog>
/// 
public class V4_SparData_Generator
{
	public byte combination { get; private set; }

	public V4_SparData_Generator(byte combination)
	{
		this.combination = combination;
	}
}
