using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(V2_NumPad))]
[RequireComponent(typeof(Dependable))]
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
	public int[] passcodeInts => passcode.Select(c => (int)(c - '0')).ToArray();

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



	public int initialTries = 3;
	public int triesRemaining;
	private void ResetTriesRemaining()
	{
		staminaRequiredRemaining = 0;
		triesRemaining = initialTries;
	}

	[SerializeField]
	public float staminaUntilResetTries = 100;

	private float staminaRequiredRemaining = 0;
	private V2_GroundhogControl gc;



	private void Awake()
	{
		var pad = this.pad;
		pad.onSubmit.AddListener(OnSubmit);

		gc = V2_GroundhogControl.instance;
		gc.StaminaDecreasedDelta += OnStaminaDecreased;

		ResetTriesRemaining();
	}

	private void OnDestroy()
	{
		pad.onSubmit.RemoveListener(OnSubmit);
		if (gc)
		{
			gc.StaminaDecreasedDelta -= OnStaminaDecreased;
		}
	}

	private void OnSubmit(string code)
	{
		if (staminaRequiredRemaining <= 0)
		{
		}

		if (code == passcode)
		{
			var dep = GetComponent<Dependable>();
			if (dep)
			{
				dep.firstLiteral = true;
			}

			onCorrectSubmitted.Invoke();

			correctSound.Play();

			padlockSprites.ShowUnlockedImage();
		}
		else
		{
			onIncorrectSubmitted.Invoke();

			incorrectSound.Play();

			padlockSprites.ShowShakeImage();

			--triesRemaining;
			if (triesRemaining <= 0)
			{
				V2_GroundhogControl.instance.Die();
			}
			else
			{
				staminaRequiredRemaining = staminaUntilResetTries;
			}
		}
	}

	void OnStaminaDecreased(float delta)
	{
		if (staminaRequiredRemaining > 0)
		{
			staminaRequiredRemaining -= delta;
			if (staminaRequiredRemaining <= 0)
			{
				ResetTriesRemaining();
			}
		}
	}
}
