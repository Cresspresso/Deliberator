using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tree = Bison.BoolExpressions.Serializable.Tree;

public class Dependable : MonoBehaviour
{
	public Tree condition = new Tree();

	public bool Evaluate() => condition.Evaluate();

	private void Awake()
	{
		Debug.Log(name + " tree = " + (condition != null ? condition.ToString() : "Error(condition null"));
		Debug.Log(name + " evaluated " + Evaluate());
	}
}
