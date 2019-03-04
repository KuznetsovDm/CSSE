using System.Text;

// ReSharper disable InconsistentNaming

namespace Programmer
{
	public class Bar
	{
		private readonly bool[,,] _symbols;

		public int XLength => _symbols.GetLength(0);
		public int YLength => _symbols.GetLength(1);
		public int ZLength => _symbols.GetLength(2);

		public Bar(int x, int y, int z)
		{
			_symbols = Initialize(x, y, z);
		}

		private bool[,,] Initialize(int x, int y, int z)
		{
			var symbols = new bool[x, y, z];

			for (var i = 0; i < symbols.GetLength(0); i++)
			{
				for (var j = 0; j < symbols.GetLength(1); j++)
				{
					for (var k = 0; k < symbols.GetLength(2); k++)
					{
						symbols[i, j, k] = true;
					}
				}
			}

			return symbols;
		}

		public string GetXYSurface(int x, int y, int z)
		{
			var builder = new StringBuilder();

			var yLine = new string(' ', YLength + 1).Remove(y + 1, 1).Insert(y + 1, "y");
			builder.AppendLine(yLine);

			for (var i = 0; i < XLength; i++)
			{
				builder.Append(i == x ? 'x' : ' ');

				for (var j = 0; j < YLength; j++)
				{
					builder.Append(GetCharByZAndExist(z, _symbols[i, j, z]));
				}

				builder.AppendLine();
			}

			return builder.ToString();
		}

		public string GetZYSurface(int x, int y, int z)
		{
			var builder = new StringBuilder();

			var yLine = new string(' ', YLength + 1).Remove(y + 1, 1).Insert(y + 1, "y");
			builder.AppendLine(yLine);

			for (var k = 0; k < ZLength; k++)
			{
				builder.Append(k == z ? 'z' : ' ');

				for (var j = 0; j < YLength; j++)
				{
					builder.Append(_symbols[x, j, k] ? '9' : '0' );
				}

				builder.AppendLine();
			}

			return builder.ToString();
		}

		public void SetValue(int x, int y, int z, bool value = false)
		{
			_symbols[x, y, z] = value;
		}

		public bool GetValue(int x, int y, int z)
		{
			return _symbols[x, y, z];
		}

		private char GetCharByZAndExist(int z, bool exist)
		{
			return (ZLength - 1 - z - (exist ? 0 : 1)).ToString()[0];
		}
	}
}
