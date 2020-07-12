using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

/// Elijah Shadbolt
/// Alpha Production
public class V3_KeyCardReader_Sprites : MonoBehaviour
{
	public Image unlockedImage;
	public Image lockedImage;
	public Image shakeImage;

	public static void ShowImage(Image show, params Image[] hide)
	{
		if (show) show.gameObject.SetActive(true);

		foreach (var other in hide)
		{
			if (other) other.gameObject.SetActive(false);
		}
	}
	public void ShowUnlockedImage() => ShowImage(unlockedImage, lockedImage, shakeImage);
	public void ShowLockedImage() => ShowImage(lockedImage, unlockedImage, shakeImage);
	public void ShowShakeImage()
	{
		ShowImage(shakeImage, lockedImage, unlockedImage);
		shakeImage.transform.DOKill(true);
		shakeImage.transform.DOShakePosition(4.0f, new Vector3(20.0f, 0, 0), 5, 0);
	}

	private void Start()
	{
		ShowLockedImage();
	}
}
