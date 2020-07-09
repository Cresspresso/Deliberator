using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class V2_SigilExplode : MonoBehaviour
{
	public float lifetime = 10;

	private void Awake()
	{
		Destroy(gameObject, lifetime);
	}
}
