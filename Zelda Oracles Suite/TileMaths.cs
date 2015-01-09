using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Zelda_Oracles_Suite
{
	public class TileMaths
	{
		public static Point[] CalculatePoints(int x0, int y0, int x1, int y1)
		{
			List<Point> points = new List<Point>();
			points.Add(new Point(x0, y0));
			bool steep = Math.Abs(y1 - y0) > Math.Abs(x1 - x0);
			if (steep)
			{
				Swap(ref x0, ref y0);
				Swap(ref x1, ref y1);
			}
			if (x0 > x1)
			{
				Swap(ref x0, ref x1);
				Swap(ref y0, ref y1);
			}
			int deltax = x1 - x0;
			int deltay = Math.Abs(y1 - y0);
			int error = deltax / 2;
			int ystep;
			int y = y0;
			if (y0 < y1)
				ystep = 1;
			else
				ystep = -1;
			for (int x = x0; x <= x1; x++)
			{
				if (steep)
					points.Add(new Point(y, x));
				else
					points.Add(new Point(x, y));
				error = error - deltay;
				if (error < 0)
				{
					y = y + ystep;
					error = error + deltax;
				}
			}

			return points.ToArray();
		}

		private static void Swap(ref int a, ref int b)
		{
			int z = a;
			a = b;
			b = z;
		}
	}
}
