using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

/// <author>Elijah Shadbolt</author>
public class V2_GroundhogControl : MonoBehaviour
{
	public Animator flashAnim;
	public AudioMixer audioMixer;

	[SerializeField]
	private float m_remainingDuration = 10.0f;

	public float remainingDuration {
		get => m_remainingDuration;
		set
		{
			m_remainingDuration = Mathf.Max(value, 0.0f);
			RemainingDurationChanged?.Invoke(m_remainingDuration);
		}
	}
	public event Action<float> RemainingDurationChanged;

	public bool hasFinished { get; private set; } = false;
	public event Action Finished;

	void Start()
	{
		flashAnim = GameObject.FindGameObjectWithTag("RedFlash").GetComponent<Animator>();
		audioMixer.SetFloat("MasterFreqGain", 1.0f);
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.R))
		{
			StartCoroutine(PlayerDied());
			//Finish();
		}
		else
		{
			remainingDuration -= Time.deltaTime;
			if (remainingDuration <= 0.0f)
			{
				StartCoroutine(PlayerDied());
				//Finish();
			}
		}
	}

	private void Finish()
	{
		hasFinished = true;

		try
		{
			Finished?.Invoke();
		}
		catch (Exception e)
		{
			Debug.LogException(e);
		}

		var cap = GetComponent<V2_RewindCapture>();
		if (cap)
		{
			cap.PresentRewindCutscene();
		}
		else
		{
			Debug.LogError("RewindCapture not found", this);
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		}
	}

	public IEnumerator PlayerDied()
	{
		flashAnim.SetTrigger("TriggerRed");

		gameObject.GetComponent<AudioSource>().Play();

		audioMixer.SetFloat("MasterCenterFreq", 7500.0f);
		audioMixer.SetFloat("MasterOctaveRange", 5.0f);
		audioMixer.SetFloat("MasterFreqGain", 0.05f);

		yield return new WaitForSeconds(2.0f);
		Finish();
	}
}
