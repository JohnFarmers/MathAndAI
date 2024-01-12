using System;
using JohnFarmer.Mathematics;

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

		public void Optimize() => value -= gradient;

		public void Optimize(double learningRate) => value -= gradient * learningRate;

		public static Operation operator +(Variable a, Variable b) => new Operation(a, b, a.value + b.value, b, a, OperationType.Add);

		public static Operation operator +(Variable a, dynamic b) => new Operation(a, b, a.value * b, b, a, OperationType.Add);

		public static Operation operator -(Variable a, Variable b) => new Operation(a, b, a.value * b.value, b, a, OperationType.Subtract);

		public static Operation	operator -(Variable a, dynamic b) => new Operation(a, b, a.value * b, b, a, OperationType.Subtract);

		public static Operation operator *(Variable a, Variable b) => new Operation(a, b, a.value * b.value, b, a, OperationType.Multiply);

		public static Operation operator *(Variable a, dynamic b) => new Operation(a, b, a.value * b, b, a, OperationType.Multiply);

		public static Operation operator /(Variable a, Variable b) => new Operation(a, b, a.value * b.value, b, a, OperationType.Divide);

		public static Operation operator /(Variable a, dynamic b) => new Operation(a, b, a.value * b, b, a, OperationType.Divide);

		public static Operation operator ^(Variable a, Variable b) => new Operation(a, b, a.value ^ b.value, b, a, OperationType.Power);

		public static Operation operator ^(Variable a, dynamic b) => new Operation(a, b, a.value * b, b, a, OperationType.Power);

		public static implicit operator int(Variable variable) => Convert.ChangeType(variable.value, typeof(int));

		public static implicit operator float(Variable variable) => Convert.ChangeType(variable.value, typeof(float));

		public static implicit operator double(Variable variable) => Convert.ChangeType(variable.value, typeof(double));

		public static implicit operator long(Variable variable) => Convert.ChangeType(variable.value, typeof(long));

		public static implicit operator Variable(int a) => new Variable(a);

		public static implicit operator Variable(float a) => new Variable(a);

		public static implicit operator Variable(double a) => new Variable(a);

		public static implicit operator Variable(long a) => new Variable(a);

		public static implicit operator Variable(Matrix a) => new Variable(a);

		public override string ToString() => Convert.ToString(value);
	}
}
