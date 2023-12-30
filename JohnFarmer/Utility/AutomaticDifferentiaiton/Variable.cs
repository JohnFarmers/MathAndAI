using System;

namespace JohnFarmer.Utility
{
	public class Variable
	{
		public dynamic value;
		public readonly Type type;
		public readonly bool requiredGrad;
		public dynamic gradient;

		public Variable(dynamic value, bool requiredGrad = false)
		{
			this.value = value;
			this.type = value.GetType();
			this.requiredGrad = requiredGrad;
		}

		public static dynamic operator +(Variable a, Variable b) => a.value + b.value;

		public static dynamic operator +(Variable a, dynamic b) => a.value + b;

		public static dynamic operator -(Variable a, Variable b) => a.value - b.value;

		public static dynamic operator -(Variable a, dynamic b) => a.value - b;

		public static dynamic operator *(Variable a, Variable b) => a.value * b.value;

		public static dynamic operator *(Variable a, dynamic b) => a.value * b;

		public static dynamic operator /(Variable a, Variable b) => a.value / b.value;

		public static dynamic operator /(Variable a, dynamic b) => a.value / b;

		public override string ToString() => Convert.ToString(value);

		public static implicit operator int(Variable variable) => Convert.ChangeType(variable.value, typeof(int));

		public static implicit operator float(Variable variable) => Convert.ChangeType(variable.value, typeof(float));

		public static implicit operator double(Variable variable) => Convert.ChangeType(variable.value, typeof(double));

		public static implicit operator long(Variable variable) => Convert.ChangeType(variable.value, typeof(long));
	}
}
