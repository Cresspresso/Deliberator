using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Old_GrabPointerFillImage : MonoBehaviour
{
	public Image fillImage;

	private Old_GrabPointer gp;

	private void UpdateGrab(Old_GrabPointer sender, float elapsedTime)
	{
		fillImage.fillAmount = elapsedTime / sender.goalTime;
	}

	void OnEnable()
	{
		if (!gp)
		{
			gp = GetComponent<Old_GrabPointer>();
			gp.onUpdateGrab.AddListener(UpdateGrab);

			UpdateGrab(gp, gp.elapsedTime);
		}
	}

	void OnDisable()
	{
		if (gp)
		{
			try
			{
				gp.onUpdateGrab.RemoveListener(UpdateGrab);
			}
			finally
			{
				gp = null;
			}
		}
	}
}
