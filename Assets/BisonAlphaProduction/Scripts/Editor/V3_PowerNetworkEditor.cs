using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using System;

/// <author>Elijah Shadbolt</author>
/// <stage>Alpha Production</stage>
public class V3_PowerNetworkEditor : EditorWindow
{
	private static int IncrementCycleIndex(int x)
	{
		return x == int.MaxValue ? 0 : x + 1;
	}

	private static int IncrementCycleIndexUntil(int x, Predicate<int> untilCondition)
	{
		x = IncrementCycleIndex(x);
		if (untilCondition(x)) return x;
		for (int i = 0; i != int.MaxValue; ++i)
		{
			x = IncrementCycleIndex(x);
			if (untilCondition(x)) return x;
		}
		throw new System.InvalidOperationException("all possible values were exhausted because untilCondition always returns false.");
	}



	public class Resource<T> : IDisposable
	{
		public readonly T value;
		private Action<T> callback;

		public Resource(T value, Action<T> callback)
		{
			this.value = value;
			this.callback = callback;
		}

		public void Dispose()
		{
			callback(value);
		}
	}

	public static Resource<T> NewResource<T>(T value, Action<T> callback)
		=> new Resource<T>(value, callback);

	public static Resource<T> NewRestoreResource<T>(T valueToRestore, Action<T> setter, T newValue)
	{
		setter(newValue);
		return new Resource<T>(valueToRestore, setter);
	}

	private static Resource<bool> NewGUIEnabledResource(bool andCondition)
	{
		var valueToRestore = GUI.enabled;
		return NewRestoreResource(valueToRestore, v => GUI.enabled = v, valueToRestore && andCondition);
	}



	Vector2 centre => Vector2.one * 50_000;
	Vector2 pan;



	class Node
	{
		public Vector2 position { get; set; }
		public float width { get; set; }
		public GameObject[] array { get; set; } = new GameObject[0];

		public enum Mode { And, Or }
		public Mode mode;

		public Node(Vector2 position, float width)
		{
			this.position = position;
			this.width = width;
		}

		public const float margin = 4;
		public const float padding = 3;
		public const float edgeButtonWidth = 20;

		public float GetHeight()
		{
			var fieldHeight = EditorGUIUtility.singleLineHeight;
			float height = margin + fieldHeight;
			height += padding + fieldHeight;
			height += (padding + fieldHeight) * array.Length;
			height += margin;
			return height;
		}

		public Rect GetRect() => new Rect(position.x, position.y, width, GetHeight());
	}

	Dictionary<int, Node> m_nodes = new Dictionary<int, Node>();
	int m_idMax = 1;
	int m_idLastDrawnNodeWindow;

	private Node AddNode(int id, Vector2 position)
	{
		if (m_nodes.ContainsKey(id)) { throw new System.InvalidOperationException("Key already exists"); }
		m_idMax = Mathf.Max(m_idMax, id);
		var width = EditorGUIUtility.labelWidth + EditorGUIUtility.fieldWidth * 3;
		var node = new Node(position, width);
		m_nodes.Add(id, node);
		return node;
	}

	private (int, Node) AddNode(Vector2 position)
	{
		var id = IncrementCycleIndexUntil(m_idMax, i => !m_edges.ContainsKey(i));
		m_idMax = id;
		return (id, AddNode(id, position));
	}

	private void RemoveNode(int id)
	{
		m_nodes.Remove(id);

		// remove edges with id as destination
		m_edges.Remove(id);

		// remove edges with id as source
		foreach (var sources in m_edges.Values)
		{
			sources.Remove(id);
		}
	}

	struct EdgeKeyPair : IEquatable<EdgeKeyPair>
	{
		public int source;
		public int destination;

		public EdgeKeyPair(int source, int destination)
		{
			this.source = source;
			this.destination = destination;
		}

		public (int, int) tuple => (source, destination);

		public static bool operator ==(EdgeKeyPair a, EdgeKeyPair b) { return a.tuple == b.tuple; }
		public static bool operator !=(EdgeKeyPair a, EdgeKeyPair b) { return a.tuple != b.tuple; }
		public override bool Equals(object obj) => obj is EdgeKeyPair other && Equals(other);
		public bool Equals(EdgeKeyPair other) => tuple.Equals(other.tuple);
		public override int GetHashCode() => -1640241191 + tuple.GetHashCode();
	}

	struct EdgeNodePair
	{
		public Node source;
		public Node destination;

		public EdgeNodePair(Node source, Node destination)
		{
			this.source = source;
			this.destination = destination;
		}
	}

	struct Edge
	{
		public EdgeKeyPair keys;
		public EdgeNodePair nodes;

		public Edge(EdgeKeyPair keys, EdgeNodePair nodes)
		{
			this.keys = keys;
			this.nodes = nodes;
		}
	}

	/// <summary>
	/// key = destination id.
	/// value = source ids.
	/// </summary>
	Dictionary<int, HashSet<int>> m_edges = new Dictionary<int, HashSet<int>>();

	private void AddEdge(EdgeKeyPair edge)
	{
		if (edge.source == edge.destination)
		{
			throw new System.ArgumentException("source cannot == destination");
		}

		if (m_edges.TryGetValue(edge.destination, out var set))
		{
			set.Add(edge.source);
		}
		else
		{
			m_edges.Add(edge.destination, new HashSet<int> { edge.source });
		}
	}

	private void RemoveEdge(EdgeKeyPair edge)
	{
		if (m_edges.TryGetValue(edge.destination, out var set))
		{
			set.Remove(edge.source);
			if (0 == set.Count)
			{
				m_edges.Remove(edge.destination);
			}
		}
	}

	private bool ContainsEdge(EdgeKeyPair edge)
	{
		if (m_edges.TryGetValue(edge.destination, out var set))
		{
			return set.Contains(edge.source);
		}
		else
		{
			return false;
		}
	}

	EdgeKeyPair m_selectedEdge;
	(int idNode, bool isSourceNode)? m_creatingEdge;

	[MenuItem("Tools/Bison/Power Network")]
	static void ShowWindow()
	{
		var window = EditorWindow.GetWindow<V3_PowerNetworkEditor>();
		window.Show();
	}

	public void Awake()
	{
		pan = -centre;
		AddNode(2, centre + new Vector2(10, 10));
		AddNode(3, centre + new Vector2(210, 210));
		AddNode(4, centre + new Vector2(410, 410));
		AddEdge(new EdgeKeyPair(destination: 3, source: 2));
		AddEdge(new EdgeKeyPair(destination: 4, source: 3));
	}

	private static void Swap<T>(ref T a, ref T b)
	{
		T c = a;
		a = b;
		b = c;
	}

	void OnGUI()
	{
		GUI.BeginGroup(new Rect(pan.x, pan.y, centre.x * 2, centre.y * 2));

		if (m_creatingEdge.HasValue)
		{
			Event e = Event.current;
			switch (e.type)
			{
				case EventType.KeyDown:
					{
						if (e.keyCode == KeyCode.Escape)
						{
							m_creatingEdge = null;
							e.Use();
						}
					}
					break;
			}
		}

		BeginWindows();
		foreach (var pair in m_nodes)
		{
			UpdateWindowGUI(pair.Key, pair.Value);
		}
		EndWindows();

		foreach (var pair in m_edges)
		{
			var idDest = pair.Key;
			if (m_nodes.TryGetValue(idDest, out var nodeDest))
			{
				var sources = pair.Value;
				foreach (var idSource in sources)
				{
					if (m_nodes.TryGetValue(idSource, out var nodeSource))
					{
						DrawEdge(new Edge(new EdgeKeyPair(idSource, idDest), new EdgeNodePair(nodeSource, nodeDest)));
					}
				}
			}
		}

		if (m_creatingEdge.HasValue)
		{
			var (idNode, isSourceNode) = m_creatingEdge.Value;
			Vector3 startPos, endPos;
			if (isSourceNode)
			{
				Rect startRect = m_nodes[idNode].GetRect();
				startPos = new Vector3(startRect.x + startRect.width, startRect.y + startRect.height / 2, 0);
				endPos = Event.current.mousePosition;
			}
			else
			{
				Rect endRect = m_nodes[idNode].GetRect();
				startPos = Event.current.mousePosition;
				endPos = new Vector3(endRect.x, endRect.y + endRect.height / 2, 0);
			}
			Vector3 startTan = startPos + Vector3.right * 50;
			Vector3 endTan = endPos + Vector3.left * 50;
			var color = Color.cyan;
			Handles.DrawBezier(startPos, endPos, startTan, endTan, color, null, 4);
		}

		GUI.EndGroup();
		
		if (Event.current.type == EventType.MouseDrag)
		{
			pan += Event.current.delta;
			Repaint();
		}

		using (NewGUIEnabledResource(
			!m_creatingEdge.HasValue
			))
		{
			const float padding = 2;
			Rect rect = new Rect(2, 2, 100, EditorGUIUtility.singleLineHeight);
			if (GUI.Button(rect, "Add Node"))
			{
				Event.current.Use();
				AddNode(-pan + 0.5f * this.position.size);
				//if (m_nodes.TryGetValue(m_idMax, out var node))
				//{
				//	AddNode(node.position + Vector2.one * 5);
				//}
				//else
				//{
				//	AddNode(centre);
				//}
			}
			rect.y += rect.height;



			using (NewGUIEnabledResource(
				m_nodes.ContainsKey(m_idLastDrawnNodeWindow)
				))
			{
				rect.y += padding;
				if (GUI.Button(rect, "Remove Node"))
				{
					Event.current.Use();
					RemoveNode(m_idLastDrawnNodeWindow);
				}
				rect.y += rect.height;
			}



			using (NewGUIEnabledResource(
				ContainsEdge(m_selectedEdge)
				))
			{
				rect.y += padding;
				if (GUI.Button(rect, "Remove Edge"))
				{
					Event.current.Use();
					RemoveEdge(m_selectedEdge);
					m_selectedEdge = default;
				}
				rect.y += rect.height;
			}
		}


		if (m_creatingEdge.HasValue)
		{
			Repaint();
		}
	}

	private void UpdateWindowGUI(int id, Node node)
	{
		var rect = node.GetRect();
		rect = GUI.Window(id, rect, DrawNode, GUIContent.none);
		node.position = rect.position;
		node.width = rect.width;
	}

	private void DrawNode(int id)
	{
		var node = m_nodes[id];

		if (Event.current.type == EventType.Repaint)
		{
			m_idLastDrawnNodeWindow = id;
		}

		var margin = Node.margin;
		var edgeButtonWidth = Node.edgeButtonWidth;
		var padding = Node.padding;

		using (NewGUIEnabledResource(
			!m_creatingEdge.HasValue
			))
		{
			var rect = new Rect(
				margin + edgeButtonWidth + padding,
				margin,
				node.width - 2 * margin - 2 * edgeButtonWidth - 2 * padding,
				EditorGUIUtility.singleLineHeight);

			node.mode = (Node.Mode)EditorGUI.EnumPopup(rect, "Mode", node.mode);
			rect.y += rect.height;

			rect.y += Node.padding;
			var array = node.array;
			System.Array.Resize(ref array, Mathf.Max(0, EditorGUI.IntField(rect, "Size", array.Length)));
			node.array = array;
			rect.y += rect.height;

			for (int i = 0; i < array.Length; ++i)
			{
				rect.y += Node.padding;
				array[i] = (GameObject)EditorGUI.ObjectField(
					rect,
					"Element " + i,
					array[i],
					typeof(GameObject),
					true);
				rect.y += rect.height;
			}
		}

		// draw "create edge" buttons
		{
			var windowHeight = node.GetHeight();

			// left, destination node button
			var rect = new Rect(margin, margin, edgeButtonWidth, windowHeight - 2 * margin);
			if (!m_creatingEdge.HasValue)
			{
				if (GUI.Button(rect, ">"))
				{
					Event.current.Use();
					m_creatingEdge = (id, isSourceNode: false);
				}
			}
			else
			{
				var (idOther, isOtherSourceNode) = m_creatingEdge.Value;
				if (idOther == id)
				{
					if (GUI.Button(rect, "X"))
					{
						Event.current.Use();
						m_creatingEdge = null;
					}
				}
				else
				{
					var edge = new EdgeKeyPair(idOther, id);
					bool b = isOtherSourceNode && !ContainsEdge(edge);
					using (NewGUIEnabledResource(b))
					{
						if (GUI.Button(rect, b ? ">" : "-"))
						{
							Event.current.Use();
							m_creatingEdge = null;
							AddEdge(edge);
							m_selectedEdge = edge;
						}
					}
				}
			}

			// right, source node button
			rect = new Rect(node.width - margin - edgeButtonWidth, margin, edgeButtonWidth, windowHeight - 2 * margin);
			if (!m_creatingEdge.HasValue)
			{
				if (GUI.Button(rect, ">"))
				{
					Event.current.Use();
					m_creatingEdge = (id, isSourceNode: true);
				}
			}
			else
			{
				var (idOther, isOtherSourceNode) = m_creatingEdge.Value;
				if (idOther == id)
				{
					if (GUI.Button(rect, "X"))
					{
						Event.current.Use();
						m_creatingEdge = null;
					}
				}
				else
				{
					var edge = new EdgeKeyPair(id, idOther);
					bool b = !isOtherSourceNode && !ContainsEdge(edge);
					using (NewGUIEnabledResource(b))
					{
						if (GUI.Button(rect, b ? ">" : "-"))
						{
							Event.current.Use();
							m_creatingEdge = null;
							AddEdge(edge);
							m_selectedEdge = edge;
						}
					}
				}
			}

			//GUI.enabled = m_creatingEdge.HasValue == false
			//	|| (m_creatingEdge.Value.isSourceNode
			//	&& m_creatingEdge.Value.idNode != id
			//	&& !ContainsEdge(new EdgeKeyPair(m_creatingEdge.Value.idNode, id)));

			//if (GUI.Button(rect, ">"))
			//{
			//	if (m_creatingEdge.HasValue)
			//	{
			//		// finish creating the edge
			//		try
			//		{
			//			var (idSource, isSourceNode) = m_creatingEdge.Value;
			//			Debug.Assert(isSourceNode);
			//			if (isSourceNode)
			//			{
			//				var edge = new EdgeKeyPair(idSource, id);
			//				AddEdge(edge);
			//				m_selectedEdge = edge;
			//			}
			//		}
			//		finally
			//		{
			//			m_creatingEdge = null;
			//		}
			//	}
			//	else
			//	{
			//		// begin creating an edge
			//		m_creatingEdge = (id, isSourceNode: false);
			//	}
			//}

			//// right, source node button
			//rect = new Rect(node.width - margin - edgeButtonWidth, margin, edgeButtonWidth, windowHeight - 2 * margin);

			//GUI.enabled = m_creatingEdge.HasValue == false
			//	|| (m_creatingEdge.Value.isSourceNode == false
			//	&& m_creatingEdge.Value.idNode != id
			//	&& !ContainsEdge(new EdgeKeyPair(id, m_creatingEdge.Value.idNode)));

			//if (GUI.Button(rect, ">"))
			//{
			//	if (m_creatingEdge.HasValue)
			//	{
			//		// finish creating the edge
			//		try
			//		{
			//			var (idDest, isSourceNode) = m_creatingEdge.Value;
			//			Debug.Assert(!isSourceNode);
			//			if (!isSourceNode)
			//			{
			//				var edge = new EdgeKeyPair(id, idDest);
			//				AddEdge(edge);
			//				m_selectedEdge = edge;
			//			}
			//		}
			//		finally
			//		{
			//			m_creatingEdge = null;
			//		}
			//	}
			//	else
			//	{
			//		// begin creating an edge
			//		m_creatingEdge = (id, isSourceNode: true);
			//	}
			//}
		}

		GUI.DragWindow();
	}

	void DrawEdge(Edge edge)
	{
		Rect startRect = edge.nodes.source.GetRect();
		Rect endRect = edge.nodes.destination.GetRect();
		Vector3 startPos = new Vector3(startRect.x + startRect.width, startRect.y + startRect.height / 2, 0);
		Vector3 endPos = new Vector3(endRect.x, endRect.y + endRect.height / 2, 0);
		Vector3 startTan = startPos + Vector3.right * 50;
		Vector3 endTan = endPos + Vector3.left * 50;

		var color = edge.keys == m_selectedEdge
			? Color.cyan
			: Color.white;
		Handles.DrawBezier(startPos, endPos, startTan, endTan, color, null, 4);

		var points = Handles.MakeBezierPoints(startPos, endPos, startTan, endTan, 3);
		var middle = points[1];

		if (Handles.Button(middle, Quaternion.identity, 4, 8, Handles.CircleHandleCap))
		{
			m_selectedEdge = edge.keys;
		}
	}
}
