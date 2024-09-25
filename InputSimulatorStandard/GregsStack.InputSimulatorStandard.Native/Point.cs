using System.Drawing;

namespace GregsStack.InputSimulatorStandard.Native
{
	internal struct Point
	{
		public int X { get; set; }

		public int Y { get; set; }

		public static implicit operator System.Drawing.Point(Point point)
		{
			return new System.Drawing.Point(point.X, point.Y);
		}
	}
}
