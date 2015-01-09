using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Zelda_Oracles_Suite
{
	public class PaletteLoader
	{
		private ROM gb;

		public PaletteLoader(ROM g)
		{
			gb = g;
		}

		public Color[,] LoadPalette(int index)
		{
			Color[,] final = new Color[8, 4];
			Color[,] basicFour = gb.GetPalette((gb.GameType == GameTypes.Ages ? 0x5C8E0 : 0x58830)); //Always used
			byte[] ram = new byte[0x10000];
			for (int k = 0; k < 4; k++)
				final[0, k] = basicFour[0, k];

			int baseColorAddress = (gb.GameType == GameTypes.Ages ? (0x17 * 0x4000) : (0x16 * 0x4000));
			if(!Settings.Japanese)
			gb.BufferLocation = (gb.GameType == GameTypes.Ages ? 0x632C : 0x6290);
			else
				gb.BufferLocation = (gb.GameType == GameTypes.Ages ? 0x6334 : 0x6290);
			gb.BufferLocation += index * 2;
			gb.BufferLocation = gb.ReadByte() + gb.ReadByte() * 0x100;
			byte a;
			bool b = true;
			if (b) { }
			a = gb.ReadByte();
			if ((a & 0x80) == 0)
				b = false;

			int count = (a & 7) + 1;
			int d = 0xDE80 + (a & 0x78);
			int baseDest = d;
			gb.BufferLocation = baseColorAddress + gb.ReadByte() + ((gb.ReadByte() - 0x40) * 0x100);

			for (int k = 0; k < count * 8; k++)
			{
				ram[d++] = gb.ReadByte();
			}

			for (int i = 8 - count; i < count + (8 - count); i++)
			{
				for (int k = 0; k < 4; k++)
				{
					ushort value = (ushort)(ram[0xDE80 + i * 8 + k * 2] + (ram[0xDE80 + i * 8 + k * 2 + 1] << 8));
					final[i, k] = gb.GetRealColor(value);
				}
			}

			return final;
		}

		public int GetPaletteIndex(GameTypes game, int dest)
		{
			int baseColorAddress = (game == GameTypes.Ages ? (0x17 * 0x4000) : (0x16 * 0x4000));
			for (int i = 0; i < 255; i++)
			{
				gb.BufferLocation = (game == GameTypes.Ages ? 0x632C : 0x6290);
				gb.BufferLocation += i * 2;
				gb.BufferLocation = gb.ReadByte() + gb.ReadByte() * 0x100;

				byte a = gb.ReadByte();

				gb.BufferLocation = baseColorAddress + gb.ReadByte() + ((gb.ReadByte() - 0x40) * 0x100);
				if ((gb.BufferLocation % 0x4000) + 0x4000 == dest)
					return i;
			}
			return -1;
		}

		public int GetPaletteAddress(GameTypes game, int index)
		{
			int baseColorAddress = (game == GameTypes.Ages ? (0x17 * 0x4000) : (0x16 * 0x4000));
			gb.BufferLocation = (game == GameTypes.Ages ? 0x632C : 0x6290);
			gb.BufferLocation += index * 2;
			gb.BufferLocation = gb.ReadByte() + gb.ReadByte() * 0x100;

			byte a = gb.ReadByte();
			gb.BufferLocation = baseColorAddress + gb.ReadByte() + ((gb.ReadByte() - 0x40) * 0x100);

			return (gb.BufferLocation % 0x4000) + 0x4000;
		}
	}
}
