using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bison.Document
{
	public class V3_DocBlock : MonoBehaviour
	{
		public V3_DocColumn column { get; private set; }

		public void OnSpawned(V3_DocColumn column)
		{
			this.column = column;
		}
	}
}
