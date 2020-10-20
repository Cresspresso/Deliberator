using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if false
/// <summary>
///		<para>Receives animation events from an attached <see cref="Animator"/>.</para>
/// </summary>
/// 
/// <changelog>
///		<log author="Elijah Shadbolt" date="28/09/2020">
///			<para>Added comments.</para>
///			<para>Suppressed warning CS0649</para>
///		</log>
///		<log author="Elijah Shadbolt" date="21/10/2020">
///			<para>Deprecated this script.</para>
///		</log>
/// </changelog>
/// 
[System.Obsolete("Replaced by V4_PlayerAnimator")]
public class V3_PlayerDeathAnimEventHandler : MonoBehaviour
{
#pragma warning disable CS0649

	[SerializeField]
	private V3_PlayerDeath m_manager;

#pragma warning restore CS0649

	/// <summary>
	/// An animation event.
	/// </summary>
	public void OnDone()
	{
		m_manager.OnDone();
	}
}
#endif