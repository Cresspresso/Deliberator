using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using ExpressionKey = Bison.BoolExpressions.Serializable.ExpressionKey;
using ExpressionType = Bison.BoolExpressions.Serializable.ExpressionType;
using GroupType = Bison.BoolExpressions.Serializable.GroupType;
using System.Linq;
using UnityEditorInternal;
using NUnit.Framework;

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

			public static readonly GUIContent operandLabel = new GUIContent("Not");
		}



		public class Group
		{
			public Group(SerializedProperty property)
			{
				this.property = property;
				pType = property.FindPropertyRelative(nameof(Serializable.Group.type));
				pOperandSequence = property.FindPropertyRelative(nameof(Serializable.Group.operandSequence));
				pOperandSequenceSize = pOperandSequence.Copy();
				pOperandSequenceSize.Next(true);
				pOperandSequenceSize.Next(true);
			}

			public readonly SerializedProperty
				property,
				pType,
				pOperandSequence,
				pOperandSequenceSize;

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

			public static readonly GUIContent[] groupTypeLabels = new GUIContent[]
			{
				new GUIContent("And"),
				new GUIContent("Or"),
				new GUIContent("Xor"),
			};

			public static GUIContent GetGroupTypeLabel(GroupType type)
			{
				var i = (int)type;
				if (i < 0 || i >= groupTypeLabels.Length)
				{
					return null;
				}
				else
				{
					return groupTypeLabels[i];
				}
			}
		}



		public class Arrays
		{
			public Arrays(SerializedProperty property)
			{
				this.property = property;
				pLiteralArray = property.FindPropertyRelative(nameof(Serializable.Arrays.literalArray));
				pDependencyArray = property.FindPropertyRelative(nameof(Serializable.Arrays.dependencyArray));
				pNotArray = property.FindPropertyRelative(nameof(Serializable.Arrays.notArray));
				pGroupArray = property.FindPropertyRelative(nameof(Serializable.Arrays.groupArray));
			}

			public readonly SerializedProperty
				property,
				pLiteralArray,
				pDependencyArray,
				pNotArray,
				pGroupArray;

			public float GetRawHeight(GUIContent label)
			{
				return EditorGUI.GetPropertyHeight(property, label, true);
			}

			public void OnRawGUI(Rect position, GUIContent label)
			{
				EditorGUI.PropertyField(position, property, label, true);
			}

			public static float helpBoxHeight => EditorGUIUtility.singleLineHeight * 2;

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
							height += GetExpressionFoldoutHeight(qNot.qOperand, ExpressionProps.Not.operandLabel, visitedKeys);
							return height;
							//float height = EditorGUIUtility.standardVerticalSpacing;
							//height += qNot.qOperand.GetPropertyHeight(expressionNotOperandLabel);
							//if (qNot.qOperand.property.isExpanded)
							//{
							//	height += GetExpressionHeight(qNot.operand, expressionNotOperandLabel, visitedKeys);
							//}
							//return height;
						},
						qGroup =>
						{
							float height = EditorGUIUtility.standardVerticalSpacing;
							GUIContent typeLabel = ExpressionProps.Group.GetGroupTypeLabel(qGroup.type);
							if (typeLabel != null)
							{
								height += EditorGUI.GetPropertyHeight(qGroup.pType, typeLabel, true);

								if (qGroup.pType.isExpanded)
								{
									height += EditorGUIUtility.standardVerticalSpacing;
									height += EditorGUI.GetPropertyHeight(qGroup.pOperandSequenceSize, true);

									var c = qGroup.pOperandSequence.arraySize;
									for (int i = 0; i < c; ++i)
									{
										var qElement = qGroup.GetOperandPropsAtIndex(i);
										var elementLabel = new GUIContent(qElement.property.displayName);

										height += EditorGUIUtility.standardVerticalSpacing;
										height += GetExpressionFoldoutHeight(qElement, elementLabel, visitedKeys);
										//height += EditorGUIUtility.standardVerticalSpacing;
										//height += qElement.GetPropertyHeight(elementLabel);
										//if (qElement.property.isExpanded)
										//{
										//	height += GetExpressionHeight(qElement.structValue, elementLabel, visitedKeys);
										//}
									}
								}
							}
							else
							{
								height += helpBoxHeight;
							}

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
								position.height -= EditorGUIUtility.standardVerticalSpacing;
								OnExpressionFoldoutGUI(position, qNot.qOperand, ExpressionProps.Not.operandLabel, visitedKeys);
								//position.y += EditorGUIUtility.standardVerticalSpacing;
								//position.height = qNot.qOperand.GetPropertyHeight(expressionNotOperandLabel);
								//var oldPosition = position;
								//var w = EditorGUIUtility.labelWidth;
								//position.width = w;
								//var oldExpanded = qNot.qOperand.property.isExpanded;
								//qNot.qOperand.property.isExpanded = EditorGUI.Foldout(position, oldExpanded, expressionNotOperandLabel, true);
								//position.x += w + 2;
								//position.width = oldPosition.width - w - 2;
								//using (EditorDisposable.EditorGUI_indentLevel(0))
								//{
								//	qNot.qOperand.OnGUI(position, GUIContent.none);
								//}

								//if (oldExpanded)
								//{
								//	position = oldPosition;
								//	position.y += position.height;
								//	position.height = GetExpressionHeight(qNot.operand, expressionNotOperandLabel, visitedKeys);
								//	OnExpressionGUI(position, qNot.operand, expressionNotOperandLabel, visitedKeys);
								//}
								return null;
							},
							qGroup =>
							{
								position.y += EditorGUIUtility.standardVerticalSpacing;
								GUIContent typeLabel = ExpressionProps.Group.GetGroupTypeLabel(qGroup.type);
								if (typeLabel != null)
								{
									var oldExpanded = qGroup.pType.isExpanded;

									position.height = EditorGUI.GetPropertyHeight(qGroup.pType, typeLabel, true);
									var oldPosition = position;
									{
										var w = EditorGUIUtility.labelWidth;
										position.width = w;
										qGroup.pType.isExpanded = EditorGUI.Foldout(position, oldExpanded, typeLabel, true);
										position.x += w + 2;
										position.width = oldPosition.width - w - 2;
										using (EditorDisposable.EditorGUI_indentLevel(0))
										{
											EditorGUI.PropertyField(position, qGroup.pType, GUIContent.none);
										}
									}
									position = oldPosition;
									position.y += position.height;

									if (oldExpanded)
									{
										using (EditorDisposable.EditorGUI_indentLevel_Increment())
										{
											position.y += EditorGUIUtility.standardVerticalSpacing;
											position.height = EditorGUI.GetPropertyHeight(qGroup.pOperandSequenceSize, true);
											EditorGUI.PropertyField(position, qGroup.pOperandSequenceSize, true);
											position.y += position.height;

											var c = qGroup.pOperandSequence.arraySize;
											for (int i = 0; i < c; ++i)
											{
												var qElement = qGroup.GetOperandPropsAtIndex(i);
												var elementLabel = new GUIContent(qElement.property.displayName);

												position.y += EditorGUIUtility.standardVerticalSpacing;
												position.height = GetExpressionFoldoutHeight(qElement, elementLabel, visitedKeys);
												OnExpressionFoldoutGUI(position, qElement, elementLabel, visitedKeys);
												position.y += position.height;

												//var oldElementExpanded = qElement.property.isExpanded;

												//position.y += EditorGUIUtility.standardVerticalSpacing;
												//position.height = qElement.GetPropertyHeight(elementLabel);
												//var oldElementPosition = position;
												//{
												//	var w = EditorGUIUtility.labelWidth;
												//	position.width = w;
												//	qElement.property.isExpanded = EditorGUI.Foldout(position, oldElementExpanded, elementLabel, true);
												//	position.x += w + 2;
												//	position.width = oldPosition.width - w - 2;
												//	using (EditorDisposable.EditorGUI_indentLevel(0))
												//	{
												//		qElement.OnGUI(position, GUIContent.none);
												//	}
												//}
												//position = oldElementPosition;
												//position.y += position.height;

												//if (oldElementExpanded)
												//{
												//	position.height = GetExpressionHeight(qElement.structValue, elementLabel, visitedKeys);
												//	OnExpressionGUI(position, qElement.structValue, elementLabel, visitedKeys);
												//	position.y += position.height;
												//}
											}
										}
									}
								}
								else
								{
									position.height = helpBoxHeight;
									{
										var oldPosition = position;
										position = EditorGUI.IndentedRect(position);
										EditorGUI.HelpBox(position, "Invalid group type", MessageType.Error);
										oldPosition = position;
									}
									position.y += position.height;
								}
								return null;
							},
							error =>
							{
								var oldPosition = position;
								position = EditorGUI.IndentedRect(position);
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
								oldPosition = position;
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

			public float GetExpressionFoldoutHeight(
				ExpressionKeyPropertyDrawer.Props qKey,
				GUIContent label,
				HashSet<ExpressionKey> visitedKeys)
			{
				float height = qKey.GetPropertyHeight(label);
				if (qKey.property.isExpanded)
				{
					height += GetExpressionHeight(qKey.structValue, label, visitedKeys);
				}
				return height;
			}

			public void OnExpressionFoldoutGUI(
				Rect position,
				ExpressionKeyPropertyDrawer.Props qKey,
				GUIContent label,
				HashSet<ExpressionKey> visitedKeys)
			{
				position.height = qKey.GetPropertyHeight(label);
				var oldPosition = position;
				var oldExpanded = qKey.property.isExpanded;
				{
					var w = EditorGUIUtility.labelWidth;
					position.width = w;
					qKey.property.isExpanded = EditorGUI.Foldout(position, oldExpanded, label, true);
					position.x += w + 2;
					position.width = oldPosition.width - w - 2;
					using (EditorDisposable.EditorGUI_indentLevel(0))
					{
						qKey.OnGUI(position, GUIContent.none);
					}
				}
				position = oldPosition;
				position.y += position.height;

				if (oldExpanded)
				{
					position.height = GetExpressionHeight(qKey.structValue, label, visitedKeys);
					OnExpressionGUI(position, qKey.structValue, label, visitedKeys);
					position.y += position.height;
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
							return '"' + (input ? input.name : "None") + '"';
						},
						qNot => "!" + GetExpressionString(qNot.operand, visitedKeys),
						qGroup =>
						{
							var sep = ") " + Serializable.Group.GetOperatorString(qGroup.type) + " (";
							var items = qGroup.operandSequence.Select(
								operandKey => GetExpressionString(operandKey, visitedKeys));
							return "((" + string.Join(sep, items) + "))";
						},
						error => "Error(" + error.ToString() + ")"
						);
				}
				finally
				{
					visitedKeys.Remove(key);
				}
			}
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
				qArrays = new ExpressionProps.Arrays(property.FindPropertyRelative(nameof(Serializable.Tree.arrays)));

				rootLabel = new GUIContent(qRoot.property.displayName, "Expression tree which references nodes in the Arrays property.");
				arraysLabel = new GUIContent(qArrays.property.displayName, "Arrays of all nodes able to be used in the root expression.");
			}

			public readonly SerializedProperty property;
			public readonly ExpressionKeyPropertyDrawer.Props qRoot;
			public readonly ExpressionProps.Arrays qArrays;

			public static float spaceBetweenRootAndRawArrays => 0;// EditorGUIUtility.singleLineHeight * 1.5f;

			public readonly GUIContent rootLabel, arraysLabel;

			public float GetPropertyHeight(GUIContent label)
			{
				float height = EditorGUIUtility.singleLineHeight;
				if (property.isExpanded)
				{
					if (property.serializedObject.isEditingMultipleObjects)
					{
						height += EditorGUIUtility.standardVerticalSpacing;
						height += EditorGUIUtility.singleLineHeight;

						height += EditorGUIUtility.standardVerticalSpacing;
						height += qRoot.GetPropertyHeight(rootLabel);
					}
					else
					{
						height += EditorGUIUtility.standardVerticalSpacing;
						height += EditorGUIUtility.singleLineHeight;

						height += EditorGUIUtility.standardVerticalSpacing;
						height += qArrays.GetExpressionFoldoutHeight(qRoot, rootLabel, new HashSet<ExpressionKey>());

						if (qRoot.property.isExpanded)
						{
							height += spaceBetweenRootAndRawArrays;
						}
					}
					height += qArrays.GetRawHeight(arraysLabel);
				}
				return height;
			}

			public void OnGUI(Rect position, GUIContent label)
			{
				position.height = EditorGUIUtility.singleLineHeight;
				var oldExpanded = property.isExpanded;
				property.isExpanded = EditorGUI.Foldout(position, property.isExpanded, property.displayName, true);
				position.y += position.height;
				if (oldExpanded)
				{
					using (EditorDisposable.EditorGUI_indentLevel_Increment())
					{
						if (property.serializedObject.isEditingMultipleObjects)
						{
							position.y += EditorGUIUtility.standardVerticalSpacing;
							position.height = EditorGUIUtility.singleLineHeight;
							{
								var oldPosition = position;
								position = EditorGUI.IndentedRect(position);
								EditorGUI.HelpBox(position, "Cannot edit multiple objects", MessageType.None);
								position = oldPosition;
							}
							position.y += position.height;

							position.y += EditorGUIUtility.standardVerticalSpacing;
							position.height = qRoot.GetPropertyHeight(rootLabel);
							qRoot.OnGUI(position, rootLabel);
							position.y += position.height;
						}
						else
						{
							position.y += EditorGUIUtility.standardVerticalSpacing;
							position.height = EditorGUIUtility.singleLineHeight;
							string expressionString = qArrays.GetExpressionString(qRoot.structValue, new HashSet<ExpressionKey>());
							{
								var oldPosition = position;
								position = EditorGUI.PrefixLabel(position, new GUIContent("Expression", expressionString));
								using (EditorDisposable.EditorGUI_indentLevel(0))
								{
									EditorGUI.SelectableLabel(position, expressionString);
								}
								position = oldPosition;
							}
							position.y += position.height;

							var visitedKeys = new HashSet<ExpressionKey>();
							position.y += EditorGUIUtility.standardVerticalSpacing;
							var oldRootExpanded = qRoot.property.isExpanded;
							position.height = qArrays.GetExpressionFoldoutHeight(qRoot, rootLabel, visitedKeys);
							qArrays.OnExpressionFoldoutGUI(position, qRoot, rootLabel, visitedKeys);
							position.y += position.height;

							if (oldRootExpanded)
							{
								position.y += spaceBetweenRootAndRawArrays;
							}
						}
						position.height = qArrays.GetRawHeight(arraysLabel);
						qArrays.OnRawGUI(position, arraysLabel);
						position.y += position.height;
					}
				}
			}
		}
	}
}