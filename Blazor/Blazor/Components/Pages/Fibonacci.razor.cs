using System.Numerics;

namespace Blazor.Components.Pages
{
	public partial class Fibonacci
	{
		int num = 2;
		int Num
		{
			get => num;
			set => num = value < 2 ? 2 : value > 500 ? 500 : value;
		}

		BigInteger[] fib = Array.Empty<BigInteger>();
		void CalculateFib()
		{
			fib = new BigInteger[Num];

			fib[0] = 0;
			fib[1] = 1;


			for (int i = 2; i < Num; i++)
			{
				fib[i] = fib[i - 1] + fib[i - 2];
			}
		}
	}
}
