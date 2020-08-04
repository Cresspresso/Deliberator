using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.ComponentModel;

namespace Bison.BoolExpressions
{
	namespace Serializable
	{
		public enum ExpressionType
		{
			Literal,
			Dependency,
			Not,
			Group,
		}

		public enum GroupType
		{
			And,
			Or,
			Xor,
		}

		[Serializable]
		public struct ExpressionKey
		{
			public ExpressionType type;
			public int index;

			public ExpressionKey(ExpressionType type, int index)
			{
				this.type = type;
				this.index = index;
			}

			public override string ToString() => "ExpresionKey(type: " + type.ToString() + ", index: " + index.ToString() + ")";
		}

		[Serializable]
		public struct Literal
		{
			public bool value;

			public override string ToString() => value.ToString();
		}

		[Serializable]
		public struct Dependency
		{
			public Dependable input;

			public override string ToString() => '"' + (input ? input.ToString() : "null") + '"';
		}

		[Serializable]
		public struct Not
		{
			public ExpressionKey operand;

			public override string ToString() => "!" + operand.ToString();
		}

		[Serializable]
		public struct Group
		{
			public GroupType type;
			public List<ExpressionKey> operandSequence;

			public static string GetOperatorString(GroupType type)
			{
				switch (type)
				{
					case GroupType.And: return "&&";
					case GroupType.Or: return "||";
					case GroupType.Xor: return "^";
					default: throw new ArgumentException("Invalid " + nameof(GroupType) + " enum value", nameof(type));
				}
			}

			public override string ToString() => string.Join(' ' + GetOperatorString(type) + ' ', operandSequence);
		}

		[Serializable]
		public sealed class Arrays
		{
			public Literal[] literalArray = new Literal[0];
			public Dependency[] dependencyArray = new Dependency[1] { new Dependency() };
			public Not[] notArray = new Not[0];
			public Group[] groupArray = new Group[0];

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
					default: throw new InvalidOperationException("Invalid " + nameof(ExpressionType) + " enum value");
				}
			}

			public bool Evaluate(ExpressionKey key)
			{
				return Visit(key,
					literal => literal.value,
					dependency =>
					{
						var input = dependency.input;
						return input ? input.Evaluate() : throw new NullReferenceException("Dependency " + key + " is null");
					},
					not => Evaluate(not.operand),
					group =>
					{
						var seq = group.operandSequence;
						if (seq.Count < 2)
						{
							throw new InvalidOperationException("Group operandSequence does not have enough elements");
						}
						else
						{
							Func<bool, ExpressionKey, bool> accumulator;
							switch (group.type)
							{
								case GroupType.And:
									{
										accumulator = (total, rhs) => total && Evaluate(rhs);
									}
									break;
								case GroupType.Or:
									{
										accumulator = (total, rhs) => total || Evaluate(rhs);
									}
									break;
								case GroupType.Xor:
									{
										accumulator = (total, rhs) => total ^ Evaluate(rhs);
									}
									break;
								default:
									{
										throw new InvalidOperationException("Invalid " + nameof(GroupType) + " enum value");
									}
							}
							bool first = Evaluate(seq.First());
							return seq.Skip(1).Aggregate(first, accumulator);
						}
					});
			}

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
		}

		[Serializable]
		public sealed class Tree
		{
			public Arrays arrays = new Arrays();
			public ExpressionKey root = new ExpressionKey(ExpressionType.Dependency, 0);

			public bool Evaluate() => arrays.Evaluate(root);
			public override string ToString() => arrays.GetExperssionString(root);
		}
	}

#if false
	namespace Referential
	{
		public abstract class AExpression
		{
			public int ID;
		}

		public sealed class Literal : AExpression
		{
			public bool value;
		}

		public sealed class Dependency : AExpression
		{
			public Dependable input;
		}

		public sealed class Not : AExpression
		{
			public AExpression operand;
		}

		public sealed class Group : AExpression
		{
			public GroupType type;
			public List<AExpression> operandSequence;
		}
	}

	namespace RuntimeReadOnly
	{
		public interface IExpression
		{
			bool Evaluate();
		}

		public sealed class Literal : IExpression
		{
			public readonly bool value;

			public Literal(bool value)
			{
				this.value = value;
			}

			public bool Evaluate() => value;

			public override string ToString() => value.ToString();
		}

		public sealed class Dependency : IExpression
		{
			public readonly Dependable input; // nullable

			public Dependency(Dependable input)
			{
				this.input = input;
			}

			public bool Evaluate() => input ? input.Evaluate() : false;

			public override string ToString() => "Dependency(" + (input ? input.name : "null") + ")";
		}

		public sealed class Not : IExpression
		{
			public readonly IExpression operand;

			public Not(IExpression operand)
			{
				if (operand == null)
				{
					throw new ArgumentNullException(nameof(operand));
				}
				this.operand = operand;
			}

			public bool Evaluate() => !operand.Evaluate();

			public override string ToString() => "!(" + operand.ToString() + ")";
		}

		public sealed class Group : IExpression
		{
			public readonly GroupType type;
			public readonly IReadOnlyList<IExpression> operandSequence;

			public Group(GroupType type, IReadOnlyList<IExpression> operandSequence)
			{
				if (type != GroupType.And
					&& type != GroupType.Or
					&& type != GroupType.Xor)
				{
					throw new InvalidEnumArgumentException(nameof(type), (int)type, typeof(GroupType));
				}
				if (operandSequence == null)
				{
					throw new ArgumentNullException(nameof(operandSequence));
				}
				if (operandSequence.Count < 2)
				{
					throw new ArgumentException(nameof(operandSequence) + " does not have enough elements");
				}
				if (operandSequence.Any(o => o == null))
				{
					throw new ArgumentNullException(nameof(operandSequence), nameof(operandSequence) + " must not contain nulls");
				}
				this.type = type;
				this.operandSequence = operandSequence;
			}

			public bool Evaluate()
			{
				switch (type)
				{
					case GroupType.And: return operandSequence.All(o => o.Evaluate());
					case GroupType.Or: return operandSequence.Any(o => o.Evaluate());
					case GroupType.Xor:
						bool r = operandSequence[0].Evaluate() ^ operandSequence[1].Evaluate();
						for (int i = 2; i < operandSequence.Count; ++i)
						{
							r = r ^ operandSequence[i].Evaluate();
						}
						return r;
					default: throw new InvalidEnumArgumentException(nameof(type), (int)type, typeof(GroupType));
				}
			}

			public override string ToString()
			{
				var seq = operandSequence.Select(o => "(" + o.ToString() + ")");
				switch (type)
				{
					case GroupType.And: return string.Join(" && ", seq);
					case GroupType.Or: return string.Join(" || ", seq);
					case GroupType.Xor: return string.Join(" ^ ", seq);
					default: return "InvalidGroupType";
				}
			}
		}
	}

	[Serializable]
	public class BoolExpressionTree
	{
		[SerializeField]
		private Serializable.Tree m_tree;

		public RuntimeReadOnly.IExpression expression { get; private set; }

		public RuntimeReadOnly.IExpression FetchExpression() => FetchExpression(m_tree.rootID);
		public RuntimeReadOnly.IExpression FetchExpression(int id)
		{
			foreach (var literal in m_tree.literalNodes)
			{
				if (literal.ID == id)
				{
					return new RuntimeReadOnly.Literal(literal.value);
				}
			}
			foreach (var dependency in m_tree.dependencyNodes)
			{
				if (dependency.ID == id)
				{
					return new RuntimeReadOnly.Dependency(dependency.input);
				}
			}
			foreach (var not in m_tree.notNodes)
			{
				if (not.ID == id)
				{
					return new RuntimeReadOnly.Not(FetchExpression(not.operandID));
				}
			}
			foreach (var group in m_tree.groupNodes)
			{
				if (group.ID == id)
				{
					return new RuntimeReadOnly.Group(
						group.type,
						group.operandSequenceIDs.Select(
						operandID => FetchExpression(operandID)
						).ToList());
				}
			}
			throw new FormatException("ID not found: " + id);
		}

		public bool Evaluate() => expression.Evaluate();
	}
#endif
}
