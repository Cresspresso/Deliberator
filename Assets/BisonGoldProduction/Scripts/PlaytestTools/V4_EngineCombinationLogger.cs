using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
///		<para>
///			Attach to a <see cref="V3_Generator"/>.
///		</para>
/// </summary>
/// 
/// <changelog>
///		<log author="Elijah Shadbolt" date="19/10/2020">
///			<para>Created this script.</para>
///		</log>
/// </changelog>
/// 
[RequireComponent(typeof(V3_Generator))]
public class V4_EngineCombinationLogger : MonoBehaviour
{
	private void Start()
	{
		StartCoroutine(Co());

		IEnumerator Co()
		{
			yield return new WaitForSeconds(1.0f);

			var gen = GetComponent<V3_Generator>();

			var combination = System.Convert.ToString(gen.generatedValue.combination, 2).PadLeft(7, '0');

			V4_PlaytestConsole.Log("Generator{ name: '" + name
				+ "', position: " + transform.position
				+ ", combination: " + combination
				+ " }");
		}
	}
}
