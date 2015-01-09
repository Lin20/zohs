using System;
using System.Collections.Generic;
using System.Text;

namespace Zelda_Oracles_Suite
{
	public struct Area
	{
		public int palette;
		public int tileset;
		public int unique;
		public int vram;
		public int animation;
		public int flags1;
		public int flags2;
		public int in_mapGroup;
		public int index;
	}

	public class AreaLoader
	{
		ROM gb;
		public Area LastArea { get; set; }
		public AreaLoader(ROM g)
		{
			gb = g;
		}

		public Area LoadArea(int index, int group, int season)
		{
			//CC2D - map group
			//CC30 - map index
			//CD24 - map area
			//FF8D - area | 0x80, and later that & 0x7f - compression type?
			//FF8B - area

			int indexBase = 0x10000 + (gb.GameType == GameTypes.Ages ? 0x0F9C : 0x0C84);
			//4:6d7a - start of procedure
			gb.BufferLocation = 0x10000 + (gb.GameType == GameTypes.Ages ? 0x12D4 : 0x133C) + (group * 2);
			//6da1
			gb.BufferLocation = 0x10000 + gb.ReadByte() + ((gb.ReadByte() - 0x40) * 0x100) + index;
			byte a = gb.ReadByte();
			byte area = (byte)(a & 0x7F);
			gb.BufferLocation = indexBase + area * 8;
			if (gb.GameType == GameTypes.Seasons)
			{
				if (gb.ReadByte() == 0xFF)
				{
					gb.BufferLocation = 0x10000 + gb.ReadByte() + ((gb.ReadByte() - 0x40) * 0x100);
					gb.BufferLocation += season * 8;
				}
				else
					gb.BufferLocation--;
			}
			return LoadIntoArea(gb.BufferLocation, area);
		}

		public Area LoadArea(int area, int season = 0)
		{
			int indexBase = 0x10000 + (gb.GameType == GameTypes.Ages ? 0x0F9C : 0x0C84);
			gb.BufferLocation = indexBase + area * 8;
			if (gb.GameType == GameTypes.Seasons)
			{
				if (gb.ReadByte() == 0xFF)
				{
					gb.BufferLocation = 0x10000 + gb.ReadByte() + ((gb.ReadByte() - 0x40) * 0x100);
					gb.BufferLocation += season * 8;
				}
				else
					gb.BufferLocation--;
			}
			return LoadIntoArea(gb.BufferLocation, area);
		}

		public Area LoadIntoArea(int location, int index)
		{
			Area a = new Area();
			a.index = index;
			gb.BufferLocation = location;
			a.flags1 = gb.ReadByte();
			a.flags2 = gb.ReadByte();
			a.unique = gb.ReadByte();
			a.vram = gb.ReadByte();
			a.palette = gb.ReadByte();
			a.tileset = gb.ReadByte();
			if (gb.GameType == GameTypes.Ages)
			{
				if ((a.flags1 & 0x70) == 0 && (a.flags2 & 0x80) != 0 && (a.flags2 & 2) == 0)
					a.tileset += 0x100;
			}
			a.in_mapGroup = gb.ReadByte();
			a.animation = gb.ReadByte();
			LastArea = a;
			return a;
		}
	}
}