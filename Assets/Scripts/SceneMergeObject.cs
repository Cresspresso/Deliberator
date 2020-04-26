using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SceneMergeObject : MonoBehaviour
{
	private void Start()
	{
		var merger = FindObjectOfType<SceneMerger>();
		if (merger && merger.objects.Contains(this) == false)
		{
			Destroy(gameObject);
			return;
		}
	}
}
