using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PathTrigger : Trigger
{
#pragma warning disable CS0649
	[SerializeField]
	private PathGroup m_group;
#pragma warning restore CS0649
	public PathGroup group => m_group;

	public override bool IsValidCollider(Collider other)
		=> other.CompareTag("Player");

	protected virtual void Start()
	{
		if (group)
		{
			group.Register(this);
		}
		else
		{
			Debug.LogWarning("Group is null", this);
		}
	}

	protected override void OnValidTriggerEnter(Collider first)
	{
		if (group)
		{
			group.InvokePartEnter(this);
		}
		else
		{
			Debug.LogWarning("Group is null", this);
		}
	}

	protected override void OnValidTriggerExit(Collider last)
	{
		if (group)
		{
			group.InvokePartExit(this);
		}
		else
		{
			Debug.LogWarning("Group is null", this);
		}
	}
}
