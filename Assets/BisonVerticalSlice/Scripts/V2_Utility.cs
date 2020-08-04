using System;
using UnityEngine;

public static class V2_Utility
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

	public static int Cycle(int value, int length)
	{
		value %= length;
		return value < 0 ? length + value : value;
	}
}
