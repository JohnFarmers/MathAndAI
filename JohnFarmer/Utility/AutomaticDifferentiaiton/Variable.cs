using JohnFarmer.Mathematics;
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

		public void Optimize(double learningRate = 1d) => value -= gradient * learningRate;

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

		public static implicit operator Matrix(Variable variable) => Convert.ChangeType(variable.value, typeof(Matrix));

		public static implicit operator Variable(int a) => new Variable(a);

		public static implicit operator Variable(float a) => new Variable(a);

		public static implicit operator Variable(double a) => new Variable(a);

		public static implicit operator Variable(long a) => new Variable(a);

		public static implicit operator Variable(Matrix a) => new Variable(a);
	}
}
