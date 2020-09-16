using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Serialization;

/// <summary>
///		<para>Controls the stamina of the player, and how to restart the day.</para>
/// </summary>
/// 
/// <changelog>
///		<log author="Elijah Shadbolt" date="17/09/2020">
///			<para>Replaced the old time limit system with a stamina system.</para>
///		</log>
/// </changelog>
public class V2_GroundhogControl : MonoBehaviour
{
	public V2_FirstPersonCharacterController fpcc => crouch.fpcc;
	public V3_Crouch crouch { get; private set; }

#pragma warning disable CS0649
	[Tooltip("Stamina decreased per metre moved")]
	[SerializeField]
	private float m_drainAmount = 1;
#pragma warning restore CS0649
	public float drainAmount => m_drainAmount;





	public Animator flashAnim;
	public AudioMixer audioMixer;

	public bool isPaused { get; set; } = false;

#pragma warning disable CS0649
	[FormerlySerializedAs("m_remainingDuration")]
	[SerializeField]
	public float stamina = 100.0f;
#pragma warning restore CS0649

	public event Action<float> StaminaChanged;

	public bool hasFinished { get; private set; } = false;
	public event Action Finished;

	private void Awake()
	{
		crouch = FindObjectOfType<V3_Crouch>();
	}

	void Start()
	{
		flashAnim = GameObject.FindGameObjectWithTag("RedFlash").GetComponent<Animator>();
		audioMixer.SetFloat("MasterFreqGain", 1.0f);
	}

	private void DrainStamina()
	{
		var dis = fpcc.displacementThisFrame;
		var v = new Vector2(dis.x, dis.z);
		float distanceMoved = v.magnitude;
		if (distanceMoved > 0.001f)
		{
			stamina -= drainAmount * distanceMoved;
		}
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.R))
		{
			Die();
		}

		if (Input.GetKeyDown(KeyCode.K))
		{
			isPaused = !isPaused;
		}

		if (!isPaused)
		{
			DrainStamina();
		}

		stamina = Mathf.Max(stamina, 0.0f);
		StaminaChanged?.Invoke(stamina);
		if (stamina == 0)
		{
			Die();
		}
	}

	private IEnumerator Co_PlayerDied()
	{
		flashAnim.SetTrigger("TriggerRed");

		gameObject.GetComponent<AudioSource>().Play();

		audioMixer.SetFloat("MasterCenterFreq", 7500.0f);
		audioMixer.SetFloat("MasterOctaveRange", 5.0f);
		audioMixer.SetFloat("MasterFreqGain", 0.05f);

		yield return new WaitForSeconds(1.5f);

		V3_SparGameObject.RestartCurrentScene();
	}

	public void Die()
	{
		if (hasFinished)
		{
			return;
		}

		hasFinished = true;

		StartCoroutine(Co_PlayerDied());
	}
}
