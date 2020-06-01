using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(NumPad))]
public class NumPadLock : MonoBehaviour
{
	[SerializeField]
	private NumPad m_pad;
	public NumPad pad {
		get
		{
			if (!m_pad)
			{
				m_pad = GetComponentInParent<NumPad>();
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

	public AudioSource correctSound;
	public AudioSource incorrectSound;

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

			correctSound.Play();
		}
		else
		{
			onIncorrectSubmitted.Invoke();

			incorrectSound.Play();
		}
	}
}
