using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SigilExplode : MonoBehaviour
{
	public float lifetime = 10;

	private void Awake()
	{
		Destroy(gameObject, lifetime);
	}
}
