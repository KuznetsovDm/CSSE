using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Programmer
{
	public class Bar
	{
		private readonly bool[,,] _symbols;
		private readonly char _symbol;


		public Bar(int x, int y, int z, char symbol)
		{
			_symbols = Initialize(x, y, z);
			_symbol = symbol;
		}

		private bool[,,] Initialize(int x, int y, int z)
		{
			var symbols = new bool[x, y, z];

			for (int i = 0; i < symbols.GetLength(0); i++)
			{
				for (int j = 0; j < symbols.GetLength(1); j++)
				{
					for (int k = 0; k < symbols.GetLength(2); k++)
					{
						symbols[i, j, k] = i % 2 == 0;
					}
				}
			}

			return symbols;
		}

		public string GetHorizontalSurface(int z)
		{
			var builder = new StringBuilder();

			for (int i = 0; i < _symbols.GetLength(0); i++)
			{
				for (int j = 0; j < _symbols.GetLength(1); j++)
				{
					builder.Append(_symbols[i, j, z] ? _symbol : ' ');
				}

				builder.AppendLine();
			}

			return builder.ToString();
		}

		public string GetVerticalSurfuce(int x)
		{
			var builder = new StringBuilder();

			for (int j = 0; j < _symbols.GetLength(0); j++)
			{
				for (int k = 0; k < _symbols.GetLength(1); k++)
				{
					builder.Append(_symbols[x, j, k] ? _symbol : ' ');
				}

				builder.AppendLine();
			}

			return builder.ToString();
		}

		public void SetValue(int x, int y, int z, bool value = false)
		{
			_symbols[x, y, z] = value;
		}
	}
}
