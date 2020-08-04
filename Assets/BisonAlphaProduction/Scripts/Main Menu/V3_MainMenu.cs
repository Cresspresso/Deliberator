using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class V3_MainMenu : MonoBehaviour
{
	private V2_MenuNavigation menuNavigation;
	public GameObject initialPanel;

	private void Awake()
	{
		menuNavigation = GetComponent<V2_MenuNavigation>();
		menuNavigation.GoInto(initialPanel);
	}
}
