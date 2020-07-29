using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// <para>This GameObject will exist for as long as the scene keeps getting restarted by the groundhog control timer.</para>
/// <para>Spar stands for Scene Preserve across Restart.</para>
/// </summary>
public static class V3_SparGameObject
{
	public const string instanceName = "SprGameObjectSingleton";

	private static GameObject m_instance;
	public static GameObject instance {
		get
		{
			if (!m_instance)
			{
				m_instance = GameObject.Find(instanceName);
			}
			return m_instance;
		}
	}

	public static void Destroy()
	{
		if (m_instance) GameObject.Destroy(m_instance);
		m_instance = new GameObject(instanceName);
	}

	public static GameObject FindOrCreate()
	{
		var go = instance;
		if (go) return go;

		m_instance = new GameObject(instanceName);
		return m_instance;
	}

	public static T FindOrCreateComponent<T>() where T : Component
	{
		var go = FindOrCreate();
		var script = go.GetComponent<T>();
		if (!script) script = go.AddComponent<T>();
		return script;
	}

	private static void OnSceneUnload_BeginPreserve()
	{
		SceneManager.sceneLoaded += OnSceneLoaded_EndPreserve;
		var go = instance;
		if (go)
		{
			GameObject.DontDestroyOnLoad(go);
		}
	}

	private static void OnSceneLoaded_EndPreserve(Scene scene, LoadSceneMode mode)
	{
		SceneManager.sceneLoaded -= OnSceneLoaded_EndPreserve;
		var go = instance;
		if (go)
		{
			SceneManager.MoveGameObjectToScene(go, scene);
		}
	}

	public static void RestartCurrentScene()
	{
		OnSceneUnload_BeginPreserve();
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}
}
