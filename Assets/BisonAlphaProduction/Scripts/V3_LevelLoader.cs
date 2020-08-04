using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
