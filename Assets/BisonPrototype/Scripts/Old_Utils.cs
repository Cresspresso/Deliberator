using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Old_Utils
{
	public static void TryExceptLog(System.Action a)
	{
		try
		{
			a();
		}
		catch (System.Exception e)
		{
			Debug.LogException(e);
		}
	}

	public static void TryExceptLog(UnityEngine.Object context, System.Action a)
	{
		try
		{
			a();
		}
		catch (System.Exception e)
		{
			Debug.LogException(e, context);
		}
	}
}
