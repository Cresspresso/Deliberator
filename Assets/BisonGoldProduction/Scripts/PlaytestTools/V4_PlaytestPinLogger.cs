using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
///		<para>
///			Attach to a <see cref="V2_NumPadLock"/>.
///		</para>
/// </summary>
/// 
/// <changelog>
///		<log author="Elijah Shadbolt" date="19/10/2020">
///			<para>Created this script.</para>
///		</log>
/// </changelog>
/// 
[RequireComponent(typeof(V2_NumPadLock))]
public class V4_PlaytestPinLogger : MonoBehaviour
{
	private void Start()
	{
		StartCoroutine(Co());

		IEnumerator Co()
		{
			yield return new WaitForSeconds(1.0f);

			var npl = GetComponent<V2_NumPadLock>();

			V4_PlaytestConsole.Log("NumPad{ name: '" + name
				+ "', position: " + transform.position
				+ ", PIN: " + npl.passcode
				+ " }");
		}
	}
}
