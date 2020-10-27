using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TSingleton = V2_Singleton<V5_FreeCameraManager>;

public class V5_FreeCameraManager : MonoBehaviour
{
	public bool isFree { get; private set; }
	public V5_FreeCamera freecam;

	public static V5_FreeCameraManager instance => TSingleton.instance;

	private void Awake()
	{
		TSingleton.OnAwake(this, V2_SingletonDuplicateMode.DestroyGameObject);

		freecam.transform.SetParent(null, true);
		freecam.gameObject.SetActive(false);
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.F6))
		{
			isFree = !isFree;
			var maincam = Camera.main;
			if (isFree)
			{
				freecam.transform.position = Camera.main.transform.position;
				freecam.transform.rotation = Camera.main.transform.rotation;
				maincam.transform.SetParent(freecam.transform);
				maincam.transform.localPosition = Vector3.zero;
				maincam.transform.localRotation = Quaternion.identity;
			}
			else
			{
				maincam.transform.SetParent(V2_FirstPersonCharacterController.instance.head);
				maincam.transform.localPosition = Vector3.zero;
				maincam.transform.localRotation = Quaternion.identity;
			}
			freecam.gameObject.SetActive(isFree);
		}
	}
}
