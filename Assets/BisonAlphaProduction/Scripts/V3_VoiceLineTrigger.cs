using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <author>Elijah Shadbolt</author>
/// <stage>Alpha Production</stage>
public class V3_VoiceLineTrigger : MonoBehaviour
{
	public bool hasTriggered { get; private set; } = false;

	public enum Mode { Replace, Enqueue }
	[Tooltip("If `Enqueue`, voice lines will be pushed to the back of the queue. If `Replace`, the queue will be cleared before pushing.")]
	public Mode mode = Mode.Replace;
	public List<VoiceLine> voiceLines = new List<VoiceLine> { new VoiceLine() };

	public virtual void OnTriggered()
	{
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

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			if (!hasTriggered)
			{
				hasTriggered = true;
				OnTriggered();
			}
		}
	}
}
