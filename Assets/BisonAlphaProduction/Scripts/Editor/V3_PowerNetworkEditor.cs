using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <author>Elijah Shadbolt</author>
/// <stage>Alpha Production</stage>
public class V3_PowerNetworkEditor : EditorWindow
{
	Vector2 centre => Vector2.one * 50_000;
	Vector2 pan;

	class Node
	{
		public Vector2 position { get; set; }
		public float width { get; set; }
		public GameObject[] array { get; private set; } = new GameObject[0];

		public enum Mode { And, Or }
		public Mode mode;

		public Node(Vector2 position, float width)
		{
			this.position = position;
			this.width = width;
		}

		public const float margin = 4;
		public const float padding = 3;

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

		public void Draw()
		{
			var rect = new Rect(margin, margin, width - 2 * margin, EditorGUIUtility.singleLineHeight);

			mode = (Node.Mode)EditorGUI.EnumPopup(rect, "Mode", mode);
			rect.y += rect.height;

			rect.y += Node.padding;
			var array = this.array;
			System.Array.Resize(ref array, Mathf.Max(0, EditorGUI.IntField(rect, "Size", array.Length)));
			this.array = array;
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
	}

	Dictionary<int, Node> m_nodes = new Dictionary<int, Node>();
	int m_idMax = 1;

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
		++m_idMax;
		var id = m_idMax;
		return (id, AddNode(id, position));
	}

	/// <summary>
	/// key = destination id.
	/// value = source ids.
	/// </summary>
	Dictionary<int, HashSet<int>> m_edges = new Dictionary<int, HashSet<int>>();

	private void AddEdge(int destination, int source)
	{
		if (m_edges.TryGetValue(destination, out var set))
		{
			set.Add(source);
		}
		else
		{
			m_edges.Add(destination, new HashSet<int> { source });
		}
	}

	private void RemoveEdge(int destination, int source)
	{
		if (m_edges.TryGetValue(destination, out var set))
		{
			set.Remove(source);
			if (0 == set.Count)
			{
				m_edges.Remove(destination);
			}
		}
	}

	private bool ContainsEdge(int destination, int source)
	{
		if (m_edges.TryGetValue(destination, out var set))
		{
			return set.Contains(source);
		}
		else
		{
			return false;
		}
	}

	[MenuItem("Tools/Bison Power Network")]
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
		AddEdge(destination: 3, source: 2);
		AddEdge(destination: 2, source: 3);
	}

	void OnGUI()
	{
		GUI.BeginGroup(new Rect(pan.x, pan.y, centre.x * 2, centre.y * 2));

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
						DrawNodeCurve(nodeSource.GetRect(), nodeDest.GetRect());
					}
				}
			}
		}

		GUI.EndGroup();
		
		if (Event.current.type == EventType.MouseDrag)
		{
			pan += Event.current.delta;
			Repaint();
		}

		if (GUI.Button(new Rect(2, 2, 16, 16), "+"))
		{
			if (m_nodes.TryGetValue(m_idMax, out var node))
			{
				AddNode(node.position + Vector2.one * 5);
			}
			else
			{
				AddNode(centre);
			}
			// DEBUG
			AddEdge(m_idMax, m_idMax - 1);
		}
	}

	private void UpdateWindowGUI(int id, Node node)
	{
		var rect = GUI.Window(id, node.GetRect(), Callback, GUIContent.none);
		node.position = rect.position;
		node.width = rect.width;
	}

	private void Callback(int id)
	{
		var node = m_nodes[id];
		node.Draw();
		GUI.DragWindow();
	}

	void DrawNodeCurve(Rect start, Rect end)
	{
		Vector3 startPos = new Vector3(start.x + start.width, start.y + start.height / 2, 0);
		Vector3 endPos = new Vector3(end.x, end.y + end.height / 2, 0);
		Vector3 startTan = startPos + Vector3.right * 50;
		Vector3 endTan = endPos + Vector3.left * 50;
		Handles.DrawBezier(startPos, endPos, startTan, endTan, Color.white, null, 4);
	}
}
