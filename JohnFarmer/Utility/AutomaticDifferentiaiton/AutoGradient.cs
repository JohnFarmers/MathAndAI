using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace JohnFarmer.Utility
{
	public class AutoGradient
	{
		private readonly List<Operation> operations = new List<Operation>();

		public void Record(Operation operation) => operations.Add(operation);

		public void Record(params Operation[] operations)
		{
			Span<Operation> span = operations.AsSpan();
			ref var start = ref MemoryMarshal.GetReference(span);
			ref var end = ref Unsafe.Add(ref start, span.Length);

			while (Unsafe.IsAddressLessThan(ref start, ref end))
			{
				this.operations.Add(start);
				start = ref Unsafe.Add(ref start, 1);
			}
		}
	}
}
