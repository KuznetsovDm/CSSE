using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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

		private static int GetDelta(int current, int max, int barLength)
		{
			var left = barLength - current;
			return max > left ? left : max;
		}

		public Point Tick()
		{
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

		public TickData TickVersion2()
		{
			//var deletedPoint = new Point(_currentX, _currentY, _currentZ);
			var tickData = new TickData();

			var xMax = Settings.XMax == 0 ? _bar.XLength : Settings.XMax;
			var yMax = Settings.YMax == 0 ? _bar.YLength : Settings.YMax;
			var zMax = Settings.ZMax == 0 ? _bar.ZLength : Settings.ZMax;

			var dx = GetDelta(_currentX, 1, xMax);
			var dy = GetDelta(_currentY, 1, yMax);
			var dz = GetDelta(_currentZ, 1, zMax);

			tickData.DeletedPoint = Cut(_bar, _currentX, _currentY, _currentZ, dx, dy, dz).FirstOrDefault();

			if (dy != 0)
			{
				_currentY += dy;
			}
			else if (dx != 0 && _currentX + dx < xMax)
			{
				_currentX += dx;
				_currentY = 0;
			}
			else if (dz != 0 && _currentZ + dz < zMax)
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
				tickData.Finished = true;
			}
			
			return tickData;
		}

		private static IEnumerable<Point> Cut(Bar bar, int currentX, int currentY, int currentZ, int dx, int dy, int dz)
		{
			for (var i = currentX; i < currentX + dx; i++)
				for (var j = currentY; j < currentY + dy; j++)
					for (var k = currentZ; k < currentZ + dz; k++)
					{
						if (!bar.GetValue(i, j, k)) yield return null;
						bar.SetValue(i, j, k, false);
						yield return new Point(i, j, k);
					}
		}
	}
}
