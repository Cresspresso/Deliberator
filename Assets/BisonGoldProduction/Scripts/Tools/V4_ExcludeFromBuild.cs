using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[DisallowMultipleComponent]
public class V4_ExcludeFromBuild : MonoBehaviour
{
	private void Awake()
	{
		Destroy(gameObject);
	}
}
