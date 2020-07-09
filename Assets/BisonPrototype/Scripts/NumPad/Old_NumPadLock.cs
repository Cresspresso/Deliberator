using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Old_NumPad))]
public class Old_NumPadLock : MonoBehaviour
{
	[SerializeField]
	private Old_NumPad m_pad;
	public Old_NumPad pad {
		get
		{
			if (!m_pad)
			{
				m_pad = GetComponentInParent<Old_NumPad>();
			}
			return m_pad;
		}
	}

	public string passcode = "1234";

	[SerializeField]
	private UnityEvent m_onCorrectSubmitted = new UnityEvent();
	public UnityEvent onCorrectSubmitted => m_onCorrectSubmitted;
	[SerializeField]
	private UnityEvent m_onIncorrectSubmitted = new UnityEvent();
	public UnityEvent onIncorrectSubmitted => m_onIncorrectSubmitted;

	private void Awake()
	{
		var pad = this.pad;
		pad.onSubmit.AddListener(OnSubmit);
	}

	private void OnDestroy()
	{
		pad.onSubmit.RemoveListener(OnSubmit);
	}

	private void OnSubmit(string code)
	{

		if (code == passcode)
		{
			onCorrectSubmitted.Invoke();

			var am = FindObjectOfType<Old_AudioManager>();
			if (am) { am.PlaySound("beepCorrect"); }
		}
		else
		{
			onIncorrectSubmitted.Invoke();

			var am = FindObjectOfType<Old_AudioManager>();
			if (am) { am.PlaySound("beepWrong"); }
		}
	}
}
