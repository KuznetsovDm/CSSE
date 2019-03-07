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

		private static int GetDelta(int current, int barLength)
		{
            return 1 + current > barLength ? 0 : 1;
		}
       
		public TickData TickVersion2()
		{
			var tickData = new TickData();
			var xMax = Settings.XMax > _bar.XLength ? _bar.XLength : Settings.XMax;
			var yMax = Settings.YMax > _bar.YLength ? _bar.YLength : Settings.YMax;
			var zMax = Settings.ZMax > _bar.ZLength ? _bar.ZLength : Settings.ZMax;

            var dx = GetDelta(_currentX, xMax);
			var dy = GetDelta(_currentY, yMax);
			var dz = GetDelta(_currentZ, zMax);

			tickData.DeletedPoint = Cut(_bar, _currentX, _currentY, _currentZ, dx, dy, dz).FirstOrDefault();

            if ((_currentY + _currentZ & 1) == 0)
            {
                if (_currentX < xMax - 1)
                    _currentX++;
                else if ((_currentZ & 1) == 0)
                {
                    if (_currentY < yMax - 1)
                        _currentY++;
                    else if (_currentZ < zMax - 1)
                    {
                        _currentZ++;
                    }
                    else
                    {
                        if (_currentX < xMax)
                            _currentX++;
                        else
                            tickData.Finished = true;
                    }      
                }
                else
                {
                    if (_currentY > 0)
                        _currentY--;
                    else if (_currentZ < zMax - 1)
                    {
                        _currentZ++;
                    }
                    else
                    {
                        if (_currentX < xMax)
                            _currentX++;
                        else
                            tickData.Finished = true;
                    }
                }
            }
            else
            {
                if (_currentX > 0)
                    _currentX--;
                else if ((_currentZ & 1) == 0)
                {
                    if (_currentY < yMax - 1)
                        _currentY++;
                    else if (_currentZ < zMax - 1)
                    {
                        _currentZ++;
                    }
                    else
                    {
                        if (_currentX > 0)
                            _currentX--;
                        else
                            tickData.Finished = true;
                    }
                }
                else
                {
                    if (_currentY > 0)
                        _currentY--;
                    else if (_currentZ < zMax - 1)
                    {
                        _currentZ++;
                    }
                    else
                    {
                        if (_currentX > 0)
                            _currentX--;
                        else
                            tickData.Finished = true;
                    }
                }
            }
			return tickData;
		}

		public void Restart()
		{
			_currentX = 0;
			_currentY = 0;
			_currentZ = 0;
		}

		private static IEnumerable<Point> Cut(Bar bar, int currentX, int currentY, int currentZ, int dx, int dy, int dz)
		{
            for (var k = currentZ; k < currentZ + dz; k++)
				for (var j = currentY; j < currentY + dy; j++)
                    for (var i = currentX; i < currentX + dx; i++)
                    {
						if (!bar.GetValue(i, j, k)) yield return null;
						bar.SetValue(i, j, k, false);
						yield return new Point(i, j, k);
					}
		}
	}
}
