using System;

namespace JohnFarmer.Utility
{
	public class Variable
	{
		public dynamic value;
		public readonly Type type;

		public Variable(dynamic value)
		{
			this.value = value;
			this.type = value.GetType();
		}

		public static dynamic operator +(Variable a, Variable b) => a.value + b.value;

		public static dynamic operator +(Variable a, Constant b) => a.value + b.value;

		public static dynamic operator +(Variable a, dynamic b) => a.value + b;

		public static dynamic operator +(dynamic a, Variable b) => a + b.value;

		public static dynamic operator -(Variable a, Variable b) => a.value - b.value;

		public static dynamic operator -(Variable a, Constant b) => a.value - b.value;

		public static dynamic operator -(Variable a, dynamic b) => a.value - b;

		public static dynamic operator -(dynamic a, Variable b) => a - b.value;

		public static dynamic operator *(Variable a, Variable b) => a.value * b.value;

		public static dynamic operator *(Variable a, Constant b) => a.value * b.value;

		public static dynamic operator *(Variable a, dynamic b) => a.value * b;

		public static dynamic operator *(dynamic a, Variable b) => a * b.value;

		public static dynamic operator /(Variable a, Variable b) => a.value / b.value;

		public static dynamic operator /(Variable a, Constant b) => a.value / b.value;

		public static dynamic operator /(Variable a, dynamic b) => a.value / b;

		public static dynamic operator /(dynamic a, Variable b) => a / b.value;

		public override string ToString() => Convert.ToString(value);
	}
}
