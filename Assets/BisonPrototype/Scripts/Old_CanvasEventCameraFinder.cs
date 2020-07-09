using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Old_CanvasEventCameraFinder : MonoBehaviour
{
	private void Start()
	{
		var canvas = GetComponent<Canvas>();
		canvas.worldCamera = Camera.main;
	}
}
