using System;
using System.Drawing;

namespace Zelda_Oracles_Suite
{
	public class TilesetLoader
	{
		public ROM GB { get; set; }
		public byte[] RawTiles { get; set; }
		public byte[] FormattedTiles { get; set; }

		private Color[] bwPalette = new Color[] { Color.FromArgb(26 * 8, 23 * 8, 20 * 8), Color.FromArgb(15 * 8, 18 * 8, 20 * 8), Color.FromArgb(8 * 8, 9 * 8, 12 * 8), Color.FromArgb(4 * 8, 2 * 8, 0 * 8) };

		public TilesetLoader(ROM g)
		{
			GB = g;
		}

		public int GetVRAMBaseAddress(int id)
		{
			if (!Settings.Japanese)
				GB.BufferLocation = (GB.GameType == GameTypes.Ages ? 0x69DA : 0x6926) + id * 2;
			else
				GB.BufferLocation = (GB.GameType == GameTypes.Ages ? 0x69DC : 0x6926) + id * 2;
			return GB.ReadByte() + GB.ReadByte() * 0x100;
		}

		public int GetVRAMUniqueAddress(int id)
		{
			GB.BufferLocation = (GB.GameType == GameTypes.Ages ? 0x1B28 : 0x195E) + 0x10000 + id * 2;
			return GB.ReadByte() + (GB.ReadByte() - 0x40) * 0x100 + 0x10000;
		}

		public void LoadTileset(int index)
		{
			RawTiles = new byte[0x10000];

			//Load the header
			if (!Settings.Japanese)
				GB.BufferLocation = (GB.GameType == GameTypes.Ages ? 0x787E : 0x7964) + index * 2;
			else
				GB.BufferLocation = (GB.GameType == GameTypes.Ages ? 0x7868 : 0x7964) + index * 2;
			GB.BufferLocation = GB.ReadByte() + GB.ReadByte() * 0x100;

			bool loop = true;
			while (loop)
			{
				index = GB.ReadByte();
				byte bank = GB.ReadByte(); //FF8E
				int location = (GB.ReadByte() - 0x40) * 0x100 + GB.ReadByte();
				int destination = GB.ReadByte() * 0x100 + GB.ReadByte();
				destination &= 0xFFF0; //The 4 bits are used as the bank 
				int count = GB.ReadByte() * 0x100 + GB.ReadByte();
				loop = (count & 0x8000) != 0;
				count &= 0x7FFF;
				int temp = GB.BufferLocation;

				if (!Settings.Japanese)
					GB.BufferLocation = (GB.GameType == GameTypes.Ages ? 0x7870 : 0x794E) + index * 2;
				else
					GB.BufferLocation = (GB.GameType == GameTypes.Ages ? 0x785A : 0x794E) + index * 2;
				GB.BufferLocation = GB.ReadByte() + GB.ReadByte() * 0x100;
				byte ff8f = GB.ReadByte();
				int someaddress = (GB.ReadByte() * 0x100) + GB.ReadByte();

				GB.BufferLocation = (bank & 0x3F) * 0x4000 + location;

				int maskBitsLeft = 1;
				byte mask = 0;
				while (count > 0)
				{
					mask >>= 1;
					if (--maskBitsLeft == 0)
					{
						maskBitsLeft = 8;
						mask = GB.ReadByte();
					}
					if ((mask & 1) == 0)
					{
						RawTiles[destination++] = GB.ReadByte();
						count--;
					}
					else
					{
						int offset = 0;
						byte size = 0;
						if ((ff8f & 0x80) == 0) //8bit offset
						{
							offset = GB.ReadByte() + GB.ReadByte() * 0x100;
							size = (byte)(offset >> 12);
							offset &= 0x1FFF;
							offset += 3; //The game does this but it doesn't appear to make a difference
						}
						else //16bit offset
						{
							size = GB.ReadByte();
							offset = GB.ReadByte() + GB.ReadByte() * 0x100;
						}
						int temp2 = GB.BufferLocation;
						GB.BufferLocation = (ff8f & 0x3F) * 0x4000 + (someaddress - 0x4000) + offset;
						for (int i = 0; i < size; i++)
						{
							RawTiles[destination++] = GB.ReadByte();
							if (--count == 0)
								break;
						}
						GB.BufferLocation = temp2;
					}
				}

				GB.BufferLocation = temp;
			}
		}

		public void UnpackRawTiles()
		{
			byte bank = (byte)(GB.GameType == GameTypes.Ages ? 0x18 : 0x17);
			//373b - start of procedure
			FormattedTiles = new byte[0x10000];
			ushort destination = 0xD000; //destination
			ushort source = 0xDC00; //copy location
			ushort bc;
			for (int i = 0; i < 256; i++)
			{
				byte a = RawTiles[source++];
				bc = (ushort)(a + (RawTiles[source++] * 0x100));
				GB.BufferLocation = bank * 0x4000 + 4 + bc * 3;

				a = GB.ReadByte();
				bc = (ushort)a;
				a = GB.ReadByte();

				bc = (ushort)(bc + ((a >> 4) * 0x100));
				int temp = GB.BufferLocation;
				GB.BufferLocation = bank * 0x4000;
				GB.BufferLocation += GB.ReadByte() + ((GB.ReadByte() - 0x40) * 0x100);
				GB.BufferLocation += bc * 4;
				for (int k = 0; k < 4; k++)
					FormattedTiles[destination++] = GB.ReadByte();

				GB.BufferLocation = temp - 1;

				a = (byte)(GB.ReadByte() & 0xF);
				bc = (ushort)(a * 0x100 + GB.ReadByte());
				GB.BufferLocation = bank * 0x4000 + 2;
				GB.BufferLocation = bank * 0x4000 + GB.ReadByte() + ((GB.ReadByte() - 0x40) * 0x100);
				GB.BufferLocation += bc * 4;
				for (int k = 0; k < 4; k++)
					FormattedTiles[destination++] = GB.ReadByte();
			}
		}

		public byte[] LoadGraphics(int vram, int unique, GraphicsDecompressor gd)
		{
			byte[] decompressed = new byte[0x3000];
			gd.DecompressGraphics(GetVRAMBaseAddress(vram), ref decompressed);
			if (unique != 0)
				gd.DecompressGraphics(GetVRAMUniqueAddress(unique), ref decompressed);

			return decompressed;
		}

		public byte[, ,] UnpackGraphics(byte[] decompressed)
		{
			byte[, ,] output = new byte[768, 8, 8];
			GB.ReadTiles(32, 24, decompressed, ref output);
			return output;
		}

		public Bitmap drawTileset(byte[, ,] gfxData, bool past, Color[,] palette)
		{
			Bitmap b = new Bitmap(256, 256);
			FastPixel fp = new FastPixel(b);
			fp.rgbValues = new byte[256 * 256 * 4];
			fp.Lock();
			for (int i = 0; i < 256; i++)
			{
				for (int y = 0; y < 2; y++)
				{
					for (int x = 0; x < 2; x++)
					{
						int by = FormattedTiles[i * 8 + x + (y * 2) + 0xD000];
						int original = by;
						byte props = FormattedTiles[i * 8 + 4 + x + (y * 2) + 0xD000];
						if (by >= 0x80)
							by += 0x180;
						else
							by += 0x280;
						bool vflip = false;
						bool hflip = false;
						byte paletteIndex = (byte)(props & 0x7);
						if (original >= 16 && original < 48 && past && paletteIndex == 6)
							paletteIndex = 0;
						if (((props >> 4) & 2) != 0)
							hflip = true;
						if (((props >> 4) & 4) != 0)
							vflip = true;
						for (int yy = 0; yy < 8; yy++)
						{
							for (int xx = 0; xx < 8; xx++)
							{
								fp.SetPixel(((i % 16) * 16) + x * 8 + xx, ((i / 16) * 16) + y * 8 + yy, palette[paletteIndex, gfxData[by, hflip ? 7 - xx : xx, vflip ? 7 - yy : yy]]);
							}
						}
					}
				}
			}
			fp.Unlock(true);

			return b;
		}
	}
}
