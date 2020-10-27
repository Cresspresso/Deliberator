using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class V5_ScreenshotSaver : MonoBehaviour
{
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.F8))
		{
			TakeScreenshotAndSave();
		}
	}

	public static void TakeScreenshotAndSave()
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

		var msg = "Saved screenshot to " + filename;
		if (V4_PlaytestConsole.instance)
		{
			V4_PlaytestConsole.Log(msg);
		}
		else
		{
			Debug.Log(msg);
		}
	}
}
