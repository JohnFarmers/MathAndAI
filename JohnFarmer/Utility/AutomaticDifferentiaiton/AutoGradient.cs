using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace JohnFarmer.Utility
{
	public class AutoGradient
	{
		public static void Backward(Operation operation)
		{
			if (operation.aType == typeof(Operation))
				Backward(operation.a, operation.dyda);
			if (operation.bType == typeof(Operation))
				Backward(operation.b, operation.dydb);
			if (operation.aType == typeof(Variable) && operation.a != null && operation.a.requiredGrad && operation.dyda != null)
				operation.a.gradient = operation.dyda;
			if (operation.bType == typeof(Variable) && operation.b != null && operation.b.requiredGrad && operation.dydb != null)
				operation.b.gradient = operation.dydb;
		}
		
		private static void Backward(Operation operation, dynamic gradient = null)
		{
			gradient = gradient != null ? gradient : 1;
			if (operation.aType == typeof(Operation))
				Backward(operation.a, operation.dyda * gradient);
			if (operation.bType == typeof(Operation))
				Backward(operation.b, operation.dydb * gradient);
			if (operation.aType == typeof(Variable) && operation.a != null && operation.a.requiredGrad && operation.dyda != null)
				operation.a.gradient = operation.dyda * gradient;
			if (operation.bType == typeof(Variable) && operation.b != null && operation.b.requiredGrad && operation.dydb != null)
				operation.b.gradient = operation.dydb * gradient;
		}
	}
}
