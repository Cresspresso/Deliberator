using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class V3_RestartMenuButton : MonoBehaviour
{
	private void Awake()
	{
		var button = GetComponent<Button>();
		button.onClick.AddListener(OnClick);
	}

	public void OnClick()
	{
		V2_GroundhogControl.instance.Die();
		V2_PauseMenu.instance.Unpause();
	}
}
