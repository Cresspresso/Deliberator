using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///		<para>Sound effects for a door.</para>
///		<para>Must be a child of a <see cref="V3_Door"/>.</para>
///		<para>See also:</para>
///		<para><see cref="V3_Door"/></para>
/// </summary>
/// 
/// <changelog>
///		<log author="Elijah Shadbolt" date="17/08/2020">
///			<para>Added this script.</para>
///		</log>
/// </changelog>
/// 
public class V3_DoorSounds : MonoBehaviour
{
#pragma warning disable CS0649
	[SerializeField]
	private AudioSource m_lockedSound;
#pragma warning restore CS0649
	public AudioSource lockedSound => m_lockedSound;

#pragma warning disable CS0649
	[SerializeField]
	private AudioSource m_openingSound;
#pragma warning restore CS0649
	public AudioSource openingSound => m_openingSound;

#pragma warning disable CS0649
	[SerializeField]
	private AudioSource m_closingSound;
#pragma warning restore CS0649
	public AudioSource closingSound => m_closingSound;

	public float soundDelay { get; set; } = 0.0f;

	private IEnumerator Co_PlaySound(AudioSource audioSource, float delay)
	{
		yield return new WaitForSeconds(delay);
		if (audioSource)
		{
			audioSource.Play();
		}
	}

	/// <summary>
	///		<para>Called by <see cref="V3_Door"/>.</para>
	/// </summary>
	public void PlaySound(AudioSource audioSource)
	{
		if (soundDelay <= 0.0f)
		{
			if (audioSource)
			{
				audioSource.Play();
			}
		}
		else
		{
			StartCoroutine(Co_PlaySound(audioSource, soundDelay));
		}
	}
}
