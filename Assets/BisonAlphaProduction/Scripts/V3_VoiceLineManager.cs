using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <author>Elijah Shadbolt</author>
/// <stage>Alpha Production</stage>
[Serializable]
public class VoiceLine
{
	public AudioClip clip;
	public float clipDurationIfNull = 0.0f;
	public float paddingDuration = 0.1f;

	[Tooltip("Closed Caption")]
	public string subtitle = "<Missing Subtitle>";
}

/// <author>Elijah Shadbolt</author>
/// <stage>Alpha Production</stage>
public class V3_VoiceLineManager : MonoBehaviour
{
	public static V3_VoiceLineManager instance { get; private set; }


	//public int GetMyIntVariable() { return m_var; }
	//private void SetMyIntVariable(int var) { m_var = var; }

	//private int m_var;
	//public int myIntVar {
	//	get { return m_var; }
	//	set { m_var = value; }
	//}

	private void Awake()
	{
		instance = this;
	}

	[SerializeField] // show in inspector, but cannot set from other scripts
	private HashSet<V3_VoiceLineAudioSource> m_vlas = new HashSet<V3_VoiceLineAudioSource>();

	public void Register(V3_VoiceLineAudioSource vla)
	{
		m_vlas.Add(vla);
	}

	public void Unregister(V3_VoiceLineAudioSource vla)
	{
		m_vlas.Remove(vla);
	}

	// `action` is a first class function, i.e. a variable which can be called like a function.
	private void ForEachVoiceLineAudioSource(System.Action<V3_VoiceLineAudioSource> action)
	{
		foreach (var vla in m_vlas)
		{
			if ((bool)vla) // bool cast returns false if vla was destroyed
			{
				action(vla);
			}
		}
	}

	public bool isPlaying { get; private set; }
	public bool isPaused { get; private set; }

	private Queue<VoiceLine> m_voiceLineQueue = new Queue<VoiceLine>();
	private VoiceLine m_currentVoiceLine = null;
	private float m_sectionTimeRemaining = 0.0f;

	private void Update()
	{
		if (isPlaying && !isPaused)
		{
			m_sectionTimeRemaining -= Time.deltaTime;
			if (m_sectionTimeRemaining <= 0.0f)
			{
				m_sectionTimeRemaining = 0.0f;
				PopAndPlayVoiceLine();
			}
		}
	}

	private void PopAndPlayVoiceLine()
	{
		if (m_voiceLineQueue.Any())
		{
			m_currentVoiceLine = m_voiceLineQueue.Dequeue();
			if (m_currentVoiceLine == null)
			{
				Debug.Log("Empty voice line discarded.", this);
				PopAndPlayVoiceLine();
			}
			else
			{
				var clip = m_currentVoiceLine.clip;
				if (clip)
				{
					m_sectionTimeRemaining = clip.length + m_currentVoiceLine.paddingDuration;
					Debug.Log("Playing voice line clip. length = " + m_sectionTimeRemaining, this);
					ForEachVoiceLineAudioSource(s => s.Play(clip));
				}
				else
				{
					m_sectionTimeRemaining = m_currentVoiceLine.clipDurationIfNull + m_currentVoiceLine.paddingDuration;
					Debug.Log("Playing voice line without clip. length = " + m_sectionTimeRemaining, this);
					ForEachVoiceLineAudioSource(s => s.Stop());
				}
			}
		}
		else
		{
			Stop();
		}
	}

	public void Stop()
	{
		Debug.Log("Stopped playing voice lines.", this);
		isPlaying = false;
		m_sectionTimeRemaining = 0.0f;
		m_currentVoiceLine = null;
		m_voiceLineQueue.Clear();
		ForEachVoiceLineAudioSource(s => s.Stop()); // The expression `s => s.Stop()` is a lambda function (first class function).
	}

	private void TryStartPlaying()
	{
		if (!isPlaying)
		{
			isPlaying = true;
			m_sectionTimeRemaining = 0.0f;
			Debug.Log("Playing " + m_voiceLineQueue.Count + " voice lines...", this);
			if (!isPaused)
			{
				PopAndPlayVoiceLine();
			}
		}
	}

	// add sequence of voice lines to back of queue.
	public void EnqueueSequence(IEnumerable<VoiceLine> voiceLines)
	{
		if (voiceLines != null)
		{
			foreach (var voiceLine in voiceLines)
			{
				m_voiceLineQueue.Enqueue(voiceLine);
			}
		}
		TryStartPlaying();
	}

	// replaces queue with new sequence of voice lines.
	public void ReplaceWithSequence(IEnumerable<VoiceLine> voiceLines)
	{
		Stop();
		if (voiceLines != null)
		{
			foreach (var voiceLine in voiceLines)
			{
				m_voiceLineQueue.Enqueue(voiceLine);
			}
			TryStartPlaying();
		}
	}
}
