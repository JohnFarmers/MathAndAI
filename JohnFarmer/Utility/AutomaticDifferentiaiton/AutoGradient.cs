using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace JohnFarmer.Utility
{
	public static class AutoGradient
	{
		public static void Backward(this Operation operation)
		{
			if (operation.aType == typeof(Operation) && operation.dyda != null)
				Backward(operation.a, operation.dyda);
			if (operation.bType == typeof(Operation) && operation.dydb != null)
				Backward(operation.b, operation.dydb);
			if (operation.aType == typeof(Variable) && operation.a != null && operation.a.requiredGrad && operation.dyda != null)
				operation.a.gradient = operation.dyda;
			if (operation.bType == typeof(Variable) && operation.b != null && operation.b.requiredGrad && operation.dydb != null)
				operation.b.gradient = operation.dydb;
		}
		
		public static void Backward(this Operation operation, dynamic accumulatedGradient = null)
		{
			accumulatedGradient ??= 1;
			if (operation.aType == typeof(Operation) && operation.dyda != null)
				Backward(operation.a, operation.dyda * accumulatedGradient);
			if (operation.bType == typeof(Operation) && operation.dydb != null)
				Backward(operation.b, operation.dydb * accumulatedGradient);
			if (operation.aType == typeof(Variable) && operation.a.requiredGrad && operation.dyda != null)
				operation.a.gradient = operation.dyda * accumulatedGradient;
			if (operation.bType == typeof(Variable) && operation.b.requiredGrad && operation.dydb != null)
				operation.b.gradient = operation.dydb * accumulatedGradient;
		}
	}
}
