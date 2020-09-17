using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class V3_StaminaDrainTrigger : MonoBehaviour
{
	public enum Mode
	{
		Pause,
		Resume,
		Toggle,
	}
	public Mode mode;

	public bool destroyAfterTriggered = true;
	public bool triggered { get; private set; } = false;

	private void OnTriggerEnter(Collider other)
	{
		if (triggered) return;

		if (other.CompareTag("Player"))
		{
			triggered = true;

			var gc = V2_GroundhogControl.instance;
			switch (mode)
			{
				case Mode.Pause: gc.isPaused = true; break;
				case Mode.Resume: gc.isPaused = false; break;
				case Mode.Toggle: gc.isPaused = !gc.isPaused; break;
				default: break;
			}

			if (destroyAfterTriggered)
			{
				Destroy(gameObject);
			}
		}
	}
}
