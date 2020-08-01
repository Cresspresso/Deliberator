using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using ExpressionKey = Bison.BoolExpressions.Serializable.ExpressionKey;
using ExpressionType = Bison.BoolExpressions.Serializable.ExpressionType;
using GroupType = Bison.BoolExpressions.Serializable.GroupType;
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
				using (EditorDisposable.EditorGUI_indentLevel(0))
				{
					position.width *= 0.5f;
					position.width -= horizontalPadding * 0.5f;
					EditorGUI.LabelField(position, new GUIContent("", "Expression Type"));
					EditorGUI.PropertyField(position, type, GUIContent.none);
					position.x += position.width + horizontalPadding;
					EditorGUI.LabelField(position, new GUIContent("", "Index"));
					EditorGUI.PropertyField(position, index, GUIContent.none);
				}
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
							if (qNot.qOperand.property.isExpanded)
							{
								height += GetExpressionHeight(qNot.operand, expressionNotOperandLabel, visitedKeys);
							}
							return height;
						},
						qGroup =>
						{
							float height = EditorGUIUtility.standardVerticalSpacing;
							GUIContent typeLabel = GetGroupTypeLabel(qGroup.type);
							if (typeLabel != null)
							{
								height += EditorGUI.GetPropertyHeight(qGroup.pType, expressionGroupTypeXorLabel, true);
							}
							else
							{
								height += helpBoxHeight;
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

			public static GUIContent GetGroupTypeLabel(GroupType type)
			{
				switch (type)
				{
					case GroupType.And:
						return expressionGroupTypeAndLabel;
					case GroupType.Or:
						return expressionGroupTypeOrLabel;
					case GroupType.Xor:
						return expressionGroupTypeXorLabel;
					default:
						return null;
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

				try
				{
					using (EditorDisposable.EditorGUI_indentLevel_Increment())
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
								var oldPosition = position;
								var w = EditorGUIUtility.labelWidth;
								position.width = w;
								var oldExpanded = qNot.qOperand.property.isExpanded;
								qNot.qOperand.property.isExpanded = EditorGUI.Foldout(position, oldExpanded, expressionNotOperandLabel, true);
								position.x += w + 2;
								position.width = oldPosition.width - w - 2;
								using (EditorDisposable.EditorGUI_indentLevel(0))
								{
									qNot.qOperand.OnGUI(position, GUIContent.none);
								}

								if (oldExpanded)
								{
									position = oldPosition;
									position.y += position.height;
									position.height = GetExpressionHeight(qNot.operand, expressionNotOperandLabel, visitedKeys);
									OnExpressionGUI(position, qNot.operand, expressionNotOperandLabel, visitedKeys);
								}
								return null;
							},
							qGroup =>
							{
								position.y += EditorGUIUtility.standardVerticalSpacing;
								GUIContent typeLabel = GetGroupTypeLabel(qGroup.type);
								if (typeLabel != null)
								{
									position.height = EditorGUI.GetPropertyHeight(qGroup.pType, typeLabel, true);
									EditorGUI.PropertyField(position, qGroup.pType, typeLabel, true);
								}
								else
								{
									position.height = helpBoxHeight;
									EditorGUI.HelpBox(position, "Invalid group type", MessageType.Error);
								}
								position.y += position.height;

								using (EditorDisposable.EditorGUI_indentLevel_Increment())
								{
									position.height = EditorGUI.GetPropertyHeight(qGroup.pOperandSequence);
									EditorGUI.PropertyField(position, qGroup.pOperandSequence, true);
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
				}
				finally
				{
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
}