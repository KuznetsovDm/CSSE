namespace Programmer
{
	public class Programmator
	{
		private readonly Bar _bar;

		private int _currentX;
		private int _currentY;
		private int _currentZ;

		public ProgrammatorSettings Settings { get; set; }

		public Programmator(Bar bar, ProgrammatorSettings settings = null)
		{
			_bar = bar;
			Settings = settings ?? new ProgrammatorSettings();
		}

		public Point Tick()
		{
			int GetDelta(int current, int max, int barLength)
			{
				var left = barLength - current;
				return max > left ? left : max;
			}

			var dx = GetDelta(_currentX, Settings.XMax, _bar.XLength);
			var dy = GetDelta(_currentY, Settings.YMax, _bar.YLength);
			var dz = GetDelta(_currentZ, Settings.ZMax, _bar.ZLength);

			Cut(_bar, _currentX, _currentY, _currentZ, dx, dy, dz);

			if (dy != 0)
			{
				_currentY += dy;
			}
			else if (dx != 0 && _currentX + dx < _bar.XLength)
			{
				_currentX += dx;
				_currentY = 0;
			}
			else if (dz != 0 && _currentZ + dz < _bar.ZLength)
			{
				_currentZ += dz;
				_currentY = 0;
				_currentX = 0;
			}
			else
			{
				_currentZ = 0;
				_currentY = 0;
				_currentX = 0;
			}

			return new Point(_currentX, _currentY, _currentZ);
		}

		private static void Cut(Bar bar, int currentX, int currentY, int currentZ, int dx, int dy, int dz)
		{
			for (var i = currentX; i < currentX + dx; i++)
				for (var j = currentY; j < currentY + dy; j++)
					for (var k = currentZ; k < currentZ + dz; k++)
						bar.SetValue(i, j, k, false);
		}
	}
}
