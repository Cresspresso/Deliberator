using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Old_RewindUI : MonoBehaviour
{
	public Animator anim;

	private void Update()
	{
		if (Old_GroundhogDay.instance.isRewinding)
		{
			anim.SetTrigger("Rewind");
			this.enabled = false;
		}
	}
}
