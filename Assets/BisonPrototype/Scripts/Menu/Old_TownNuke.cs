using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Old_TownNuke : MonoBehaviour
{
	public float slowSpeed = 0.05f;

	public GameObject[] gameObjectsToActivate = new GameObject[0];
	public Animator[] animatorsToEnable = new Animator[0];
	public Transform[] animatorsToEnableParents = new Transform[0];
	public Animator[] animatorsToSetSpeed = new Animator[0];
	public ParticleSystem[] particleSystemsToPlay = new ParticleSystem[0];

	private void Start()
	{
		Time.timeScale = slowSpeed;

		//foreach (var go in gameObjectsToActivate.Where(a => a))
		//{
		//	go.SetActive(false);
		//}

		//foreach (var anim in animatorsToEnable.Where(a => a))
		//{
		//	anim.enabled = false;
		//}

		//foreach (var anims in animatorsToEnableParents.Where(t => t)
		//	.Select(t => t.GetComponentsInChildren<Animator>().Where(a => a)))
		//{
		//	foreach (var anim in anims)
		//	{
		//		anim.enabled = false;
		//	}
		//}

		//foreach (var anim in animatorsToSetSpeed.Where(a => a))
		//{
		//	anim.SetFloat("Speed", slowSpeed);
		//}

		//foreach (var ps in particleSystemsToPlay.Where(a => a))
		//{
		//	ps.Stop();
		//}
	}

	public void Play()
	{
		Time.timeScale = 1.0f;

		//foreach (var go in gameObjectsToActivate.Where(a => a))
		//{
		//	go.SetActive(true);
		//}

		//foreach (var anim in animatorsToEnable.Where(a => a))
		//{
		//	anim.enabled = true;
		//}

		//foreach (var anims in animatorsToEnableParents.Where(t => t)
		//	.Select(t => t.GetComponentsInChildren<Animator>().Where(a => a)))
		//{
		//	foreach (var anim in anims)
		//	{
		//		anim.enabled = true;
		//	}
		//}

		//foreach (var anim in animatorsToSetSpeed.Where(a => a))
		//{
		//	anim.SetFloat("Speed", 1.0f);
		//}

		//foreach (var ps in particleSystemsToPlay.Where(a => a))
		//{
		//	ps.Play();
		//}
	}
}
