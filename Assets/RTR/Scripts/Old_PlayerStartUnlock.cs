using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Old_PlayerStartUnlock : MonoBehaviour
{
	public Old_PlayerController player;
	public Old_GroundhogDay groundhogDay;

	private void Awake()
	{
		if (!groundhogDay)
		{
			groundhogDay = FindObjectOfType<Old_GroundhogDay>();
		}
		Debug.Assert(groundhogDay, "groundhogDay is null", this);

		if (!player)
		{
			player = FindObjectOfType<Old_PlayerController>();
		}
		Debug.Assert(player, "player is null", this);
	}

	private void Start()
	{
		player.isGameControlEnabled = false;
	}

	private void Update()
	{
		if (groundhogDay.elapsedTime > 0.0f)
		{
			player.isGameControlEnabled = true;
			Destroy(this);
		}
	}
}
