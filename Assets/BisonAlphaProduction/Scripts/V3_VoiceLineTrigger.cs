using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
///		<para>When the player walks through this trigger collider, the <see cref="V3_VoiceLineManager"/> will play a voice line sequence.</para>
///		<para>Attach this script to a <see cref="GameObject"/> with a <see cref="Collider"/>.</para>
///		<para>Make sure that <see cref="Collider.isTrigger"/> is true.</para>
///		<para>See also:</para>
///		<para><see cref="V3_VoiceLineManager"/></para>
///		<para><see cref="V3_VoiceLineAudioSource"/></para>
/// </summary>
/// 
/// <changelog>
///		<log author="Elijah Shadbolt" date="10/08/2020">
///			Added comments.
///		</log>
/// </changelog>
/// 
public class V3_VoiceLineTrigger : MonoBehaviour
{
	/// <summary>
	///		<para>This is needed to make sure it is not triggered multiple times in the same frame from multiple trigger collider events.</para>
	/// </summary>
	/// 
	/// <changelog>
	///		<log author="Elijah Shadbolt" date="10/08/2020">
	///			Added comments.
	///		</log>
	/// </changelog>
	/// 
	public bool hasTriggered { get; private set; } = false;



	/// <summary>
	///		<para>See also <see cref="mode"/>.</para>
	///	</summary>
	/// 
	/// <changelog>
	///		<log author="Elijah Shadbolt" date="10/08/2020">
	///			Added comments.
	///		</log>
	/// </changelog>
	/// 
	public enum Mode { Replace, Enqueue }



	/// <summary>
	///		<para>If <see cref="Mode.Enqueue"/>, voice lines will be pushed to the back of the queue.</para>
	///		<para>If <see cref="Mode.Replace"/>, the queue will be cleared before pushing.</para>
	///		<para>See also:</para>
	///		<para><see cref="V3_VoiceLineManager.EnqueueSequence(IEnumerable{VoiceLine})"/></para>
	///		<para><see cref="V3_VoiceLineManager.ReplaceWithSequence(IEnumerable{VoiceLine})"/></para>
	/// </summary>
	/// 
	/// <changelog>
	///		<log author="Elijah Shadbolt" date="10/08/2020">
	///			Added comments.
	///		</log>
	/// </changelog>
	/// 
	[Tooltip(@"If `Enqueue`, voice lines will be pushed to the back of the queue. If `Replace`, the queue will be cleared before pushing.")]
	public Mode mode = Mode.Replace;



	/// <summary>
	///		<para>The sequence of voice lines to start playing.</para>
	///		<para>See also:</para>
	///		<para><see cref="V3_VoiceLineManager.EnqueueSequence(IEnumerable{VoiceLine})"/></para>
	///		<para><see cref="V3_VoiceLineManager.ReplaceWithSequence(IEnumerable{VoiceLine})"/></para>
	/// </summary>
	/// 
	/// <changelog>
	///		<log author="Elijah Shadbolt" date="10/08/2020">
	///			Added comments.
	///		</log>
	/// </changelog>
	/// 
	[Tooltip(@"The sequence of voice lines to start playing.")]
	[NonEmpty]
	public List<VoiceLine> voiceLines = new List<VoiceLine> { new VoiceLine() };



	/// <summary>
	///		<para>Starts playing the voicelines, and destroys this script's <see cref="GameObject"/>.</para>
	///		<para>See also:</para>
	///		<para><see cref="OnTriggerEnter(Collider)"/></para>
	/// </summary>
	/// 
	/// <changelog>
	///		<log author="Elijah Shadbolt" date="10/08/2020">
	///			<para>Added comments.</para>
	///			<para>Moved hasTriggered code from <see cref="OnTriggerEnter(Collider)"/> to here.</para>
	///		</log>
	/// </changelog>
	/// 
	public void PlayAndDie()
	{
		if (hasTriggered) return; /// prevent it from being called twice

		hasTriggered = true;

		var manager = V3_VoiceLineManager.instance;
		if (mode == Mode.Replace)
		{
			manager.ReplaceWithSequence(voiceLines);
		}
		else if (mode == Mode.Enqueue)
		{
			manager.EnqueueSequence(voiceLines);
		}
		else
		{
			Debug.LogError("Invalid enum value", this);
		}
		Destroy(gameObject);
	}



	/// <summary>
	///		<para>Unity Message Method: <a href="https://docs.unity3d.com/ScriptReference/MonoBehaviour.OnTriggerEnter.html"></a></para>
	/// </summary>
	/// 
	/// <changelog>
	///		<log author="Elijah Shadbolt" date="10/08/2020">
	///			<para>Added comments.</para>
	///			<para>Moved hasTriggered code from here to <see cref="PlayAndDie"/>.</para>
	///		</log>
	/// </changelog>
	/// 
	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			PlayAndDie();
		}
	}
}
