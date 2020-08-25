using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Dependable))]
public class V3_EvacaLights : MonoBehaviour
{
	public Old_yRotationScript rotator;
	public Light theLight;
	public AudioSource audioSource;

	private void Awake()
	{
		GetComponent<Dependable>().onChanged.AddListener((isPowered) => this.enabled = isPowered);
	}

	private void OnEnable()
	{
		rotator.enabled = true;
		theLight.enabled = true;
		audioSource.enabled = true;
	}

	private void OnDisable()
	{
		rotator.enabled = false;
		theLight.enabled = false;
		audioSource.enabled = false;
	}
}
