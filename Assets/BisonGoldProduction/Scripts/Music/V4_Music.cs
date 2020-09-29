﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class V4_Music : MonoBehaviour
{
#pragma warning disable CS0649

	[SerializeField]
	private int m_ID = 1;
	public int ID => m_ID;

#pragma warning restore CS0649

	private void Awake()
	{
		var spar = V3_SparGameObject.FindOrCreateComponent<V4_SparMusic>();
		if (spar.instanceID == ID)
		{
			Destroy(gameObject);
		}
		else
		{
			if (spar.child)
			{
				Destroy(spar.child);
				spar.child = null;
			}
			spar.instanceID = ID;
			gameObject.transform.SetParent(spar.transform, true);
			spar.child = gameObject;
		}
	}
}
