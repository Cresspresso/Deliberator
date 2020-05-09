using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <author>Elijah Shadbolt</author>
public class SceneMerger : MonoBehaviour
{
	public string[] sceneNames = new string[0];

	private void Start()
	{
		// load scenes async
		foreach (var name in sceneNames)
		{
			try
			{
				SceneManager.LoadSceneAsync(name, LoadSceneMode.Additive);
			}
			catch (Exception e)
			{
				Debug.LogException(e, this);
			}
		}
	}
}
