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
			And = 0,
			Or = 1,
			Xor = 2,
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

			public override string ToString()
			{
				return "ExpresionKey(type: " + type.ToString() + ", index: " + index.ToString() + ")";
			}
		}

		[Serializable]
		public struct Literal
		{
			public bool value;
		}

		[Serializable]
		public struct Dependency
		{
			public Dependable input;
		}

		[Serializable]
		public struct Not
		{
			public ExpressionKey operand;
		}

		[Serializable]
		public struct Group
		{
			public GroupType type;
			public List<ExpressionKey> operandSequence;
		}

		[Serializable]
		public sealed class Tree
		{
			public ExpressionKey root;
			public Literal[] literalArray;
			public Dependency[] dependencyArray;
			public Not[] notArray;
			public Group[] groupArray;

			public Tree()
			{
				literalArray = new Literal[1] { new Literal() };
				literalArray[0].value = true;
				root = new ExpressionKey(ExpressionType.Literal, 0);
			}
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
