using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using ExpressionKey = Bison.BoolExpressions.Serializable.ExpressionKey;
using ExpressionType = Bison.BoolExpressions.Serializable.ExpressionType;
using GroupType = Bison.BoolExpressions.Serializable.GroupType;
using UnityEditor.Rendering;
using ICSharpCode.NRefactory.Ast;
using System.Runtime.CompilerServices;
using System.Linq;

namespace Bison.BoolExpressions.Editor
{
	[CustomPropertyDrawer(typeof(ExpressionKey))]
	public class ExpressionKeyPropertyDrawer : PropertyDrawer
	{
		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
			=> new Props(property).GetPropertyHeight(label);

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
			=> new Props(property).OnGUI(position, label);

		public class Props
		{
			public Props(SerializedProperty property)
			{
				this.property = property;
				type = property.FindPropertyRelative(nameof(ExpressionKey.type));
				index = property.FindPropertyRelative(nameof(ExpressionKey.index));
			}

			public readonly SerializedProperty
				property,
				type,
				index;

			public ExpressionKey structValue => new ExpressionKey(
				type: (ExpressionType)type.enumValueIndex,
				index: index.intValue);

			public static float horizontalPadding = 2.0f;

			public float GetPropertyHeight(GUIContent label)
			{
				return EditorGUI.GetPropertyHeight(property, label, false);
			}

			public void OnGUI(Rect position, GUIContent label)
			{
				position = EditorGUI.PrefixLabel(position, label);
				var old = EditorGUI.indentLevel;
				EditorGUI.indentLevel = 0;
				position.width *= 0.5f;
				position.width -= horizontalPadding * 0.5f;
				EditorGUI.LabelField(position, new GUIContent("", "Expression Type"));
				EditorGUI.PropertyField(position, type, GUIContent.none);
				position.x += position.width + horizontalPadding;
				EditorGUI.LabelField(position, new GUIContent("", "Index"));
				EditorGUI.PropertyField(position, index, GUIContent.none);
				EditorGUI.indentLevel = old;
			}
		}
	}

	namespace ExpressionProps
	{
		public class Literal
		{
			public Literal(SerializedProperty property)
			{
				this.property = property;
				pValue = property.FindPropertyRelative(nameof(Serializable.Literal.value));
			}

			public readonly SerializedProperty
				property,
				pValue;

			public bool value => pValue.boolValue;

			public Serializable.Literal structValue => new Serializable.Literal
			{
				value = value,
			};
		}

		public class Dependency
		{
			public Dependency(SerializedProperty property)
			{
				this.property = property;
				pInput = property.FindPropertyRelative(nameof(Serializable.Dependency.input));
			}

			public readonly SerializedProperty
				property,
				pInput;

			public Dependable input => pInput.objectReferenceValue as Dependable;

			public Serializable.Dependency structValue => new Serializable.Dependency
			{
				input = input,
			};
		}

		public class Not
		{
			public Not(SerializedProperty property)
			{
				this.property = property;
				qOperand = new ExpressionKeyPropertyDrawer.Props(property.FindPropertyRelative(nameof(Serializable.Not.operand)));
			}

			public readonly SerializedProperty property;
			public readonly ExpressionKeyPropertyDrawer.Props qOperand;

			public ExpressionKey operand => qOperand.structValue;

			public Serializable.Not structValue => new Serializable.Not
			{
				operand = operand,
			};
		}

		public class Group
		{
			public Group(SerializedProperty property)
			{
				this.property = property;
				pType = property.FindPropertyRelative(nameof(Serializable.Group.type));
				pOperandSequence = property.FindPropertyRelative(nameof(Serializable.Group.operandSequence));
			}

			public readonly SerializedProperty
				property,
				pType,
				pOperandSequence;

			public GroupType type => (GroupType)pType.enumValueIndex;
			public List<ExpressionKey> operandSequence {
				get
				{
					int c = pOperandSequence.arraySize;
					var ret = new List<ExpressionKey>(c);
					for (int i = 0; i < c; ++i)
					{
						var operand = GetOperandPropsAtIndex(i);
						ret.Add(operand.structValue);
					}
					return ret;
				}
			}

			public ExpressionKeyPropertyDrawer.Props GetOperandPropsAtIndex(int i)
			{
				return new ExpressionKeyPropertyDrawer.Props(pOperandSequence.GetArrayElementAtIndex(i));
			}

			public Serializable.Group structValue => new Serializable.Group
			{
				type = type,
				operandSequence = operandSequence,
			};
		}
	}

	[CustomPropertyDrawer(typeof(Serializable.Tree))]
	public class TreePropertyDrawer : PropertyDrawer
	{
		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
			=> new Props(property).GetPropertyHeight(label);

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
			=> new Props(property).OnGUI(position, label);

		class Props
		{
			public Props(SerializedProperty property)
			{
				this.property = property;
				qRoot = new ExpressionKeyPropertyDrawer.Props(property.FindPropertyRelative(nameof(Serializable.Tree.root)));
				pLiteralArray = property.FindPropertyRelative(nameof(Serializable.Tree.literalArray));
				pDependencyArray = property.FindPropertyRelative(nameof(Serializable.Tree.dependencyArray));
				pNotArray = property.FindPropertyRelative(nameof(Serializable.Tree.notArray));
				pGroupArray = property.FindPropertyRelative(nameof(Serializable.Tree.groupArray));
			}

			public readonly SerializedProperty
				property,
				pLiteralArray,
				pDependencyArray,
				pNotArray,
				pGroupArray;

			public readonly ExpressionKeyPropertyDrawer.Props qRoot;

			public const float verticalSpace = 10.0f;
			public static float helpBoxHeight => EditorGUIUtility.singleLineHeight * 2;

			public static readonly GUIContent rootLabel = new GUIContent("Root");

			public static readonly GUIContent expressionNotOperandLabel = new GUIContent("Not");

			public static readonly GUIContent expressionGroupTypeAndLabel = new GUIContent("And");
			public static readonly GUIContent expressionGroupTypeOrLabel = new GUIContent("Or");
			public static readonly GUIContent expressionGroupTypeXorLabel = new GUIContent("Xor");

			public bool TryGetExpressionArray(ExpressionType type, out SerializedProperty pArray)
			{
				switch (type)
				{
					case ExpressionType.Literal:
						pArray = pLiteralArray;
						return true;
					case ExpressionType.Dependency:
						pArray = pDependencyArray;
						return true;
					case ExpressionType.Not:
						pArray = pNotArray;
						return true;
					case ExpressionType.Group:
						pArray = pGroupArray;
						return true;
					default:
						pArray = null;
						return false;
				}
			}

			public enum GetExpressionPropertyError
			{
				Success,
				InvalidEnumExpressionType,
				IndexOutOfRange,
			}

			public bool TryGetExpressionProperty(
				ExpressionKey key,
				out SerializedProperty pExpression,
				out GetExpressionPropertyError error)
			{
				if (TryGetExpressionArray(key.type, out var pArray))
				{
					if (key.index < 0 || key.index >= pArray.arraySize)
					{
						pExpression = null;
						error = GetExpressionPropertyError.IndexOutOfRange;
						return false;
					}
					else
					{
						pExpression = pArray.GetArrayElementAtIndex(key.index);
						error = GetExpressionPropertyError.Success;
						return true;
					}
				}
				else
				{
					pExpression = null;
					error = GetExpressionPropertyError.InvalidEnumExpressionType;
					return false;
				}
			}

			public static R VisitExpressionProps1<R>(
				ExpressionType type,
				SerializedProperty pExpression,
				Func<ExpressionProps.Literal, R> fnLiteral,
				Func<ExpressionProps.Dependency, R> fnDependency,
				Func<ExpressionProps.Not, R> fnNot,
				Func<ExpressionProps.Group, R> fnGroup,
				Func<R> fnError)
			{
				switch (type)
				{
					case ExpressionType.Literal: return fnLiteral(new ExpressionProps.Literal(pExpression));
					case ExpressionType.Dependency: return fnDependency(new ExpressionProps.Dependency(pExpression));
					case ExpressionType.Not: return fnNot(new ExpressionProps.Not(pExpression));
					case ExpressionType.Group: return fnGroup(new ExpressionProps.Group(pExpression));
					default: return fnError();
				}
			}

			public R VisitExpressionProps<R>(
				ExpressionKey key,
				Func<ExpressionProps.Literal, R> fnLiteral,
				Func<ExpressionProps.Dependency, R> fnDependency,
				Func<ExpressionProps.Not, R> fnNot,
				Func<ExpressionProps.Group, R> fnGroup,
				Func<GetExpressionPropertyError, R> fnError)
			{
				if (TryGetExpressionProperty(key, out var pExpression, out var error))
				{
					return VisitExpressionProps1(key.type, pExpression,
						fnLiteral,
						fnDependency,
						fnNot,
						fnGroup,
						() => throw new InvalidOperationException("Internal Error"));
				}
				else
				{
					return fnError(error);
				}
			}

			public float GetExpressionHeight(
				ExpressionKey key,
				GUIContent label,
				HashSet<ExpressionKey> visitedKeys)
			{
				if (false == visitedKeys.Add(key))
				{
					return helpBoxHeight;
				}

				try
				{
					return VisitExpressionProps(key,
						qLiteral =>
						{
							float height = EditorGUIUtility.standardVerticalSpacing;
							height += EditorGUI.GetPropertyHeight(qLiteral.pValue);
							return height;
						},
						qDependency =>
						{
							float height = EditorGUIUtility.standardVerticalSpacing;
							height += EditorGUI.GetPropertyHeight(qDependency.pInput);
							return height;
						},
						qNot =>
						{
							float height = EditorGUIUtility.standardVerticalSpacing;
							height += qNot.qOperand.GetPropertyHeight(expressionNotOperandLabel);
							height += GetExpressionHeight(qNot.operand, expressionNotOperandLabel, visitedKeys);
							return height;
						},
						qGroup =>
						{
							float height = EditorGUIUtility.standardVerticalSpacing;
							switch (qGroup.type)
							{
								case GroupType.And:
									{
										height += EditorGUI.GetPropertyHeight(qGroup.pType, expressionGroupTypeAndLabel, true);
									}
									break;
								case GroupType.Or:
									{
										height += EditorGUI.GetPropertyHeight(qGroup.pType, expressionGroupTypeOrLabel, true);
									}
									break;
								case GroupType.Xor:
									{
										height += EditorGUI.GetPropertyHeight(qGroup.pType, expressionGroupTypeXorLabel, true);
									}
									break;
								default:
									{
										height += helpBoxHeight;
									}
									break;
							}

							height += EditorGUI.GetPropertyHeight(qGroup.pOperandSequence);

							return height;
						},
						error =>
						{
							return helpBoxHeight;
						}
					);
				}
				finally
				{
					visitedKeys.Remove(key);
				}
			}

			public void OnExpressionGUI(
				Rect position,
				ExpressionKey key,
				GUIContent label,
				HashSet<ExpressionKey> visitedKeys)
			{
				if (false == visitedKeys.Add(key))
				{
					EditorGUI.HelpBox(position, "Recursive expressions are not allowed. Change the index.", MessageType.Error);
					return;
				}

				++EditorGUI.indentLevel;
				try
				{
					VisitExpressionProps<object>(key,
						qLiteral =>
						{
							position.y += EditorGUIUtility.standardVerticalSpacing;
							position.height -= EditorGUIUtility.standardVerticalSpacing;
							EditorGUI.PropertyField(position, qLiteral.pValue, true);
							return null;
						},
						qDependency =>
						{
							position.y += EditorGUIUtility.standardVerticalSpacing;
							position.height -= EditorGUIUtility.standardVerticalSpacing;
							EditorGUI.PropertyField(position, qDependency.pInput, true);
							return null;
						},
						qNot =>
						{
							position.y += EditorGUIUtility.standardVerticalSpacing;
							position.height = qNot.qOperand.GetPropertyHeight(expressionNotOperandLabel);
							qNot.qOperand.OnGUI(position, expressionNotOperandLabel);
							position.y += position.height;

							position.height = GetExpressionHeight(qNot.operand, expressionNotOperandLabel, visitedKeys);
							OnExpressionGUI(position, qNot.operand, expressionNotOperandLabel, visitedKeys);
							return null;
						},
						qGroup =>
						{
							position.y += EditorGUIUtility.standardVerticalSpacing;
							switch (qGroup.type)
							{
								case GroupType.And:
									{
										position.height = EditorGUI.GetPropertyHeight(qGroup.pType, expressionGroupTypeAndLabel, true);
										EditorGUI.PropertyField(position, qGroup.pType, expressionGroupTypeAndLabel, true);
									}
									break;
								case GroupType.Or:
									{
										position.height = EditorGUI.GetPropertyHeight(qGroup.pType, expressionGroupTypeOrLabel, true);
										EditorGUI.PropertyField(position, qGroup.pType, expressionGroupTypeOrLabel, true);
									}
									break;
								case GroupType.Xor:
									{
										position.height = EditorGUI.GetPropertyHeight(qGroup.pType, expressionGroupTypeXorLabel, true);
										EditorGUI.PropertyField(position, qGroup.pType, expressionGroupTypeXorLabel, true);
									}
									break;
								default:
									{
										position.height = helpBoxHeight;
										EditorGUI.HelpBox(position, "Invalid group type", MessageType.Error);
									}
									break;
							}
							position.y += position.height;

							++EditorGUI.indentLevel;
							try
							{
								position.height = EditorGUI.GetPropertyHeight(qGroup.pOperandSequence);
								EditorGUI.PropertyField(position, qGroup.pOperandSequence, true);
							}
							finally
							{
								--EditorGUI.indentLevel;
							}

							return null;
						},
						error =>
						{
							switch (error)
							{
								case GetExpressionPropertyError.IndexOutOfRange:
									EditorGUI.HelpBox(position, "Index out of range", MessageType.Error);
									break;
								case GetExpressionPropertyError.InvalidEnumExpressionType:
									EditorGUI.HelpBox(position, "Invalid expression type", MessageType.Error);
									break;
								default:
									EditorGUI.HelpBox(position, "Internal error", MessageType.Error);
									throw new InvalidOperationException("Internal error");
							}
							return null;
						}
					);
				}
				finally
				{
					--EditorGUI.indentLevel;
					visitedKeys.Remove(key);
				}
			}

			public string GetExpressionString(
				ExpressionKey key,
				HashSet<ExpressionKey> visitedKeys)
			{
				if (false == visitedKeys.Add(key))
				{
					return "Error(Infinite Recursion)";
				}

				try
				{
					return VisitExpressionProps(key,
						qLiteral => qLiteral.value.ToString(),
						qDependency =>
						{
							var input = qDependency.input;
							return "Dep(" + (input ? input.name : "None") + ")";
						},
						qNot => "!" + GetExpressionString(qNot.operand, visitedKeys),
						qGroup =>
						{
							int i = (int)qGroup.type;
							var strings = new string[] {
								"&&",
								"||",
								"^",
							};
							if (i < 0 || i >= strings.Length)
							{
								return "Error(Invalid Group Type)";
							}
							else
							{
								return "("
								+ string.Join(
									") " + strings[i] + " (",
									qGroup.operandSequence.Select(operandKey =>
								GetExpressionString(operandKey, visitedKeys)))
								+ ")";
							}
						},
						error => "Error(" + error.ToString() + ")"
						);
				}
				finally
				{
					visitedKeys.Remove(key);
				}
			}

			public float GetPropertyHeight(GUIContent label)
			{
				float height = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

				height += qRoot.GetPropertyHeight(rootLabel);
				height += GetExpressionHeight(qRoot.structValue, rootLabel, new HashSet<ExpressionKey>());

				height += verticalSpace;

				height += EditorGUI.GetPropertyHeight(property, label, true);

				return height;
			}

			public void OnGUI(Rect position, GUIContent label)
			{
				position.height = EditorGUIUtility.singleLineHeight;
				string msg = GetExpressionString(qRoot.structValue, new HashSet<ExpressionKey>());
				EditorGUI.LabelField(position, "Expression", msg);
				position.y += position.height + EditorGUIUtility.standardVerticalSpacing;

				position.height = qRoot.GetPropertyHeight(rootLabel);
				qRoot.OnGUI(position, rootLabel);
				position.y += position.height;

				var visitedKeys = new HashSet<ExpressionKey>();
				position.height = GetExpressionHeight(qRoot.structValue, rootLabel, visitedKeys);
				OnExpressionGUI(position, qRoot.structValue, rootLabel, visitedKeys);
				position.y += position.height;

				position.y += verticalSpace;

				position.height = EditorGUI.GetPropertyHeight(property, label, true);
				EditorGUI.PropertyField(position, property, true);
				position.y += position.height;
			}
		}
	}

#if false
	namespace P2
	{
		sealed class Tree
		{
			public readonly SerializedProperty
				property,
				root,
				literalArray,
				dependencyArray,
				notArray,
				groupArray;

			public Tree(SerializedProperty property)
			{
				this.property = property;
				root = property.FindPropertyRelative(nameof(Serializable.Tree.root));
				literalArray = property.FindPropertyRelative(nameof(Serializable.Tree.literalArray));
				dependencyArray = property.FindPropertyRelative(nameof(Serializable.Tree.dependencyArray));
				notArray = property.FindPropertyRelative(nameof(Serializable.Tree.notArray));
				groupArray = property.FindPropertyRelative(nameof(Serializable.Tree.groupArray));
			}

			public static float helpBoxHeight => EditorGUIUtility.singleLineHeight * 2;
			public static void HelpBoxKeyNotFoundError(Rect position, ExpressionKey key)
			{
				EditorGUI.HelpBox(position, key.ToString() + " not found in Tree", MessageType.Error);
			}

			public SerializedProperty Find(ExpressionKey key)
			{
				SerializedProperty array;
				switch (key.type)
				{
					case ExpressionType.Literal: array = literalArray; break;
					case ExpressionType.Dependency: array = dependencyArray; break;
					case ExpressionType.Not: array = notArray; break;
					case ExpressionType.Group: array = groupArray; break;
					default: return null;
				}
				return array.GetArrayElementAtIndex(key.index);
			}

			public float GetHeight(GUIContent label)
			{
				return EditorGUI.GetPropertyHeight(property, label, false)
					+ EditorGUI.GetPropertyHeight(root, true);
			}

			public void OnGUI(Rect position, GUIContent label)
			{
				label = new GUIContent(label);
				EditorGUI.PropertyField(position, property, false);
			}
		}
	}

	namespace Properties
	{
		sealed class Tree
		{
			public readonly SerializedProperty
				root,
				literalArray,
				dependencyArray,
				notArray,
				groupArray;

			public Tree(SerializedProperty property)
			{
				root = property.FindPropertyRelative(nameof(Serializable.Tree.root));
				literalArray = property.FindPropertyRelative(nameof(Serializable.Tree.literalArray));
				dependencyArray = property.FindPropertyRelative(nameof(Serializable.Tree.dependencyArray));
				notArray = property.FindPropertyRelative(nameof(Serializable.Tree.notArray));
				groupArray = property.FindPropertyRelative(nameof(Serializable.Tree.groupArray));
			}

			public const float spaceMargin = 10;
			public const float padding = 2.0f;
			public static float errorHeight => EditorGUIUtility.singleLineHeight * 2;

			public static void HelpBoxKeyNotFoundError(Rect position, ExpressionKey key)
			{
				EditorGUI.HelpBox(position, key.ToString() + " not found in Expression Tree", MessageType.Error);
			}

			private bool FindID(int id, SerializedProperty pArray, out SerializedProperty pExpression)
			{
				for (int i = 0; i < pArray.arraySize; ++i)
				{
					var pElement = pArray.GetArrayElementAtIndex(i);
					var pID = pElement.FindPropertyRelative(nameof(Serializable.AExpression.ID));
					if (pID.intValue == id)
					{
						pExpression = pElement;
						return true;
					}
				}
				pExpression = null;
				return false;
			}

			public bool FindID(int id, out Expression expressionProps)
			{
				SerializedProperty pExpression;
				if (FindID(id, literalNodes, out pExpression))
				{
					expressionProps = new Literal(this, pExpression);
					return true;
				}
				else if (FindID(id, dependencyNodes, out pExpression))
				{
					expressionProps = new Dependency(this, pExpression);
					return true;
				}
				else if (FindID(id, notNodes, out pExpression))
				{
					expressionProps = new Not(this, pExpression);
					return true;
				}
				else if (FindID(id, groupNodes, out pExpression))
				{
					expressionProps = new Group(this, pExpression);
					return true;
				}
				else
				{
					expressionProps = null;
					return false;
				}
			}

			public static float ExpressionFieldHeight(Expression expressionProps)
			{
				return EditorGUIUtility.singleLineHeight
					+ expressionProps.GetHeight();
			}

			public static Expression ExpressionFieldGUI(Rect position, Expression expressionProps)
			{
				Rect foreword = position;
				foreword.height = EditorGUIUtility.singleLineHeight;
				var choice = (ExpressionType)EditorGUI.EnumPopup(foreword, "Expression Type", expressionProps.expressionType);
				if (choice != expressionProps.expressionType)
				{
					switch (choice)
					{
						case ExpressionType.Literal:
							{
								Debug.Log("Make it a Literal!");
							}
							break;
						case ExpressionType.Dependency:
							{
								Debug.Log("Make it a Dependency!");
							}
							break;
						case ExpressionType.Not:
							{
								Debug.Log("Make it a Not!");
							}
							break;
						case ExpressionType.Group:
							{
								Debug.Log("Make it a Group!");
							}
							break;
					}
				}
				position.y += foreword.height;
				position.height -= foreword.height;
				expressionProps.OnGUI(position);
				return expressionProps;
			}
		}

		abstract class Expression
		{
			public readonly Tree tree;
			public readonly ExpressionType expressionType;
			public readonly SerializedProperty
				expression,
				ID;

			public Expression(Tree tree, SerializedProperty expression, ExpressionType expressionType)
			{
				this.tree = tree;
				this.expressionType = expressionType;
				this.expression = expression;
				ID = expression.FindPropertyRelative(nameof(Serializable.AExpression.ID));
			}

			protected abstract float GetHeight();
			protected abstract void OnGUI(Rect position);
		}

		sealed class Literal : Expression
		{
			public readonly SerializedProperty value;

			public Literal(Tree tree, SerializedProperty expression) : base(tree, expression, ExpressionType.Literal)
			{
				value = expression.FindPropertyRelative(nameof(Serializable.Literal.value));
			}

			static readonly GUIContent theLabel = new GUIContent("Literal");

			protected override float GetHeight()
			{
				float height = EditorGUI.GetPropertyHeight(expression, theLabel, false);
				if (expression.isExpanded)
				{
					height += EditorGUI.GetPropertyHeight(value, true);
				}
				return height;
			}

			protected override void OnGUI(Rect position)
			{
				position.height = EditorGUI.GetPropertyHeight(expression, theLabel, false);
				EditorGUI.PropertyField(position, expression, theLabel, false);
				position.y += position.height;

				if (expression.isExpanded)
				{
					++EditorGUI.indentLevel;

					position.height = EditorGUI.GetPropertyHeight(value, true);
					EditorGUI.PropertyField(position, value, true);
					position.y += position.height;

					--EditorGUI.indentLevel;
				}
			}
		}

		sealed class Dependency : Expression
		{
			public readonly SerializedProperty input;

			public Dependency(Tree tree, SerializedProperty expression) : base(tree, expression, ExpressionType.Dependency)
			{
				input = expression.FindPropertyRelative(nameof(Serializable.Dependency.input));
			}

			static readonly GUIContent theLabel = new GUIContent("Dependency");

			protected override float GetHeight()
			{
				float height = EditorGUI.GetPropertyHeight(expression, theLabel, false);
				if (expression.isExpanded)
				{
					height += EditorGUI.GetPropertyHeight(input, true);
				}
				return height;
			}

			protected override void OnGUI(Rect position)
			{
				position.height = EditorGUI.GetPropertyHeight(expression, theLabel, false);
				EditorGUI.PropertyField(position, expression, theLabel, false);
				position.y += position.height;

				if (expression.isExpanded)
				{
					++EditorGUI.indentLevel;

					position.height = EditorGUI.GetPropertyHeight(input, true);
					EditorGUI.PropertyField(position, input, true);
					position.y += position.height;

					--EditorGUI.indentLevel;
				}
			}
		}

		sealed class Not : Expression
		{
			public readonly SerializedProperty operandID;

			public Not(Tree tree, SerializedProperty expression) : base(tree, expression, ExpressionType.Not)
			{
				operandID = expression.FindPropertyRelative(nameof(Serializable.Not.operandID));
			}

			//public static readonly GUIContent operandIDLabel = new GUIContent("Not");
			static readonly GUIContent theLabel = new GUIContent("Not");

			protected override float GetHeight()
			{
				float height = EditorGUI.GetPropertyHeight(expression, theLabel, false);
				if (expression.isExpanded)
				{
					if (tree.FindID(operandID.intValue, out var expressionProps))
					{
						height += ExpressionFieldHeight(expressionProps);
					}
					else
					{
						height += Tree.errorHeight;
					}
				}
				return height;
			}

			protected override void OnGUI(Rect position)
			{
				position.height = EditorGUI.GetPropertyHeight(expression, theLabel, false);
				EditorGUI.PropertyField(position, expression, theLabel, false);
				position.y += position.height;

				if (expression.isExpanded)
				{
					++EditorGUI.indentLevel;

					if (tree.FindID(operandID.intValue, out var expressionProps))
					{
						position.height = ExpressionFieldHeight(expressionProps);
						ExpressionFieldGUI(position, expressionProps);
						position.y += position.height;
					}
					else
					{
						position.height = Tree.errorHeight;
						Tree.HelpBoxIDNotFoundError(position, operandID.intValue);
						position.y += position.height;
					}

					--EditorGUI.indentLevel;
				}
			}
		}

		sealed class Group : Expression
		{
			public SerializedProperty
				groupType,
				operandSequenceIDs;

			public Group(Tree tree, SerializedProperty expression) : base(tree, expression, ExpressionType.Group)
			{
				groupType = expression.FindPropertyRelative(nameof(Serializable.Group.type));
				operandSequenceIDs = expression.FindPropertyRelative(nameof(Serializable.Group.operandSequenceIDs));
			}

			static readonly GUIContent theLabel = new GUIContent("Not");

			protected override float GetHeight()
			{
				return EditorGUI.GetPropertyHeight(expression, theLabel, true);
			}

			protected override void OnGUI(Rect position)
			{
				EditorGUI.PropertyField(position, expression, theLabel, true);
			}
		}
	}

	[CustomPropertyDrawer(typeof(BoolExpressionTree))]
	public class BoolExpressionTreePropertyDrawer : PropertyDrawer
	{
		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			float height = EditorGUI.GetPropertyHeight(property, label, true);

			var treeProps = new Properties.Tree(property);

			height += Properties.Tree.spaceMargin;
			height += EditorGUI.GetPropertyHeight(treeProps.rootID, true);

			if (treeProps.FindID(treeProps.rootID.intValue, out var rootExpressionProps))
			{
				height += Expression.ExpressionFieldHeight(rootExpressionProps);
			}
			else
			{
				height += Properties.Tree.errorHeight;
			}

			return height;
		}

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			//EditorGUI.PropertyField(position, property, label, true);
			label = new GUIContent(label);

			position.height = EditorGUI.GetPropertyHeight(property, label, true);
			EditorGUI.PropertyField(position, property, label, true);
			position.y += position.height;

			var treeProps = new Properties.Tree(property);

			position.y += Properties.Tree.spaceMargin;
			position.height = EditorGUI.GetPropertyHeight(treeProps.rootID, true);
			EditorGUI.PropertyField(position, treeProps.rootID);
			position.y += position.height;

			++EditorGUI.indentLevel;

			if (treeProps.FindID(treeProps.rootID.intValue, out var rootExpressionProps))
			{
				position.height = Expression.ExpressionFieldHeight(rootExpressionProps);
				Expression.ExpressionFieldGUI(position, rootExpressionProps);
				position.y += position.height;
			}
			else
			{
				position.height = Properties.Tree.errorHeight;
				Properties.Tree.HelpBoxIDNotFoundError(position, treeProps.rootID.intValue);
				position.y += position.height;
			}

			--EditorGUI.indentLevel;
		}
	}
#endif
}