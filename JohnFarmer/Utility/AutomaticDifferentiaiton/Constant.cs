using System;

namespace JohnFarmer.Utility
{
	public class Constant
	{
		public readonly dynamic value;

		public readonly Type type;

		public Constant(dynamic value)
		{
			this.value = value;
			this.type = value.GetType();
		}

		public static dynamic operator +(Constant a, Constant b) => a.value + b.value;

		public static dynamic operator +(Constant a, Variable b) => a.value + b.value;

		public static dynamic operator +(Constant a, dynamic b) => a.value + b;

		public static dynamic operator +(dynamic a, Constant b) => a + b.value;

		public static dynamic operator -(Constant a, Constant b) => a.value - b.value;

		public static dynamic operator -(Constant a, Variable b) => a.value - b.value;

		public static dynamic operator -(Constant a, dynamic b) => a.value - b;

		public static dynamic operator -(dynamic a, Constant b) => a - b.value;

		public static dynamic operator *(Constant a, Constant b) => a.value * b.value;

		public static dynamic operator *(Constant a, Variable b) => a.value * b.value;

		public static dynamic operator *(Constant a, dynamic b) => a.value * b;

		public static dynamic operator *(dynamic a, Constant b) => a * b.value;

		public static dynamic operator /(Constant a, Constant b) => a.value / b.value;

		public static dynamic operator /(Constant a, Variable b) => a.value / b.value;

		public static dynamic operator /(Constant a, dynamic b) => a.value / b;

		public static dynamic operator /(dynamic a, Constant b) => a / b.value;

		public override string ToString() => Convert.ToString(value);
	}
}
