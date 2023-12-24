namespace JohnFarmer.Utility
{
	public class PrimitiveOperation
	{
		public readonly dynamic a;
		public readonly dynamic b;
		public readonly dynamic result;
		public readonly dynamic dyda;
		public readonly dynamic dydb;

		private PrimitiveOperation(dynamic a, dynamic b, dynamic result, dynamic dyda, dynamic dydb)
		{
			this.a = a;
			this.b = b;
			this.result = result;
			this.dyda = dyda;
			this.dydb = dydb;
		}

		public static PrimitiveOperation Add(dynamic a, dynamic b) => new PrimitiveOperation(a, b, a + b, 1, 1);

		public static PrimitiveOperation Subtract(dynamic a, dynamic b) => new PrimitiveOperation(a, b, a - b, 1, 1);

		public static PrimitiveOperation Multiply(dynamic a, dynamic b) => new PrimitiveOperation(a, b, a * b, b, a);

		public static PrimitiveOperation Divide(dynamic a, dynamic b) => new PrimitiveOperation(a, b, a / b, 1 / b, -(a / (b ^ 2)));
	}
}
