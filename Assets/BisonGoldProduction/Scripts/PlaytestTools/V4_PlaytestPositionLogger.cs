using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
///		<para>
///			Attach to a <see cref="Transform"/>.
///		</para>
/// </summary>
/// 
/// <changelog>
///		<log author="Elijah Shadbolt" date="19/10/2020">
///			<para>Created this script.</para>
///		</log>
/// </changelog>
/// 
public class V4_PlaytestPositionLogger : MonoBehaviour
{
	private void Start()
	{
		StartCoroutine(Co());

		IEnumerator Co()
		{
			yield return new WaitForSeconds(1.0f);
			V4_PlaytestConsole.Log("GameObject{ name: '" + name
				+ "', position: " + transform.position
				+ " }");
		}
	}
}
