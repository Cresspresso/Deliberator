using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(V2_NumPad))]
public class V2_NumPadLock : MonoBehaviour
{
	[SerializeField]
	private V2_NumPad m_pad;
	public V2_NumPad pad {
		get
		{
			if (!m_pad)
			{
				m_pad = GetComponentInParent<V2_NumPad>();
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

#pragma warning disable CS0649
	[SerializeField]
	private V3_KeyCardReader_Sprites padlockSprites;
#pragma warning restore CS0649

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

			padlockSprites.ShowUnlockedImage();
		}
		else
		{
			onIncorrectSubmitted.Invoke();

			incorrectSound.Play();

			padlockSprites.ShowShakeImage();
		}
	}
}
