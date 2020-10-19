using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
///		<para>
///			Attach to a <see cref="V3_VaultSafe"/>.
///		</para>
/// </summary>
/// 
/// <changelog>
///		<log author="Elijah Shadbolt" date="19/10/2020">
///			<para>Created this script.</para>
///		</log>
/// </changelog>
/// 
[RequireComponent(typeof(V3_VaultSafe))]
public class V4_PlaytestVaultLogger : MonoBehaviour
{
	private void Start()
	{
		StartCoroutine(Co());

		IEnumerator Co()
		{
			yield return new WaitForSeconds(1.0f);

			var vaultSafe = GetComponent<V3_VaultSafe>();
			var vsc = vaultSafe.combination;

			// aggregate vsc to a string.
			var it = vsc.GetEnumerator();
			var combination = "";
			if (it.MoveNext())
			{
				combination += it.Current.ToString().PadLeft(2, '0');
			}
			while (it.MoveNext())
			{
				combination += '-' + it.Current.ToString().PadLeft(2, '0');
			}

			// log the message.
			V4_PlaytestConsole.Log("VaultSafe{ name: '" + name
				+ "', position: " + transform.position
				+ ", combination: " + combination
				+ " }");
		}
	}
}
