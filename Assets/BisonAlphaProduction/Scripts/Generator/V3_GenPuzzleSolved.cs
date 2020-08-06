using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class V3_GenPuzzleSolved : MonoBehaviour
{
	private void Awake()
	{
		GetComponent<Dependable>().onChanged.AddListener(OnPoweredChanged);
	}

	private void OnPoweredChanged(bool isPowered)
	{
		if (!enabled) return;

		if (isPowered)
		{
			enabled = false;
			Debug.Log("All generators Powered!");
		}
	}
}
