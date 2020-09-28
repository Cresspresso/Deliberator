using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class V3_EndingTrigger : MonoBehaviour
{
	[SerializeField]
	private V3_SparEnding m_data;

	private void Awake()
	{
		if (!m_data)
		{
			m_data = GetComponent<V3_SparEnding>();
			if (!m_data)
			{
				Debug.LogError(nameof(V3_SparEnding) + " is null", this);
			}
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (V2_GroundhogControl.instance.hasFinished) return;
		if (!m_data) return;

		if (other.CompareTag("Player"))
		{
			V2_GroundhogControl.instance.Finish();

			V3_SparGameObject.Destroy();
			var preservedData = V3_SparGameObject.FindOrCreateComponent<V3_SparEnding>();
			preservedData.Copy(m_data);

			V3_SparGameObject.LoadScene("Ending Video");
		}
	}
}
