namespace Programmer
{
	public class Point
	{
		public Point(int x, int y, int z)
		{
			X = x;
			Y = y;
			Z = z;
		}

		public int X { get; }
		public int Y { get; }
		public int Z { get; }

		public override bool Equals(object obj)
		{
			if (!(obj is Point point)) return false;

			return X == point.X && Y == point.Y && Z == point.Z;
		}

		public bool Equals(Point other)
		{
			return X == other.X && Y == other.Y && Z == other.Z;
		}

		public override int GetHashCode()
		{
			unchecked
			{
				var hashCode = X;
				hashCode = (hashCode * 397) ^ Y;
				hashCode = (hashCode * 397) ^ Z;
				return hashCode;
			}
		}
	}
}
