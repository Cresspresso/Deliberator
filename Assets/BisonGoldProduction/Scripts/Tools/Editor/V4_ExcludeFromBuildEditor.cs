using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

public class V4_ExcludeFromBuildEditor : IPreprocessBuildWithReport, IPostprocessBuildWithReport
{
	public int callbackOrder => 0;

	public void OnPreprocessBuild(BuildReport report)
	{
		//var sceneNames = new string[]
		//{
		//	"Intro Scene",
		//};
		//foreach (var sceneName in sceneNames)
		//{
		//	EditorSceneManager.OpenScene(sceneName);
		//	foreach (var go in Object.FindObjectsOfType<V4_ExcludeFromBuild>().Select(c => c.gameObject))
		//	{

		//	}
		//}
	}
	
	public void OnPostprocessBuild(BuildReport report)
	{

	}
}
