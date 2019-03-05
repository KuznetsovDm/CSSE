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

			var yLine = new string(' ', YLength + 2).Remove(y + 1, 1).Insert(y + 1, "y");
			builder.AppendLine(yLine);


			for (var i = 0; i < XLength; i++)
			{
				builder.Append(i == x ? 'x' : ' ');

				for (var j = 0; j < YLength; j++)
				{
					builder.Append(GetCharByZAndExist(i, j));
				}

				builder.AppendLine();
			}

			if (x == XLength)
			{
				builder.Append('x');
			}

			return builder.ToString();
		}

		public bool[,] GetXYSurface(int z)
		{
			var result = new bool[XLength, YLength];
			for (var i = 0; i < XLength; i++)
			{
				for (var j = 0; j < YLength; j++)
				{
					result[i, j] = _symbols[i, j, z];
				}
			}

			return result;
		}

		public bool[,] GetZYSurface(int x)
		{
			var result = new bool[ZLength, YLength];
			for (var k = 0; k < ZLength; k++)
			{
				for (var j = 0; j < YLength; j++)
				{
					result[k, j] = _symbols[x, j, k];
				}
			}

			return result;
		}

		public string GetZYSurface(int x, int y, int z)
		{
			var builder = PrintKnife(y);

			for (var k = 0; k < ZLength; k++)
			{
				builder.Append(' ');
				builder.Append(' ');
				builder.Append(k == z ? 'z' : ' ');

				for (var j = 0; j < YLength; j++)
				{
					builder.Append(_symbols[x, j, k] ? '9' : ' ' );
				}

				builder.AppendLine();
			}

			return builder.ToString();
		}

		public void SetValue(int x, int y, int z, bool value)
		{
			_symbols[x, y, z] = value;
		}

		public bool GetValue(int x, int y, int z)
		{
			return _symbols[x, y, z];
		}

		private char GetCharByZAndExist(int x, int y)
		{
			var k = ZLength - 1;
			for (; k >= 0 ; k--)
			{
				if(!_symbols[x,y,k]) break;
			}

			return (ZLength - 2 - k).ToString()[0];
		}

		private StringBuilder PrintKnife(int y)
		{
			//| |
			//\ |
			// \|

			string GetLine(string str) { return new string(' ', YLength + 2 + str.Length).Remove(y, str.Length).Insert(y, str);}

			var builder = new StringBuilder();

			var firstLine = GetLine("| |");
			var secondLine = GetLine("\\ |");
			var thirdLine = GetLine(" \\|");

			builder.AppendLine(firstLine);
			builder.AppendLine(secondLine);
			builder.AppendLine(thirdLine);

			return builder;
		}
	}
}
