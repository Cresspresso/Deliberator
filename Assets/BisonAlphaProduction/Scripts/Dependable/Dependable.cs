using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
		catch (Exception e)
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
		catch (Exception e)
		{
			Debug.LogException(e, this);
		}
	}

	public bool firstLiteral {
		get => condition.arrays.literalArray[0].value;
		set => condition.arrays.literalArray[0] = new Bison.BoolExpressions.Serializable.Literal(value);
	}

	public List<Dependable> GetDependencies()
	{
		var set = new List<Dependable>();
		try
		{
			condition.GetDependencies(set);
		}
		catch (Exception e)
		{
			Debug.LogException(e, this);
			if (e.Data.Contains("exceptions"))
			{
				if (e.Data["exceptions"] is IEnumerable<Exception> exceptions)
				{
					foreach (var innerException in exceptions)
					{
						Debug.LogException(innerException, this);
					}
				}
			}
		}
		return set;
	}

	public List<T> GetDependencyComponents<T>() where T : Component
	{
		return GetDependencies()
			.Select(d => d.GetComponent<T>())
			.Where(c => c)
			.ToList();
	}
}
