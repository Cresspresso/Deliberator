using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///		<para>Plays an <see cref="AudioClip"/> sent by the <see cref="V3_VoiceLineManager"/> when a new voiceline begins playing.</para>
///		<para>Attach to a <see cref="GameObject"/> with an <see cref="AudioSource"/>.</para>
///		<para>The <see cref="AudioSource"/> can have <see cref="AudioSource.spatialBlend"/> set to full 3D.</para>
///		<para>See also:</para>
///		<para><see cref="V3_VoiceLineManager"/></para>
///		<para><see cref="V3_VoiceLineTrigger"/></para>
/// </summary>
/// 
/// <changelog>
///		<log author="Elijah Shadbolt" date="10/08/2020">
///			Added comments.
///		</log>
/// </changelog>
/// 
[RequireComponent(typeof(AudioSource))]
public class V3_VoiceLineAudioSource : MonoBehaviour
{
	/// <summary>
	///		<para>The manager that this script has registered itself with.</para>
	/// </summary>
	/// 
	/// <changelog>
	///		<log author="Elijah Shadbolt" date="10/08/2020">
	///			Added comments.
	///		</log>
	/// </changelog>
	/// 
	private V3_VoiceLineManager manager;



	/// <summary>
	///		<para>Required component.</para>
	/// </summary>
	/// 
	/// <changelog>
	///		<log author="Elijah Shadbolt" date="10/08/2020">
	///			Added comments.
	///		</log>
	/// </changelog>
	/// 
	public AudioSource audioSource { get; private set; }



	/// <summary>
	///		<para>Plays an audio clip.</para>
	///		<para>This is called by this script's manager <see cref="manager"/>.</para>
	///		<para>See also:</para>
	///		<para><see cref="V3_VoiceLineManager.PopAndPlayVoiceLine"/></para>
	/// </summary>
	/// 
	/// <changelog>
	///		<log author="Elijah Shadbolt" date="10/08/2020">
	///			Added comments.
	///		</log>
	/// </changelog>
	/// 
	public void Play(AudioClip clip)
	{
		audioSource.Stop();
		audioSource.clip = clip;
		audioSource.Play();
	}



	/// <summary>
	///		<para>Stops playing the audio clip (if one was playing).</para>
	///		<para>This is called by this script's manager <see cref="manager"/>.</para>
	///		<para>See also:</para>
	///		<para><see cref="V3_VoiceLineManager.PopAndPlayVoiceLine"/></para>
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
		audioSource.Stop();
	}



	/// <summary>
	///		<para>Unity Message Method: <a href="https://docs.unity3d.com/ScriptReference/MonoBehaviour.Start.html"/></para>
	/// </summary>
	/// 
	/// <changelog>
	///		<log author="Elijah Shadbolt" date="10/08/2020">
	///			Added comments.
	///		</log>
	/// </changelog>
	/// 
	private void Start()
	{
		audioSource = GetComponent<AudioSource>();

		/// This code must be in <see cref="Start"/>,
		/// after <see cref="V3_VoiceLineManager.instance"/>
		/// is populated in <see cref="V3_VoiceLineManager.Awake"/>.
		/// 
		manager = V3_VoiceLineManager.instance;
		Debug.Assert(manager);
		manager.Register(this);
	}



	/// <summary>
	///		<para>Unity Message Method: <a href="https://docs.unity3d.com/ScriptReference/MonoBehaviour.OnDestroy.html"/></para>
	/// </summary>
	/// 
	/// <changelog>
	///		<log author="Elijah Shadbolt" date="10/08/2020">
	///			Added comments.
	///		</log>
	/// </changelog>
	/// 
	private void OnDestroy()
	{
		/// Life check because the manager could be destroyed before this script.
		if (manager)
		{
			manager.Unregister(this);
		}
	}
}
