using System;
using UnityEngine;

public static class Utility
{
	public static bool TryElseLog(UnityEngine.Object context, Action action)
	{
		try
		{
			action();
			return true;
		}
		catch (Exception e)
		{
			Debug.LogError(e, context);
			return false;
		}
	}

	public static bool TryElseLog(Action action)
	{
		try
		{
			action();
			return true;
		}
		catch (Exception e)
		{
			Debug.LogError(e);
			return false;
		}
	}
}
