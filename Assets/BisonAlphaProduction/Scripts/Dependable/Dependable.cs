using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Tree = Bison.BoolExpressions.Serializable.Tree;

public sealed class Dependable : MonoBehaviour
{
	public Tree condition = new Tree();

	public bool isPowered { get; private set; } = false;

	public class UnityEvent_bool : UnityEvent<bool> { }
	[SerializeField]
	private UnityEvent_bool m_onChanged = new UnityEvent_bool();
	public UnityEvent_bool onChanged => m_onChanged;

	private void Start()
	{
		try
		{
			isPowered = condition.Evaluate();
		}
		catch (System.Exception e)
		{
			Debug.LogException(e, this);
		}
		onChanged.Invoke(isPowered);
	}

	private void Update()
	{
		try
		{
			var newValue = condition.Evaluate();
			if (newValue != isPowered)
			{
				isPowered = newValue;
				onChanged.Invoke(newValue);
			}
		}
		catch (System.Exception e)
		{
			Debug.LogException(e, this);
		}
	}
}
