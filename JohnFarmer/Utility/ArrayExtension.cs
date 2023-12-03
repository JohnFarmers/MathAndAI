using System;
using CommunityToolkit.HighPerformance;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

namespace MathAndAI.JohnFarmer.Utility
{
	public static class ArrayExtension
	{
		public static string ToStr<T>(this T[] array)
		{
			string str = "[";

			Span<T> span = array.AsSpan();
			ref var start = ref MemoryMarshal.GetReference(span);
			ref var end = ref Unsafe.Add(ref start, span.Length);

			while (Unsafe.IsAddressLessThan(ref start, ref end))
			{
				str += start + ", ";
				start = ref Unsafe.Add(ref start, 1);
			}

			return array.GetType() + ": " + str.Remove(str.Length - 2) + "]";
		}
	}
}
