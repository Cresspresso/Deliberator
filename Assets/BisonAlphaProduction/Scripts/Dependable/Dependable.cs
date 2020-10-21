using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using Tree = Bison.BoolExpressions.Serializable.Tree;

/// <summary>
///		<para>A component that can be used to determine if the object is powered on or off.</para>
///		<para>Has a simple boolean expression which is editable in the Inspector.</para>
///		<para>This is useful when you don't want to write tons of small MonoBehaviour scripts just for different boolean expressions.</para>
/// </summary>
/// 
/// <changelog>
///		<log author="Elijah Shadbolt" date="18/09/2020">
///			<para>Added comments.</para>
///		</log>
/// </changelog>
/// 
public sealed class Dependable : MonoBehaviour
{
	/// <summary>
	///		<para>The boolean expression.</para>
	/// </summary>
	public Tree condition = new Tree();

	/// <summary>
	///		<para>True iff <see cref="condition"/> is true.</para>
	///		<para>This property is updated in the <see cref="Update"/> event.</para>
	/// </summary>
	public bool isPowered { get; private set; } = false;

	/// <summary>
	///		<para>Concrete specialization of <see cref="UnityEvent{T0}"/> for <see cref="bool"/>.</para>
	/// </summary>
	public class UnityEvent_bool : UnityEvent<bool> { }

	[Tooltip("Event triggered after the property isPowered is changed"
		+ " (from true to false or from false to true)."
		+ "\nThe argument is the property isPowered.")]
	[SerializeField]
	private UnityEvent_bool m_onChanged = new UnityEvent_bool();

	/// <summary>
	///		<para>
	///			Event triggered after the property <see cref="isPowered"/> is changed
	///			(from true to false or from false to true).
	///		</para>
	///		<para>
	///			The argument is the property <see cref="isPowered"/>.
	///		</para>
	/// </summary>
	public UnityEvent_bool onChanged => m_onChanged;

	/// <summary>
	///		<para>Unity event method.</para>
	/// </summary>
	private void Start()
	{
		/// Evaluate isPowered for the first time.
		try
		{
			isPowered = condition.Evaluate();
		}
		catch (Exception e)
		{
			Debug.LogException(e, this);
		}
		/// Invoke onChanged for the first time.
		onChanged.Invoke(isPowered);
	}

	public bool ReEvaluate()
	{
		/// Check if the outcome of the boolean expression has changed.
		try
		{
			var newValue = condition.Evaluate();
			if (newValue != isPowered)
			{
				/// Change the value of isPowered.
				isPowered = newValue;
				onChanged.Invoke(newValue);
			}
		}
		catch (Exception e)
		{
			Debug.LogException(e, this);
		}
		return isPowered;
	}

	/// <summary>
	///		<para>Unity event method.</para>
	/// </summary>
	private void Update()
	{
		ReEvaluate();
	}

	/// <summary>
	///		<para>
	///			A shortcut to get or set the value of the first
	///			boolean literal of the array of literals in the boolean condition expression.
	///		</para>
	///		<para>
	///			If there are no literals in the array, it will throw an exception.
	///		</para>
	/// </summary>
	public bool firstLiteral {
		get => condition.arrays.literalArray[0].value;
		set => condition.arrays.literalArray[0] = new Bison.BoolExpressions.Serializable.Literal(value);
	}

	public bool hasFirstLiteral => condition.arrays.literalArray.Length > 0;

	/// <summary>
	///		<para>
	///			Gets a list of all the <see cref="Dependable"/>
	///			references in the <see cref="condition"/> expression.
	///		</para>
	///		<para>Duplicates are included.</para>
	///		<para>Should not throw. Exceptions are logged with <see cref="Debug.LogException(Exception, UnityEngine.Object)"/>.</para>
	/// </summary>
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

	/// <summary>
	///		<para>
	///			Gets a list of components of type <typeparamref name="T"/>
	///			attached to the <see cref="Dependable"/> references
	///			found in the <see cref="condition"/> expression.
	///		</para>
	///		<para>Duplicate <see cref="Dependable"/> references are included.</para>
	///		<para>Components are retrieved with <see cref="Component.GetComponent{T}"/>.</para>
	///		<para>Should not throw. Exceptions are logged with <see cref="Debug.LogException(Exception, UnityEngine.Object)"/>.</para>
	/// </summary>
	/// <typeparam name="T">Type of component to get from each <see cref="Dependable"/> reference.</typeparam>
	public List<T> GetDependencyComponents<T>() where T : Component
	{
		return GetDependencies()
			.Select(d => d.GetComponent<T>())
			.Where(c => c)
			.ToList();
	}
}
