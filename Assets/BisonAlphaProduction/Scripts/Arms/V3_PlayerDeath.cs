using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class V3_PlayerDeath : MonoBehaviour
{
	[SerializeField]
	private GameObject m_visuals;
	public GameObject visuals => m_visuals;

	[SerializeField]
	private GameObject m_mainCameraGameObject;
	public GameObject mainCameraGameObject => m_mainCameraGameObject;

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

	public void PlayAnimation()
	{
		mainCameraGameObject.SetActive(false);
		visuals.SetActive(true);
	}
}
