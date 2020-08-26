using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
///		<para>This <see cref="GameObject"/> will exist for as long as the scene keeps getting restarted by calling <see cref="RestartCurrentScene"/>.</para>
///		<para>Attach components to this <see cref="GameObject"/> to preserve data between scene restarts.</para>
///		<para>This <see cref="GameObject"/> is destroyed if a different scene is loaded, or the same scene is loaded without using <see cref="RestartCurrentScene"/>.</para>
///		<para>"Spar" stands for "scene preserve across restart".</para>
///		<para>See also:</para>
///		<para><see cref="V3_SparRandomizerDatabase{TValue}"/></para>
///		<para><see cref="V3_Randomizer{TValue, TSparRandomizerDatabase}"/></para>
/// </summary>
/// 
/// <changelog>
///		<log author="Elijah Shadbolt" date="11/08/2020">
///			<para>Added comments.</para>
///		</log>
/// </changelog>
/// 
public static class V3_SparGameObject
{
	/// <summary>
	///		<para>Name of the singleton <see cref="GameObject"/>.</para>
	///		<para>See also:</para>
	///		<para><see cref="FindOrCreate"/></para>
	/// </summary>
	/// 
	/// <changelog>
	///		<log author="Elijah Shadbolt" date="11/08/2020">
	///			<para>Added comments.</para>
	///		</log>
	/// </changelog>
	/// 
	public const string instanceName = "SprGameObjectSingleton";



	/// <summary>
	///		<para>See also:</para>
	///		<para><see cref="instance"/></para>
	/// </summary>
	/// 
	/// <changelog>
	///		<log author="Elijah Shadbolt" date="11/08/2020">
	///			<para>Added comments.</para>
	///		</log>
	/// </changelog>
	/// 
	private static GameObject m_instance;



	/// <summary>
	///		<para>The current instance of the singleton <see cref="GameObject"/> in the scene.</para>
	///		<para>Returns <see langword="null"/> if no such instance exists in the scene.</para>
	///		<para>See also:</para>
	///		<para><see cref="FindOrCreate"/></para>
	///		<para><see cref="FindOrCreateComponent{T}"/></para>
	/// </summary>
	/// 
	/// <changelog>
	///		<log author="Elijah Shadbolt" date="11/08/2020">
	///			<para>Added comments.</para>
	///		</log>
	/// </changelog>
	/// 
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



	/// <summary>
	///		<para>Destroys the singleton <see cref="GameObject"/>.</para>
	/// </summary>
	/// 
	/// <changelog>
	///		<log author="Elijah Shadbolt" date="11/08/2020">
	///			<para>Added comments.</para>
	///		</log>
	/// </changelog>
	/// 
	public static void Destroy()
	{
		if (m_instance)
		{
			m_instance.SetActive(false);
			GameObject.Destroy(m_instance);
		}
		m_instance = null;
	}



	/// <summary>
	///		<para>Returns the singleton instance of the <see cref="GameObject"/> in the scene.</para>
	///		<para>If it does not exist, a new one is created.</para>
	///		<para>Never returns <see langword="null"/>.</para>
	///		<para>See also:</para>
	///		<para><see cref="FindOrCreateComponent{T}"/></para>
	///		<para><see cref="instance"/></para>
	/// </summary>
	/// 
	/// <changelog>
	///		<log author="Elijah Shadbolt" date="11/08/2020">
	///			<para>Added comments.</para>
	///		</log>
	/// </changelog>
	/// 
	public static GameObject FindOrCreate()
	{
		var go = instance;
		if (go) return go;

		m_instance = new GameObject(instanceName);
		return m_instance;
	}



	/// <summary>
	///		<para>Returns the singleton instance of a <see cref="Component"/> attached to the singleton <see cref="GameObject"/>.</para>
	///		<para>If the <see cref="GameObject"/> does not exist, a new one is created.</para>
	///		<para>If the <see cref="Component"/> does not exist, a new one is attached to the <see cref="GameObject"/>.</para>
	///		<para>Never returns <see langword="null"/>.</para>
	///		<para>See also:</para>
	///		<para><see cref="FindOrCreate"/></para>
	///		<para><see cref="instance"/></para>
	/// </summary>
	/// <typeparam name="T">The type of <see cref="Component"/>.</typeparam>
	/// 
	/// <changelog>
	///		<log author="Elijah Shadbolt" date="11/08/2020">
	///			<para>Added comments.</para>
	///		</log>
	/// </changelog>
	/// 
	public static T FindOrCreateComponent<T>() where T : Component
	{
		var go = FindOrCreate();
		var script = go.GetComponent<T>();
		if (!script) script = go.AddComponent<T>();
		return script;
	}



	/// <summary>
	///		<para>Can return <see langword="null"/> if the <see cref="instance"/> does not exist or the <see cref="Component"/> does not exist on it.</para>
	/// </summary>
	/// 
	/// <changelog>
	///		<log author="Elijah Shadbolt" date="26/08/2020">
	///			<para>Created this method.</para>
	///		</log>
	/// </changelog>
	/// 
	public static T GetComponent<T>() where T : Component
	{
		var go = instance;
		if (!go) return null;
		return go.GetComponent<T>();
	}



	/// <summary>
	///		<para>Begins the process of preserving the <see cref="GameObject"/> when loading a new scene.</para>
	///		<para>Call this before a different scene is loaded.</para>
	///		<para>WARNING: Make sure it is only called once, and load the next scene immediately after.</para>
	///		<para>See also:</para>
	///		<para><see cref="RestartCurrentScene"/></para>
	///		<para><see cref="LoadScene(int)"/></para>
	/// </summary>
	/// 
	/// <changelog>
	///		<log author="Elijah Shadbolt" date="11/08/2020">
	///			<para>Added comments.</para>
	///		</log>
	/// </changelog>
	/// 
	public static void OnSceneUnload_BeginPreserve()
	{
		SceneManager.sceneLoaded += OnSceneLoaded_EndPreserve;
		var go = instance;
		if (go)
		{
			Object.DontDestroyOnLoad(go);
		}
	}



	/// <summary>
	///		<para>Listener/callback, subscribed to the <see cref="SceneManager.sceneLoaded"/> event.</para>
	///		<para>Concludes the process of preserving the <see cref="GameObject"/> when loading a new scene.</para>
	///		<para>See also:</para>
	///		<para><see cref="RestartCurrentScene"/></para>
	/// </summary>
	/// <param name="scene">The new scene that has just finished loading.</param>
	/// <param name="mode">How the new scene was loaded.</param>
	/// 
	/// <changelog>
	///		<log author="Elijah Shadbolt" date="11/08/2020">
	///			<para>Added comments.</para>
	///		</log>
	/// </changelog>
	/// 
	private static void OnSceneLoaded_EndPreserve(Scene scene, LoadSceneMode mode)
	{
		SceneManager.sceneLoaded -= OnSceneLoaded_EndPreserve;
		var go = instance;
		if (go)
		{
			/// Undo <see cref="Object.DontDestroyOnLoad(Object)"/>
			/// so that the game object will be destroyed
			/// unless <see cref="RestartCurrentScene"/> is called again.
			SceneManager.MoveGameObjectToScene(go, scene);
		}
	}



	/// <summary>
	///		<para>Restarts the current scene, without destroying the singleton <see cref="GameObject"/> instance.</para>
	///		<para>See also:</para>
	///		<para><see cref="LoadScene(int)"/></para>
	///		<para><see cref="OnSceneUnload_BeginPreserve"/></para>
	/// </summary>
	/// 
	/// <changelog>
	///		<log author="Elijah Shadbolt" date="11/08/2020">
	///			<para>Added comments.</para>
	///		</log>
	/// </changelog>
	/// 
	public static void RestartCurrentScene()
	{
		OnSceneUnload_BeginPreserve();
		SceneManager.LoadScene(V2_Utility.GetCurrentSceneBuildIndex());
	}



	/// <summary>
	///		<para>Loads a different scene, without destroying the singleton <see cref="GameObject"/> instance.</para>
	///		<para>See also:</para>
	///		<para><see cref="RestartCurrentScene"/></para>
	///		<para><see cref="SceneManager.LoadScene(int)"/></para>
	///		<para><see cref="OnSceneUnload_BeginPreserve"/></para>
	/// </summary>
	/// 
	/// <changelog>
	///		<log author="Elijah Shadbolt" date="11/08/2020">
	///			<para>Added this function as an alternative to <see cref="RestartCurrentScene"/>.</para>
	///			<para>Added comments.</para>
	///		</log>
	/// </changelog>
	/// 
	public static void LoadScene(int sceneBuildIndex)
	{
		OnSceneUnload_BeginPreserve();
		SceneManager.LoadScene(sceneBuildIndex);
	}



	public static void LoadScene(string sceneName)
	{
		OnSceneUnload_BeginPreserve();
		SceneManager.LoadScene(sceneName);
	}
}
