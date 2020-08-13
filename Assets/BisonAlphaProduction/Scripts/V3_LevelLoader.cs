using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
///		<para>Attach to a <see cref="UnityEngine.UI.Button"/>.</para>
///		<para>Set <see cref="sceneName"/> in the inspector.</para>
///		<para>When the button is clicked, the level will be loaded by name.</para>
/// </summary>
/// 
/// <changelog>
///		<log author="Elijah Shadbolt" date="13/08/2020">
///			<para>Added comments.</para>
///		</log>
/// </changelog>
/// 
public class V3_LevelLoader : MonoBehaviour
{
	public string sceneName = "Placholder";

	private void Awake()
	{
		GetComponent<Button>().onClick.AddListener(OnClick);
	}

	private void OnClick()
	{
		SceneManager.LoadScene(sceneName);
	}
}
