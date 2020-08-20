using System;
using System.Collections;
using System.Collections.Generic;
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

	public static float Cycle(float value, float length)
	{
		value %= length;
		return value < 0.0f ? length + value : value;
	}

	public static Transform[] ChildrenToArray(this Transform parent)
	{
		var c = parent.childCount;
		var children = new Transform[c];
		for (int i = 0; i < c; ++i)
		{
			children[i] = parent.GetChild(i);
		}
		return children;
	}

	public static int GetCurrentSceneBuildIndex() => UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;

	public static T ExtractRandomElement<T>(IList<T> list)
	{
		if (list.Count == 0)
		{
			throw new InvalidOperationException("List is empty");
		}
		int i = UnityEngine.Random.Range(0, list.Count);
		T v = list[i];
		list.RemoveAt(i);
		return v;
	}

	public static List<int> ListFromRange(int length)
	{
		var list = new List<int>(length);
		for (int i = 0; i < length; ++i)
		{
			list.Add(i);
		}
		return list;
	}
}

namespace Bison.Utility
{
	public struct Range
	{
		private Range(float min, float length)
		{
			this.min = min;
			this.length = length;
		}
		public static readonly Range ZeroUntilOne = new Range(0.0f, 1.0f);
		public static readonly Range ZeroUntil360 = new Range(0.0f, 360.0f);
		public static Range FromLength(float length) => new Range(0.0f, length);
		public static Range FromLength(float min, float length) => new Range(min, length);
		public static Range FromMinMax(float min, float max) => new Range(min, max - min);

		/// <summary>
		/// Minimum, inclusive. Start of range.
		/// </summary>
		public float min;

		/// <summary>
		/// Length of range.
		/// </summary>
		public float length;

		/// <summary>
		/// Maximum, exclusive. End of range.
		/// </summary>
		public float max {
			get => min + length;
			set => length = value - min;
		}

		public float Lerp(float t) => min + length * t;
		public float InverseLerp(float value) => (value - min) / length;
		public static float Map(float value, Range from, Range to) => to.Lerp(from.InverseLerp(value));

		public float Clamp(float value) => Mathf.Clamp(value, min, max);

		// has not been tested:
		//public float Cycle(float value) => min + V2_Utility.Cycle(value - min, length);
	}
}
