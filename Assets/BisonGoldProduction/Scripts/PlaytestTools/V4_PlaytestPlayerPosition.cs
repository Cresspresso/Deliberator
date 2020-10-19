using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

/// <changelog>
///		<log author="Elijah Shadbolt" date="19/10/2020">
///			<para>Created this script.</para>
///		</log>
/// </changelog>
/// 
public class V4_PlaytestPlayerPosition : MonoBehaviour
{
#pragma warning disable CS0649

	[SerializeField]
	private Text m_textElement;
	public Text textElement => m_textElement;

#pragma warning restore CS0649

	private void LateUpdate()
	{
		var pos = V2_FirstPersonCharacterController.instance.position;
		textElement.text = "Player Position: " + FormatVector3(pos);
	}

	public static string FormatVector3(Vector3 vector)
		=> string.Format("x={0:0.000}, y={1:0.000}, z={2:0.000}", vector.x, vector.y, vector.z);
}
