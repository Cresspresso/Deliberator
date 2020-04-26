using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMerger : MonoBehaviour
{
	public string[] sceneNames = new string[0];
	public HashSet<SceneMergeObject> objects { get; private set; }

	private void Awake()
	{
		if (FindObjectOfType<SceneMerger>() != this)
		{
			Debug.LogError("Two SceneMergers in the same Scene", this);
			return;
		}

		objects = new HashSet<SceneMergeObject>(FindObjectsOfType<SceneMergeObject>());
	}

	private void Start()
	{
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
