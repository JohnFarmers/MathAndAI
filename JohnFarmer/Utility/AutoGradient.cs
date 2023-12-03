using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Numerics;

namespace JohnFarmer.Utility
{
	public class AutoGradient
	{

	}

    public class Variable<T> where T : INumber<T>
    {
        public T value;
    }

    public class Expression<T> where T : INumber<T> 
    {
        
    }

    public struct Term<T>
	{
        public readonly T BaseValue;
        public readonly double Coefficient;
        public readonly double Power;

        public Term(T baseValue, double coefficient = 1, double power = 1)
        {
            BaseValue = baseValue;
            Coefficient = coefficient;
            Power = power;
        }

        public Term<T> Derivative { get => new(BaseValue, Coefficient * Power, Power - 1); }

		public override string ToString() => $"{Coefficient} * {BaseValue} ^ {Power}";
	}
}
