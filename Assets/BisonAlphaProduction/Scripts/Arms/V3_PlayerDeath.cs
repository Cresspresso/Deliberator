using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if false
/// <summary>
///		<para>Manages the animation of the player character fainting.</para>
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
[System.Obsolete("Replaced by V4_PlayerAnimator.")]
public class V3_PlayerDeath : MonoBehaviour
{
#pragma warning disable CS0649

	[SerializeField]
	private GameObject m_visuals;
	public GameObject visuals => m_visuals;

	[SerializeField]
	private GameObject m_mainCameraGameObject;
	public GameObject mainCameraGameObject => m_mainCameraGameObject;

#pragma warning restore CS0649

	public bool isDone { get; private set; } = false;

	public static V3_PlayerDeath instance => V2_Singleton<V3_PlayerDeath>.instance;

	private void Awake()
	{
		V2_Singleton<V3_PlayerDeath>.OnAwake(this, V2_SingletonDuplicateMode.Ignore);
	}

	/// <summary>
	///		<para>Called by <see cref="V3_PlayerDeathAnimEventHandler.OnDone"/>.</para>
	/// </summary>
	public void OnDone()
	{
		isDone = true;
	}

	/// <summary>
	///		<para>Called by <see cref="V2_GroundhogControl"/>.</para>
	/// </summary>
	public void PlayAnimation()
	{
		mainCameraGameObject.SetActive(false);
		visuals.SetActive(true);
	}
}
#endif