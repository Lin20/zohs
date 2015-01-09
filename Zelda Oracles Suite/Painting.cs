using System;
using System.Collections.Generic;
using System.Text;

namespace Zelda_Oracles_Suite
{
	public class Painting
	{
		private static byte[] _map;
		private static int _width;
		private static int _height;
		private static byte[] _paintTiles;

		public static void Paint(ref byte[] map, byte[] paintTiles, int width, int height)
		{
			//0  1  2
			//3  4  5
			//6  7  8
			_map = new byte[map.Length];
			Array.Copy(map, _map, map.Length);
			_width = width;
			_height = height;
			_paintTiles = paintTiles;
			byte b = paintTiles[4];

			for (int x = 0; x < width; x++)
			{
				for (int y = 0; y < height; y++)
				{
					if (!CheckTile(x, y))
						continue;
					//0
					if (!CheckTile(x - 1, y) && !CheckTile(x, y - 1) && CheckTile(x + 1, y) && CheckTile(x, y + 1))
						map[x + y * width] = paintTiles[0];
					//1
					else if (CheckTile(x - 1, y) && !CheckTile(x, y - 1) && CheckTile(x + 1, y) && CheckTile(x, y + 1))
						map[x + y * width] = paintTiles[1];
					//2
					else if (CheckTile(x - 1, y) && !CheckTile(x, y - 1) && !CheckTile(x + 1, y) && CheckTile(x, y + 1))
						map[x + y * width] = paintTiles[2];
					//3
					else if (!CheckTile(x - 1, y) && CheckTile(x, y - 1) && CheckTile(x + 1, y) && CheckTile(x, y + 1))
						map[x + y * width] = paintTiles[3];
					//5
					else if (!CheckTile(x + 1, y) && CheckTile(x, y - 1) && CheckTile(x - 1, y) && CheckTile(x, y + 1))
						map[x + y * width] = paintTiles[5];
					//6
					else if (!CheckTile(x - 1, y) && CheckTile(x, y - 1) && CheckTile(x + 1, y) && !CheckTile(x, y + 1))
						map[x + y * width] = paintTiles[6];
					//7
					else if (CheckTile(x - 1, y) && CheckTile(x, y - 1) && CheckTile(x + 1, y) && !CheckTile(x, y + 1))
						map[x + y * width] = paintTiles[7];
					//8
					else if (CheckTile(x - 1, y) && CheckTile(x, y - 1) && !CheckTile(x + 1, y) && !CheckTile(x, y + 1))
						map[x + y * width] = paintTiles[8];
					else
						map[x + y * width] = paintTiles[4];
				}
			}
		}

		private static bool CheckTile(int x, int y)
		{
			if (x < 0 || y < 0 || x >= _width || y >= _height)
				return false;
			byte b = _map[x + y * _width];
			if (_paintTiles[0] == b)
				return true;
			if (_paintTiles[1] == b)
				return true;
			if (_paintTiles[2] == b)
				return true;
			if (_paintTiles[3] == b)
				return true;
			if (_paintTiles[4] == b)
				return true;
			if (_paintTiles[5] == b)
				return true;
			if (_paintTiles[6] == b)
				return true;
			if (_paintTiles[7] == b)
				return true;
			if (_paintTiles[8] == b)
				return true;
			return false;
		}

		private static byte GetTile(int x, int y)
		{
			if (x < 0 || y < 0 || x >= _width || y >= _height)
				return 0;
			return _map[x + y * _width];
		}
	}
}
