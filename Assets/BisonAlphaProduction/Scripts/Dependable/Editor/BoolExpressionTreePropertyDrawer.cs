using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using ExpressionKey = Bison.BoolExpressions.Serializable.ExpressionKey;
using ExpressionType = Bison.BoolExpressions.Serializable.ExpressionType;
using GroupType = Bison.BoolExpressions.Serializable.GroupType;

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
				root = new ExpressionKeyPropertyDrawer.Props(property.FindPropertyRelative(nameof(Serializable.Tree.root)));
				literalArray = property.FindPropertyRelative(nameof(Serializable.Tree.literalArray));
				dependencyArray = property.FindPropertyRelative(nameof(Serializable.Tree.dependencyArray));
				notArray = property.FindPropertyRelative(nameof(Serializable.Tree.notArray));
				groupArray = property.FindPropertyRelative(nameof(Serializable.Tree.groupArray));
			}

			public readonly SerializedProperty
				property,
				literalArray,
				dependencyArray,
				notArray,
				groupArray;

			public readonly ExpressionKeyPropertyDrawer.Props root;

			public const float padding = 2.0f;
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
						pArray = literalArray;
						return true;
					case ExpressionType.Dependency:
						pArray = dependencyArray;
						return true;
					case ExpressionType.Not:
						pArray = notArray;
						return true;
					case ExpressionType.Group:
						pArray = groupArray;
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
					if (TryGetExpressionProperty(key, out var pExpression, out _))
					{
						switch (key.type)
						{
							case ExpressionType.Literal:
								{
									return padding + EditorGUI.GetPropertyHeight(pExpression.FindPropertyRelative(nameof(Serializable.Literal.value)));
								}
							case ExpressionType.Dependency:
								{
									return padding + EditorGUI.GetPropertyHeight(pExpression.FindPropertyRelative(nameof(Serializable.Dependency.input)));
								}
							case ExpressionType.Not:
								{
									var operand = new ExpressionKeyPropertyDrawer.Props(
										pExpression.FindPropertyRelative(nameof(Serializable.Not.operand)));

									float height = padding + operand.GetPropertyHeight(expressionNotOperandLabel);

									height += GetExpressionHeight(operand.structValue, expressionNotOperandLabel, visitedKeys);

									return height;
								}
							case ExpressionType.Group:
								{
									float height = padding;
									var pGroupType = pExpression.FindPropertyRelative(nameof(Serializable.Group.type));
									switch ((GroupType)pGroupType.enumValueIndex)
									{
										case GroupType.And:
											{
												height += EditorGUI.GetPropertyHeight(pGroupType, expressionGroupTypeAndLabel, true);
											}
											break;
										case GroupType.Or:
											{
												height += EditorGUI.GetPropertyHeight(pGroupType, expressionGroupTypeOrLabel, true);
											}
											break;
										case GroupType.Xor:
											{
												height += EditorGUI.GetPropertyHeight(pGroupType, expressionGroupTypeXorLabel, true);
											}
											break;
										default:
											{
												height += helpBoxHeight;
											}
											break;
									}

									var pOperandSequence = pExpression.FindPropertyRelative(nameof(Serializable.Group.operandSequence));
									height += EditorGUI.GetPropertyHeight(pOperandSequence);

									return height;
								}
							default: throw new InvalidOperationException("Internal Error");
						}
					}
					else
					{
						return helpBoxHeight;
					}
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
					if (TryGetExpressionProperty(key, out var pExpression, out var error))
					{
						switch (key.type)
						{
							case ExpressionType.Literal:
								{
									position.y += padding;
									position.height -= padding;
									EditorGUI.PropertyField(position, pExpression.FindPropertyRelative(nameof(Serializable.Literal.value)), true);
									return;
								}
							case ExpressionType.Dependency:
								{
									position.y += padding;
									position.height -= padding;
									EditorGUI.PropertyField(position, pExpression.FindPropertyRelative(nameof(Serializable.Dependency.input)), true);
									return;
								}
							case ExpressionType.Not:
								{
									var operand = new ExpressionKeyPropertyDrawer.Props(
										pExpression.FindPropertyRelative(nameof(Serializable.Not.operand)));

									position.y += padding;
									position.height = operand.GetPropertyHeight(expressionNotOperandLabel);
									operand.OnGUI(position, expressionNotOperandLabel);
									position.y += position.height;

									position.height = GetExpressionHeight(operand.structValue, expressionNotOperandLabel, visitedKeys);
									OnExpressionGUI(position, operand.structValue, expressionNotOperandLabel, visitedKeys);

									return;
								}
							case ExpressionType.Group:
								{
									position.y += padding;
									var pGroupType = pExpression.FindPropertyRelative(nameof(Serializable.Group.type));
									switch ((GroupType)pGroupType.enumValueIndex)
									{
										case GroupType.And:
											{
												position.height = EditorGUI.GetPropertyHeight(pGroupType, expressionGroupTypeAndLabel, true);
												EditorGUI.PropertyField(position, pGroupType, expressionGroupTypeAndLabel, true);
											}
											break;
										case GroupType.Or:
											{
												position.height = EditorGUI.GetPropertyHeight(pGroupType, expressionGroupTypeOrLabel, true);
												EditorGUI.PropertyField(position, pGroupType, expressionGroupTypeOrLabel, true);
											}
											break;
										case GroupType.Xor:
											{
												position.height = EditorGUI.GetPropertyHeight(pGroupType, expressionGroupTypeXorLabel, true);
												EditorGUI.PropertyField(position, pGroupType, expressionGroupTypeXorLabel, true);
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
										var pOperandSequence = pExpression.FindPropertyRelative(nameof(Serializable.Group.operandSequence));
										position.height = EditorGUI.GetPropertyHeight(pOperandSequence);
										EditorGUI.PropertyField(position, pOperandSequence, true);
									}
									finally
									{
										--EditorGUI.indentLevel;
									}

									return;
								}
							default: throw new InvalidOperationException("Internal Error");
						}
					}
					else
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
								throw new InvalidOperationException();
						}
					}
				}
				finally
				{
					--EditorGUI.indentLevel;
					visitedKeys.Remove(key);
				}
			}

			public float GetPropertyHeight(GUIContent label)
			{
				float height = root.GetPropertyHeight(rootLabel);

				height += GetExpressionHeight(root.structValue, rootLabel, new HashSet<ExpressionKey>());

				height += verticalSpace;

				height += EditorGUI.GetPropertyHeight(property, label, true);

				return height;
			}

			public void OnGUI(Rect position, GUIContent label)
			{
				position.height = root.GetPropertyHeight(rootLabel);
				root.OnGUI(position, rootLabel);
				position.y += position.height;

				var visitedKeys = new HashSet<ExpressionKey>();
				position.height = GetExpressionHeight(root.structValue, rootLabel, visitedKeys);
				OnExpressionGUI(position, root.structValue, rootLabel, visitedKeys);
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