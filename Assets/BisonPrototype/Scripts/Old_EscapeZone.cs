using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Old_EscapeZone : MonoBehaviour
{
	private Animator anim;
	public GameObject visuals;
	public bool isEscaping => anim.enabled;

	private void Start()
	{
		anim = GetComponent<Animator>();
		visuals.SetActive(false);
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.GetComponentInParent<Old_PlayerController>())
		{
			var gd = FindObjectOfType<Old_GroundhogDay>();
			if (!gd.isGameEnding)
			{
				gd.isGameEnding = true;
				visuals.SetActive(true);
				anim.enabled = true;
				FindObjectOfType<Old_PlayerController>().isGameControlEnabled = false;
			}
		}
	}

	private void Update()
	{
		if (anim.enabled && anim.IsInTransition(0))
		{
			var info = anim.GetAnimatorTransitionInfo(0);
			if (info.IsUserName("CustomTransition"))
			{
				SceneManager.LoadScene(2);// "EscapeVideoScene"
			}
		}
	}
}
