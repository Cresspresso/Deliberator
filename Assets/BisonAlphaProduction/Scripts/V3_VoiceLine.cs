using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;

/// <summary>
///		<para>Data structure for a voice line audio clip and closed captions text.</para>
///		<para>Can be drawn in the inspector.</para>
///		<para>See also:</para>
///		<para><see cref="V3_VoiceLineTrigger.voiceLines"/></para>
///		<para><see cref="V3_VoiceLineManager"/></para>
///		<para><see cref="V3_VoiceLineAudioSource"/></para>
/// </summary>
/// 
/// <changelog>
///		<log author="Elijah Shadbolt" date="10/08/2020">
///			<para>Added comments.</para>
///		</log>
/// </changelog>
/// 
[Serializable]
public class VoiceLine
{
	/// <summary>
	///		<para>The audio clip to play as part of this voice line, or null.</para>
	/// </summary>
	/// 
	/// <changelog>
	///		<log author="Elijah Shadbolt" date="10/08/2020">
	///			<para>Added comments.</para>
	///		</log>
	/// </changelog>
	/// 
	[Tooltip(@"
The audio clip to play as part of this voice line, or null.
")]
	public AudioClip clip;



	/// <summary>
	///		<para>If <see cref="clip"/> is null, this value is used in place of the clip's <see cref="AudioClip.length"/>.</para>
	/// </summary>
	/// 
	/// <changelog>
	///		<log author="Elijah Shadbolt" date="10/08/2020">
	///			<para>Added comments.</para>
	///		</log>
	/// </changelog>
	/// 
	[Tooltip(@"
If Clip is null, this value is used in place of the clip's length.
")]
	public float clipDurationIfNull = 0.0f;



	/// <summary>
	///		<para>
	///			Time added after the audio clip has finished playing,
	///			inserted before the voice line is considered finished playing.
	///		</para>
	/// </summary>
	/// 
	/// <changelog>
	///		<log author="Elijah Shadbolt" date="10/08/2020">
	///			<para>Added comments.</para>
	///		</log>
	/// </changelog>
	/// 
	[Tooltip(@"
Time added after the audio clip has finished playing,
inserted before the voice line is considered finished playing.
")]
	public float paddingDuration = 0.1f;



	/// <summary>
	///		<para>The closed caption text to go along with the audio clip.</para>
	/// </summary>
	/// 
	/// <changelog>
	///		<log author="Elijah Shadbolt" date="10/08/2020">
	///			<para>Added comments.</para>
	///		</log>
	/// </changelog>
	/// 
	[Tooltip(@"
The closed caption text to go along with the audio clip.
")]
	[TextArea(1, 3)]
	public string subtitle = "<Missing Subtitle>";
}
