using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace JohnFarmer.Utility
{
	public class AutoGradient
	{
        private readonly List<PrimitiveOperation> operations = new List<PrimitiveOperation>();

		public void Record(PrimitiveOperation operation) => operations.Add(operation);

		public void Record(params PrimitiveOperation[] operations)
		{
			Span<PrimitiveOperation> span = operations.AsSpan();
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
