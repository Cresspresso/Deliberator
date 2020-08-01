using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dependable : MonoBehaviour
{
	[SerializeField]
	private Bison.BoolExpressions.Serializable.Tree m_condition;

	//public virtual bool Evaluate()
	//{
	//	return m_tree.expression.Evaluate();
	//}
}
