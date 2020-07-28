using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class V3_LaserEmitter : MonoBehaviour
{
	public LineRenderer lineRenderer { get; private set; }

	public LayerMask hitMask = ~0;
	public float laserLength = 10.0f;
	public float skin = 0.01f;

	private void Awake()
	{
		lineRenderer = GetComponent<LineRenderer>();
	}

	private void Update()
	{
		Ray ray = new Ray(transform.position, transform.forward);
		float length = laserLength;
		var positions = new List<Vector3>();

		const int maxTries = 10000;
		int tries;
		for (tries = 0; tries < maxTries; ++tries)
		{
			if (Physics.Raycast(ray, out var hit, length, hitMask, QueryTriggerInteraction.Ignore))
			{
				positions.Add(ray.GetPoint(Mathf.Max(hit.distance - skin, 0)));
				length -= hit.distance;
				if (hit.collider.CompareTag("Reflect Laser"))
				{
					ray = new Ray(hit.point, Vector3.Reflect(ray.direction, hit.normal));
				}
				else
				{
					break;
				}
			}
			else
			{
				positions.Add(ray.GetPoint(length));
				break;
			}
		}
		if (tries == maxTries)
		{
			Debug.LogWarning("Too many tries.", this);
		}

		lineRenderer.SetPositions(positions.ToArray());
	}
}
