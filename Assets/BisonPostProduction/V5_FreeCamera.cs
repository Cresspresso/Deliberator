using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class V5_FreeCamera : MonoBehaviour
{
	public Vector3 speed = new Vector3(10, 10, 10);
	public float horzSpeed = 10;
	public float vertSpeed = 10;

	private float horz, vert, roll;

	private void OnEnable()
	{
		transform.position = Camera.main.transform.position;
		transform.rotation = Camera.main.transform.rotation;

		horz = Mathf.Repeat(transform.localEulerAngles.y, 360);
		vert = Mathf.Clamp(transform.localEulerAngles.x, -89.9f, 89.9f);
		roll = Mathf.Repeat(transform.localEulerAngles.z, 360);
	}

	private void Update()
	{
		int up = 0;
		if (Input.GetKey(KeyCode.E))
		{
			++up;
		}
		if (Input.GetKey(KeyCode.Q))
		{
			--up;
		}
		var inp = new Vector3(Input.GetAxis("Horizontal"), up, Input.GetAxis("Vertical"));
		var delta = Vector3.Scale(inp, speed) * (Input.GetKey(KeyCode.LeftShift) ? 3 : 1) * Time.deltaTime;
		transform.position += transform.forward * delta.z + transform.right * delta.x + transform.up * delta.y;

		horz += Input.GetAxis("Mouse X") * horzSpeed;
		horz = Mathf.Repeat(horz, 360);

		vert -= Input.GetAxis("Mouse Y") * vertSpeed;
		vert = Mathf.Clamp(vert, -89.9f, 89.9f);

		transform.localEulerAngles = new Vector3(vert, horz, roll);



		if (Input.GetKeyDown(KeyCode.T))
		{
			var now = System.DateTime.Now;
			var Y = now.Year.ToString();
			var M = now.Month.ToString().PadLeft(2, '0');
			var d = now.Day.ToString().PadLeft(2, '0');
			var H = now.Hour.ToString().PadLeft(2, '0');
			var m = now.Minute.ToString().PadLeft(2, '0');
			var s = now.Second.ToString().PadLeft(2, '0');

			var filename = $"{Application.persistentDataPath}/{Y}-{M}-{d}--{H}-{m}-{s}.png";
			ScreenCapture.CaptureScreenshot(filename);
			V4_PlaytestConsole.Log("Saved screenshot to " + filename);
		}
	}
}
