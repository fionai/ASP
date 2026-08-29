using System.Numerics;

namespace Blazor.Components.Pages
{
	public partial class Power
	{
		double num = 0;
		int deg = 1;
		double pow = 0;
		int Deg
		{
			get => deg;
			set => deg = value < 0 ? 0 : value > 50 ? 50 : value;
		}
		void CalculatePow()
		{
			pow = Math.Pow(num, deg);
		}
	}
}
