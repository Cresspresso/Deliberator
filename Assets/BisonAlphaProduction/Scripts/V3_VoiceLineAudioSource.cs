using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <author>Elijah Shadbolt</author>
/// <stage>Alpha Production</stage>
[RequireComponent(typeof(AudioSource))]
public class V3_VoiceLineAudioSource : MonoBehaviour
{
	private V3_VoiceLineManager manager;
	public AudioSource audioSource { get; private set; }

	public void Play(AudioClip clip)
	{
		audioSource.Stop();
		audioSource.clip = clip;
		audioSource.Play();
	}

	public void Stop()
	{
		audioSource.Stop();
	}

	private void Start()
	{
		audioSource = GetComponent<AudioSource>();

		manager = V3_VoiceLineManager.instance;
		Debug.Assert(manager);
		manager.Register(this);
	}

	private void OnDestroy()
	{
		if (manager)
		{
			manager.Unregister(this);
		}
	}
}
