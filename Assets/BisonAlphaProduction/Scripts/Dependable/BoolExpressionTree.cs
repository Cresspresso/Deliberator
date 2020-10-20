using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

/// <changelog>
///		<log author="Elijah Shadbolt" date="18/09/2020">
///			<para>Added comments.</para>
///			<para>Removed unused code blocks.</para>
///		</log>
/// </changelog>
/// 
namespace Bison.BoolExpressions.Serializable
{
	/// <summary>
	///		<para>Type of boolean expression AST node.</para>
	///		<para>Used to distinguish between data-oriented arrays of AST nodes.</para>
	/// </summary>
	public enum ExpressionType
	{
		/// <summary>
		/// Boolean literal, either <see langword="true"/> or <see langword="false"/>.
		/// </summary>
		/// <seealso cref="Serializable.Literal"/>
		/// <seealso cref="Arrays.literalArray"/>
		Literal,

		/// <summary>
		/// Object reference to a different <see cref="Dependable"/>.
		/// </summary>
		/// <seealso cref="Serializable.Dependency"/>
		/// <seealso cref="Arrays.dependencyArray"/>
		Dependency,

		/// <summary>
		/// Logical NOT operator (!expr).
		/// </summary>
		/// <seealso cref="Serializable.Not"/>
		/// <seealso cref="Arrays.notArray"/>
		Not,

		/// <summary>
		///	A sequence of operands combined with a binary logical operator.
		/// </summary>
		/// <seealso cref="Serializable.Group"/>
		/// <seealso cref="Arrays.groupArray"/>
		/// <seealso cref="GroupType"/>
		Group,
	}

	/// <summary>
	/// Type of logical operator for combining a sequence of operands
	/// in a <see cref="Group"/> node.
	/// </summary>
	/// <seealso cref="Group"/>
	/// <seealso cref="Arrays.groupArray"/>
	/// <seealso cref="ExpressionType.Group"/>
	public enum GroupType
	{
		/// <summary>
		/// Combines the sequence of operands with logical AND operation: (A && B && C && ... && X)
		/// </summary>
		And,

		/// <summary>
		/// Combines the sequence of operands with logical OR operation: (A || B || C || ... || X)
		/// </summary>
		Or,

		/// <summary>
		/// Combines the sequence of operands with logical Exclusive OR operation: (A != B != C != ... != X)
		/// </summary>
		Xor,
	}

	/// <summary>
	///		<para>Identifier for a node in the abstract syntax tree (AST).</para>
	///		<para>
	///			The data-oriented structure of a <see cref="Tree"/> requires
	///			that there be a way for an AST node to reference
	///			different types of AST nodes from different arrays,
	///			without C# object references.
	///		</para>
	/// </summary>
	/// <seealso cref="Arrays"/>
	/// <seealso cref="Arrays.Visit"/>
	[Serializable]
	public struct ExpressionKey
	{
		/// <summary>
		/// Which data-oriented array the node is in.
		/// </summary>
		/// <seealso cref="Arrays"/>
		public ExpressionType type;

		/// <summary>
		/// The index of the node in the data-oriented array.
		/// </summary>
		public int index;

		public ExpressionKey(ExpressionType type, int index)
		{
			this.type = type;
			this.index = index;
		}

		/// <summary>
		/// Formats the struct as a string.
		/// For example: `ExpressionKey(type: Literal, index: 0)`
		/// </summary>
		public override string ToString() => nameof(ExpressionKey) + "("
			+ nameof(type) + ": " + type.ToString()
			+ ", " + nameof(index) + ": " + index.ToString()
			+ ")";
	}

	/// <summary>
	/// Boolean literal, either <see langword="true"/> or <see langword="false"/>.
	/// </summary>
	/// <seealso cref="ExpressionType.Literal"/>
	/// <seealso cref="Arrays.literalArray"/>
	[Serializable]
	public struct Literal
	{
		/// <summary>
		/// The literal boolean value stored in this AST node.
		/// </summary>
		public bool value;

		public Literal(bool value)
		{
			this.value = value;
		}

		public override string ToString() => value.ToString();
	}

	/// <summary>
	/// Object reference to a different <see cref="Dependable"/>.
	/// </summary>
	/// <seealso cref="ExpressionType.Dependency"/>
	/// <seealso cref="Arrays.dependencyArray"/>
	[Serializable]
	public struct Dependency
	{
		/// <summary>
		/// An object reference to a <see cref="Dependable"/> script.
		/// <para>Should not be null.</para>
		/// </summary>
		public Dependable input;

		public Dependency(Dependable input)
		{
			this.input = input;
		}

		public override string ToString() => '"' + (input ? input.ToString() : "null") + '"';
	}

	/// <summary>
	/// Logical NOT operator (!expr).
	/// </summary>
	/// <seealso cref="ExpressionType.Not"/>
	/// <seealso cref="Arrays.notArray"/>
	[Serializable]
	public struct Not
	{
		/// <summary>
		/// The AST node which should be inverted with the boolean NOT operator.
		/// </summary>
		public ExpressionKey operand;

		public Not(ExpressionKey operand)
		{
			this.operand = operand;
		}

		public override string ToString() => "!" + operand.ToString();
	}

	/// <summary>
	///	A sequence of operands combined with a binary logical operator.
	/// </summary>
	/// <seealso cref="ExpressionType.Group"/>
	/// <seealso cref="Arrays.groupArray"/>
	/// <seealso cref="GroupType"/>
	[Serializable]
	public struct Group
	{
		/// <summary>
		/// The operator to use when evaluating the sequence of operands.
		/// </summary>
		public GroupType type;

		/// <summary>
		///		<para>Children AST nodes of this Group node.</para>
		///		<para>Data oriented identifiers/references.</para>
		///		<para>These are combined with a binary literal operator from left to right.</para>
		/// </summary>
		public List<ExpressionKey> operandSequence;

		public Group(GroupType type, List<ExpressionKey> operandSequence)
		{
			this.type = type;
			this.operandSequence = operandSequence;
		}

		/// <summary>
		/// Gets a string from the type of operator.
		/// </summary>
		public static string GetOperatorString(GroupType type)
		{
			switch (type)
			{
				case GroupType.And: return "&&";
				case GroupType.Or: return "||";
				case GroupType.Xor: return "^";
				default: throw new ArgumentException("Invalid " + nameof(GroupType) + " enum value (" + (int)type + ")", nameof(type));
			}
		}

		public override string ToString() => string.Join(' ' + GetOperatorString(type) + ' ', operandSequence);
	}

	/// <summary>
	///		<para>
	///			A data-oriented Struct-of-Arrays
	///			containing all the AST nodes which could appear
	///			in a boolean expression.
	///		</para>
	///		<para>
	///			The data-oriented approach was chosen because
	///			it works beautifully with Unity's serialization.
	///			It allows prefab instances to have overridden values.
	///		</para>
	/// </summary>
	[Serializable]
	public sealed class Arrays
	{
		/// <summary>
		/// Array of all the <see cref="Literal"/> nodes in the expression.
		/// </summary>
		public Literal[] literalArray = new Literal[1] { new Literal() };

		/// <summary>
		/// Array of all the <see cref="Dependency"/> nodes in the expression.
		/// </summary>
		public Dependency[] dependencyArray = new Dependency[1] { new Dependency() };

		/// <summary>
		/// Array of all the <see cref="Not"/> nodes in the expression.
		/// </summary>
		public Not[] notArray = new Not[0];

		/// <summary>
		/// Array of all the <see cref="Group"/> nodes in the expression.
		/// </summary>
		public Group[] groupArray = new Group[0];

		/// <summary>
		/// A switch statement which executes one function depending on the AST node that is being visited.
		/// </summary>
		/// <param name="key">Which AST node should be visited.</param>
		/// <param name="fnLiteral">The function to invoke if the <paramref name="key"/> is <see cref="ExpressionType.Literal"/>.</param
		/// <param name="fnDependency">The function to invoke if the <paramref name="key"/> is <see cref="ExpressionType.Dependency"/>.</param>
		/// <param name="fnNot">The function to invoke if the <paramref name="key"/> is <see cref="ExpressionType.Not"/>.</param>
		/// <param name="fnGroup">The function to invoke if the <paramref name="key"/> is <see cref="ExpressionType.Group"/>.</param>
		/// <returns>The value returned by the function that was invoked.</returns>
		public R Visit<R>(
			ExpressionKey key,
			Func<Literal, R> fnLiteral,
			Func<Dependency, R> fnDependency,
			Func<Not, R> fnNot,
			Func<Group, R> fnGroup)
		{
			switch (key.type)
			{
				case ExpressionType.Literal: return fnLiteral(literalArray[key.index]);
				case ExpressionType.Dependency: return fnDependency(dependencyArray[key.index]);
				case ExpressionType.Not: return fnNot(notArray[key.index]);
				case ExpressionType.Group: return fnGroup(groupArray[key.index]);
				default: throw new InvalidOperationException(key + ": Invalid " + nameof(ExpressionType) + " enum value (" + (int)key.type + ")");
			}
		}

		/// <summary>
		///		<para>Returns the current boolean value of a boolean expression node.</para>
		///		<para>Dependencies are evaluted with <see cref="Dependable.isPowered"/>.</para>
		/// </summary>
		public bool Evaluate(ExpressionKey key)
		{
			return Visit(key,
				/// If the node is a Literal, return its boolean value.
				literal => literal.value,
				/// If the node is a Dependency, return the current value of its `isPowered` property.
				dependency =>
				{
					var input = dependency.input;
					return input ? input.isPowered : throw new NullReferenceException(key + ": Dependency input is null");
				},
				/// If the node is a Not operator, return the inverse of its operand.
				not => !Evaluate(not.operand),
				/// If the node is a Group node, evaluate its operands.
				group =>
				{
					var seq = group.operandSequence;
					if (seq.Count < 2)
					{
						throw new InvalidOperationException(key + ": Group operandSequence does not have enough elements");
					}
					else
					{
						/// Determine how the operands should be combined.
						Func<bool, ExpressionKey, bool> accumulator;
						switch (group.type)
						{
							case GroupType.And:
								{
									/// For example, (A && B && C && ... && X)
									accumulator = (total, rhs) => total && Evaluate(rhs);
								}
								break;
							case GroupType.Or:
								{
									/// For example, (A || B || C || ... || X)
									accumulator = (total, rhs) => total || Evaluate(rhs);
								}
								break;
							case GroupType.Xor:
								{
									/// For example, ((((A != B) != C) != ...) != X)
									accumulator = (total, rhs) => total ^ Evaluate(rhs);
								}
								break;
							default:
								{
									throw new InvalidOperationException(key + ": Invalid " + nameof(GroupType) + " enum value (" + (int)group.type + ")");
								}
						}
						/// Combine the operands from left to right.
						bool first = Evaluate(seq.First());
						return seq.Skip(1).Aggregate(first, accumulator);
					}
				});
		}

		/// <summary>
		/// Gets a string for the expression represented by an AST node.
		/// </summary>
		public string GetExperssionString(ExpressionKey key)
		{
			try
			{
				return Visit<string>(key,
					literal => literal.value.ToString(),
					dependency => '"' + (dependency.input ? dependency.input.name : "null") + '"',
					not => '!' + GetExperssionString(not.operand),
					group =>
					{
						var sep = ") " + Group.GetOperatorString(group.type) + " (";
						var seq = group.operandSequence.Select(
							operand => GetExperssionString(operand));
						return "((" + string.Join(sep, seq) + "))";
					}
					);
			}
			catch
			{
				return "Error(" + key + ")";
			}
		}

		/// <summary>
		/// Adds the dependencies of a node to a collection.
		/// </summary>
		/// <param name="key">The node to get dependencies of.</param>
		/// <param name="set">A collection to add dependencies to.</param>
		/// <returns>The <paramref name="set"/> parameter.</returns>
		/// <seealso cref="Dependable.GetDependencies"/>
		/// 
		public ICollection<Dependable> GetDependencies(ExpressionKey key, ICollection<Dependable> set)
		{
			return Visit<ICollection<Dependable>>(key,
				literal => set,
				dependency =>
				{
					if (dependency.input)
					{
						set.Add(dependency.input);
					}
					return set;
				},
				not => GetDependencies(not.operand, set),
				group =>
				{
					var exceptions = new List<Exception>();
					foreach (var operandKey in group.operandSequence)
					{
						try
						{
							GetDependencies(operandKey, set);
						}
						catch (Exception e)
						{
							exceptions.Add(e);
						}
					}
					if (exceptions.Count > 0)
					{
						var e = new Exception("One or more group operands threw an exception");
						e.Data.Add("exceptions", exceptions);
						throw e;
					}
					return set;
				}
				);
		}
	}

	/// <summary>
	///		A complete abstract syntax tree (AST) for a boolean expression,
	///		which is serializable in Unity,
	///		and editable in the Inspector,
	///		and can be evaluated at runtime.
	/// </summary>
	[Serializable]
	public sealed class Tree
	{
		/// <summary>
		///		A data-oriented Struct-of-Arrays which has all the AST nodes in this tree, except the root.
		/// </summary>
		public Arrays arrays = new Arrays();

		/// <summary>
		///		Which AST node should be evaulated to give the output of the tree.
		///		<para>A value that represents a reference to an AST node in the <see cref="arrays"/> property.</para>
		/// </summary>
		public ExpressionKey root = new ExpressionKey(ExpressionType.Literal, 0);

		/// <summary>
		///		Computes the boolean value of this boolean expression at this point in time.
		/// </summary>
		public bool Evaluate() => arrays.Evaluate(root);

		/// <summary>
		///		Gets a formatted string for what the expression looks like.
		/// </summary>
		public override string ToString() => arrays.GetExperssionString(root);

		/// <summary>
		///		Adds all <see cref="Dependable"/> references in the tree
		///		(starting from the root node)
		///		to a collection.
		/// </summary>
		/// <param name="set">The colletion to add references to.</param>
		/// <returns>The <paramref name="set"/> parameter.</returns>
		public ICollection<Dependable> GetDependencies(ICollection<Dependable> set) => arrays.GetDependencies(root, set);
	}
}
