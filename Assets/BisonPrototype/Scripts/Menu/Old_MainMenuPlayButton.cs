using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Old_MainMenuPlayButton : MonoBehaviour
{
	[SerializeField]
	private Button m_button;
	public Button button {
		get
		{
			if (!m_button)
			{
				m_button = GetComponent<Button>();
			}
			return m_button;
		}
	}

#pragma warning disable CS0649
	[SerializeField]
	private Animator m_anim;
#pragma warning restore CS0649
	public Animator anim => m_anim;

	public Old_TownNuke nuke;

	public float delay = 5.0f;
	public bool isPlaying = false;

	private void Awake()
	{
		button.onClick.AddListener(OnClicked);
	}

	private void OnDestroy()
	{
		button.onClick.RemoveListener(OnClicked);
	}

	private void OnClicked()
	{
		isPlaying = true;
		PlayAnimations();
	}

	private void Update()
	{
		if (isPlaying)
		{
			delay -= Time.deltaTime;
			if (delay <= 0.0f)
			{
				SceneManager.LoadScene(1);
			}
		}
	}

	private void PlayAnimations()
	{
		if (anim) { anim.enabled = true; }

		if (!nuke) { nuke = FindObjectOfType<Old_TownNuke>(); }
		if (nuke)
		{
			nuke.Play();
		}
	}
}
