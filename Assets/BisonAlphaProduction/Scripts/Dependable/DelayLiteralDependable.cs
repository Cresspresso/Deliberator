using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Bison.BoolExpressions.Serializable;

/// <summary>
/// When another Dependable is powered,
/// this script sets the first Literal of its own Dependable to True,
/// then waits for a time,
/// then sets it to False.
/// </summary>
[RequireComponent(typeof(Dependable))]
public sealed class DelayLiteralDependable : MonoBehaviour
{
	public Dependable output { get; private set; }

#pragma warning disable CS0649
	[SerializeField]
	private Dependable m_input;
#pragma warning restore CS0649
	public Dependable input => m_input;

	public float duration = 1.0f;
	public float timeRemaining { get; private set; }

	public class UnityEvent_float : UnityEngine.Events.UnityEvent<float> { }
	[SerializeField]
	private UnityEvent_float m_onTimeRemainingChanged = new UnityEvent_float();
	public UnityEvent_float onTimeRemainingChanged => m_onTimeRemainingChanged;
	private void InvokeOnTimeRemainingChanged() => onTimeRemainingChanged.Invoke(timeRemaining);

	private void Awake()
	{
		Debug.Assert(m_input, "Input is null", this);
		m_input.onChanged.AddListener(OnChanged);

		output = GetComponent<Dependable>();
	}

	private void Start()
	{
		InvokeOnTimeRemainingChanged();
	}

	private void Update()
	{
		if (timeRemaining > 0)
		{
			timeRemaining -= Time.deltaTime;
			if (timeRemaining <= 0)
			{
				StopTimer();
			}
			else
			{
				InvokeOnTimeRemainingChanged();
			}
		}
	}

	private void StartTimer()
	{
		output.firstLiteral = true;
		timeRemaining = duration;
		InvokeOnTimeRemainingChanged();
	}

	public void StopTimer()
	{
		output.firstLiteral = false;
		timeRemaining = 0;
		InvokeOnTimeRemainingChanged();
	}

	private void OnChanged(bool isPowered)
	{
		if (isPowered)
		{
			StartTimer();
		}
		else
		{
			StopTimer();
		}
	}
}
