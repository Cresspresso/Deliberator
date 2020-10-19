using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TSingleton = V2_Singleton<V4_PlaytestConsole>;

/// <summary>
///		<para>Manager for tools used by playtesters in the build.</para>
///		<para>Must not be deactivated.</para>
/// </summary>
/// 
/// <changelog>
///		<log author="Elijah Shadbolt" date="19/10/2020">
///			<para>Created this script.</para>
///		</log>
/// </changelog>
/// 
public class V4_PlaytestConsole : MonoBehaviour
{
#pragma warning disable CS0649

	[SerializeField]
	private Text m_consoleTextElement;
	public Text consoleTextElement => m_consoleTextElement;

	[SerializeField]
	private GameObject m_playtestToolsMenuPanel;
	public GameObject playtestToolsMenuPanel => m_playtestToolsMenuPanel;

	[SerializeField]
	private V2_PauseMenu m_pauseMenu;
	public V2_PauseMenu pauseMenu => m_pauseMenu;

	[SerializeField]
	private GameObject m_visuals;
	public GameObject visuals => m_visuals;

#pragma warning restore CS0649

	public static V4_PlaytestConsole instance => TSingleton.instance;

	private void Awake()
	{
		TSingleton.OnAwake(this, V2_SingletonDuplicateMode.Ignore);
		consoleTextElement.text = "";

		const string s = "------------------";
		Logi(s + " SCENE LOADED " + s);

		visuals.SetActive(false);
	}


	public const bool kPrintToUnityConsole = true;

	public void Logi(string message)
	{
		if (kPrintToUnityConsole)
		{
			Debug.Log("PlaytestConsole: " + message);
		}
		AppendText("\n" + message);
	}

	public void Logi(string message, Object context)
	{
		if (kPrintToUnityConsole)
		{
			Debug.Log("PlaytestConsole: " + message, context);
		}
		AppendText("\n" + message);
	}

	private void AppendText(string text)
	{
		text = consoleTextElement.text + text;
		const int maxLength = 10_000;
		if (text.Length > maxLength)
		{
			text = text.Substring(text.Length - maxLength);
		}
		consoleTextElement.text = text;
	}



	public static void Log(string message) => instance.Logi(message);
	public static void Log(string message, Object context) => instance.Logi(message, context);



	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.F3))
		{
			if (visuals.activeInHierarchy)
			{
				visuals.SetActive(false);
			}
			else
			{
				visuals.SetActive(true);
			}
		}
	}
}
