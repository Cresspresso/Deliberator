using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
///		<para>Manages how to play a sequence of voice lines.</para>
///		<para>
///			Singleton script within each scene. 
///			Attach to the "GroundhogControl" <see cref="GameObject"/>.
///			The instance can be retrieved with <see cref="instance"/>.
///		</para>
///		<para>See also:</para>
///		<para><see cref="V3_VoiceLineTrigger"/></para>
///		<para><see cref="V3_VoiceLineAudioSource"/></para>
///		<para><see cref="VoiceLine"/></para>
/// </summary>
/// 
/// <changelog>
///		<log author="Elijah Shadbolt" date="10/08/2020">
///			<para>Added comments.</para>
///		</log>
/// </changelog>
/// 
public class V3_VoiceLineManager : MonoBehaviour
{
	/// <summary>
	///		<para>The currently active instance of the singleton script <see cref="V3_VoiceLineManager"/> in the scene.</para>
	///		<para>This property is populated in the <see cref="Awake"/> event.</para>
	/// </summary>
	/// 
	/// <changelog>
	///		<log author="Elijah Shadbolt" date="10/08/2020">
	///			<para>Added comments.</para>
	///		</log>
	/// </changelog>
	/// 
	public static V3_VoiceLineManager instance { get; private set; }



	/// <summary>
	///		<para>Unity Message Method: <a href="https://docs.unity3d.com/ScriptReference/MonoBehaviour.Awake.html"/></para>
	/// </summary>
	/// 
	/// <changelog>
	///		<log author="Elijah Shadbolt" date="10/08/2020">
	///			<para>Added comments.</para>
	///		</log>
	/// </changelog>
	/// 
	private void Awake()
	{
		/// State that this current instance is the singleton instance in the scene.
		instance = this;
	}



	/// <summary>
	///		<para>The set of all <see cref="V3_VoiceLineAudioSource"/> instances in the scene.</para>
	///		<para>See also:</para>
	///		<para><see cref="V3_VoiceLineAudioSource"/></para>
	///		<para><see cref="Register(V3_VoiceLineAudioSource)"/></para>
	/// </summary>
	/// 
	/// <changelog>
	///		<log author="Elijah Shadbolt" date="10/08/2020">
	///			<para>Added comments.</para>
	///		</log>
	/// </changelog>
	/// 
	[SerializeField] // show in inspector, but cannot set from other scripts
	private HashSet<V3_VoiceLineAudioSource> m_vlas = new HashSet<V3_VoiceLineAudioSource>();



	/// <summary>
	///		<para>
	///			Informs this manager that there is a new
	///			instance of <see cref="V3_VoiceLineAudioSource"/> in the scene.
	///		</para>
	///		<para>
	///			Once the source has been registered,
	///			it must be unregistered by calling
	///			<see cref="Unregister(V3_VoiceLineAudioSource)"/>
	///			before the source is destroyed.
	///		</para>
	/// </summary>
	/// 
	/// <changelog>
	///		<log author="Elijah Shadbolt" date="10/08/2020">
	///			<para>Added comments.</para>
	///		</log>
	/// </changelog>
	/// 
	public void Register(V3_VoiceLineAudioSource vla)
	{
		m_vlas.Add(vla);
	}



	/// <summary>
	///		<para>
	///			Informs this manager that a <see cref="V3_VoiceLineAudioSource"/>
	///			is about to be destroyed.
	///		</para>
	///		<para>
	///			The source should first be registered with <see cref="Register(V3_VoiceLineAudioSource)"/>.
	///		</para>
	/// </summary>
	/// 
	/// <changelog>
	///		<log author="Elijah Shadbolt" date="10/08/2020">
	///			<para>Added comments.</para>
	///		</log>
	/// </changelog>
	/// 
	public void Unregister(V3_VoiceLineAudioSource vla)
	{
		m_vlas.Remove(vla);
	}



	/// <summary>
	///		<para>Invokes an action on every <see cref="V3_VoiceLineAudioSource"/> in the <see cref="m_vlas"/> collection.</para>
	/// </summary>
	/// <param name="action">
	///		<para>The function to call for each <see cref="V3_VoiceLineAudioSource"/> element.</para>
	///		<para>It is a first class function, i.e. a variable which can be called like a function.</para>
	/// </param>
	/// 
	/// <changelog>
	///		<log author="Elijah Shadbolt" date="10/08/2020">
	///			<para>Added comments.</para>
	///		</log>
	/// </changelog>
	/// 
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



	/// <summary>
	///		<para>True when a voice line is currently playing.</para>
	/// </summary>
	/// 
	/// <changelog>
	///		<log author="Elijah Shadbolt" date="10/08/2020">
	///			Added comments.
	///		</log>
	/// </changelog>
	/// 
	public bool isPlaying { get; private set; }



	/// <summary>
	///		<para>True when the audio has been paused.</para>
	///		<para>This property is independent of <see cref="isPlaying"/> (they do not affect each other).</para>
	/// </summary>
	/// 
	/// <changelog>
	///		<log author="Elijah Shadbolt" date="10/08/2020">
	///			Added comments.
	///		</log>
	/// </changelog>
	/// 
	public bool isPaused { get; private set; }



	/// <summary>
	///		<para>Queue of voice lines to be played after the current voice line has finished.</para>
	/// </summary>
	/// 
	/// <changelog>
	///		<log author="Elijah Shadbolt" date="10/08/2020">
	///			Added comments.
	///		</log>
	/// </changelog>
	/// 
	private Queue<VoiceLine> m_voiceLineQueue = new Queue<VoiceLine>();



	/// <summary>
	///		<para>The current voice line being played, or null.</para>
	///		<para>See also:</para>
	///		<para><see cref="m_sectionTimeRemaining"/></para>
	/// </summary>
	/// 
	/// <changelog>
	///		<log author="Elijah Shadbolt" date="10/08/2020">
	///			Added comments.
	///		</log>
	/// </changelog>
	/// 
	private VoiceLine m_currentVoiceLine = null;



	/// <summary>
	///		<para>
	///			The remaining duration of time before the
	///			currently playing voice line is deemed to have finished playing,
	///			after which the next voice line is loaded.
	///		</para>
	///		<para>See also:</para>
	///		<para><see cref="m_currentVoiceLine"/></para>
	/// </summary>
	/// 
	/// <changelog>
	///		<log author="Elijah Shadbolt" date="10/08/2020">
	///			Added comments.
	///		</log>
	/// </changelog>
	/// 
	private float m_sectionTimeRemaining = 0.0f;



	/// <summary>
	///		<para>Unity Message Method: <a href="https://docs.unity3d.com/ScriptReference/MonoBehaviour.Update.html"/></para>
	/// </summary>
	/// 
	/// <changelog>
	///		<log author="Elijah Shadbolt" date="10/08/2020">
	///			Added comments.
	///		</log>
	/// </changelog>
	/// 
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



	/// <summary>
	///		<para>
	///			Attempts to pop the next voice line from the queue <see cref="m_voiceLineQueue"/>
	///			and start playing it.
	///		</para>
	///		<para>If the queue is empty, it will stop all the voice line audio sources.</para>
	///		<para>See also:</para>
	///		<para><see cref="m_currentVoiceLine"/></para>
	/// </summary>
	/// 
	/// <changelog>
	///		<log author="Elijah Shadbolt" date="10/08/2020">
	///			Added comments.
	///		</log>
	/// </changelog>
	/// 
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
					V3_ClosedCaptions.instance.DisplayNewSubtitle(m_currentVoiceLine.subtitle, m_sectionTimeRemaining);
					ForEachVoiceLineAudioSource(s => s.Play(clip));
				}
				else
				{
					m_sectionTimeRemaining = m_currentVoiceLine.clipDurationIfNull + m_currentVoiceLine.paddingDuration;
					Debug.Log("Playing voice line without clip. length = " + m_sectionTimeRemaining, this);
					V3_ClosedCaptions.instance.DisplayNewSubtitle(m_currentVoiceLine.subtitle, m_sectionTimeRemaining);
					ForEachVoiceLineAudioSource(s => s.Stop());
				}
			}
		}
		else
		{
			Stop();
		}
	}



	/// <summary>
	///		<para>Stops playing the current voice line immediately.</para>
	///		<para>Clears the queue of subsequent voice lines.</para>
	/// </summary>
	/// 
	/// <changelog>
	///		<log author="Elijah Shadbolt" date="10/08/2020">
	///			Added comments.
	///		</log>
	/// </changelog>
	/// 
	public void Stop()
	{
		Debug.Log("Stopped playing voice lines.", this);
		isPlaying = false;
		m_sectionTimeRemaining = 0.0f;
		m_currentVoiceLine = null;
		m_voiceLineQueue.Clear();
		ForEachVoiceLineAudioSource(s => s.Stop()); // The expression `s => s.Stop()` is a lambda function (first class function).
	}



	/// <summary>
	///		<para>
	///			Plays the first voice line from the queue,
	///			only if there is not already a voice line playing.
	///		</para>
	/// </summary>
	/// 
	/// <changelog>
	///		<log author="Elijah Shadbolt" date="10/08/2020">
	///			Added comments.
	///		</log>
	/// </changelog>
	/// 
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



	/// <summary>
	///		<para>Adds a sequence of voice lines to the back of the queue.</para>
	///		<para>These will be played after the current playing voice line and any subsequent voice lines have finished playing.</para>
	/// </summary>
	/// <param name="voiceLines">The new sequence of voice lines to be played later.</param>
	/// 
	/// <changelog>
	///		<log author="Elijah Shadbolt" date="10/08/2020">
	///			Added comments.
	///		</log>
	/// </changelog>
	/// 
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



	/// <summary>
	///		<para>Replaces the queue with a new sequence of voice lines.</para>
	///		<para>If a voice line is currently playing, it is stopped and the first voice line from this new sequence is played instead.</para>
	/// </summary>
	/// <param name="voiceLines">The new sequence of voice lines to begin playing immediately.</param>
	/// 
	/// <changelog>
	///		<log author="Elijah Shadbolt" date="10/08/2020">
	///			Added comments.
	///		</log>
	/// </changelog>
	/// 
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
