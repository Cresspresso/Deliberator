using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class V2_Quitter : MonoBehaviour
{
	public static void StaticQuit()
	{
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#endif
		Application.Quit();
	}

	public void Quit()
	{
		StaticQuit();
	}
}
