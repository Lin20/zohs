using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Zelda_Oracles_Suite
{
	public class MapLoader
	{
		private ROM gb;
		private AreaLoader areaLoader;
		private byte[] ages26;
		private byte[] ages27;
		private byte[] seasons27;
		private byte[] seasons28;

		public MapLoader(ROM g, AreaLoader a)
		{
			gb = g;
			areaLoader = a;

			ages26 = new byte[64];
			ages27 = new byte[64];
			seasons27 = new byte[64];
			seasons28 = new byte[64];
			for (int y = 0; y < 8; y++)
			{
				for (int x = 0; x < 8; x++)
				{
					if (x < 5 && y == 0)
						ages26[x] = (byte)(x + 0xD0);
					if (y == 0)
						ages27[x] = (byte)(x);
					else if (y == 1 && x < 3)
						ages27[x + y * 8] = (byte)(x + y * 8);
					else if (y == 2 && x < 4)
						ages27[x + y * 8] = (byte)(0xAB + x);
					else if (y == 3 && x < 6)
						ages27[x + y * 8] = (byte)(0xF6 + x);
					if (y < 2)
						seasons27[x + y * 8] = (byte)(x + y * 8 + 0xE0);
					if (y < 2)
						seasons28[x + y * 8] = (byte)(x + y * 8);
					else if (y == 2 && x < 3)
						seasons28[x + y * 8] = (byte)(x + 0x10);
					else if (y > 2)
						seasons28[x + y * 8] = (byte)(x + y * 8 + 0x98);
				}
			}
		}

		public Room LoadedRoom { get; set; }

		public byte[] GetDungeonFormation(int indexFromZero)
		{
			if (gb.GameType == GameTypes.Ages && indexFromZero == 30)
				return ages26;
			if (gb.GameType == GameTypes.Ages && indexFromZero == 31)
				return ages27;
			if (gb.GameType == GameTypes.Seasons && indexFromZero == 27)
				return seasons27;
			if (gb.GameType == GameTypes.Seasons && indexFromZero == 28)
				return seasons28;
			int f = indexFromZero - (gb.GameType == GameTypes.Ages ? 4 : 5);
			gb.BufferLocation = (gb.GameType == GameTypes.Ages ? 0x4FCE : 0x4F41) + (f * 0x40);
			return gb.ReadBytes(0x40);
		}

		public byte GetFormationIndex(int indexFromZero, int map)
		{
			int f = indexFromZero - (gb.GameType == GameTypes.Ages ? 4 : 5);
			if (f < 0)
				return (byte)map;
			if (gb.GameType == GameTypes.Ages && indexFromZero == 30)
				return ages26[map];
			if (gb.GameType == GameTypes.Ages && indexFromZero == 31)
				return ages27[map];
			if (gb.GameType == GameTypes.Seasons && indexFromZero == 27)
				return seasons27[map];
			if (gb.GameType == GameTypes.Seasons && indexFromZero == 28)
				return seasons28[map];
			gb.BufferLocation = (gb.GameType == GameTypes.Ages ? 0x4FCE : 0x4F41) + (f * 0x40) + map;
			return gb.ReadByte();
		}

		public static int GetRealMapGroup(int groupIndex)
		{
			if (groupIndex < 4)
				return groupIndex;
			groupIndex -= 4;
			if (groupIndex >= 26)
				return 4 + (groupIndex & 1);
			if (groupIndex >= 14)
				return 5;
			else
				return 4;
		}

		public void DecompressSmall()
		{
			int ind = 0;
			LoadedRoom.decompressed = new byte[80];
			LoadedRoom.overworld = new Room.OverworldBlock[80];
			int compression = LoadedRoom.compressionType;

			gb.BufferLocation = LoadedRoom.dataLocation;
			//Compression types
			//2 - 16-bit
			//1 - 8-bit
			//0 - uncompressed

			if (compression != 0 && compression != 0xFF)
			{
				byte filler = 0;
				while (ind < 80)
				{
					LoadedRoom.overworld[ind] = new Room.OverworldBlock();
					int bitmap = gb.ReadByte() + (compression == 2 ? (gb.ReadByte() << 8) : 0);
					if (bitmap != 0)
						filler = gb.ReadByte();
					LoadedRoom.overworld[ind].x = (byte)(ind % 10);
					LoadedRoom.overworld[ind].y = (byte)(ind / 10);
					LoadedRoom.overworld[ind].set = (byte)((compression == 2 ? ind / 16 : ind / 8));
					for (int i = 0; i < (compression == 2 ? 16 : 8); i++)
					{
						if ((bitmap & 1) == 1)
						{
							LoadedRoom.decompressed[ind] = filler;
							LoadedRoom.overworld[ind].id = filler;
							LoadedRoom.overworld[ind].type = 1;
						}
						else
						{
							LoadedRoom.decompressed[ind] = gb.ReadByte();
							LoadedRoom.overworld[ind].id = LoadedRoom.decompressed[ind];
						}
						bitmap >>= 1;
						ind++;
					}
				}
			}
			else
			{
				LoadedRoom.decompressed = gb.ReadBytes(80);
				for (int i = 0; i < 80; i++)
				{
					LoadedRoom.overworld[i] = new Room.OverworldBlock();
					LoadedRoom.overworld[i].id = LoadedRoom.decompressed[i];
					LoadedRoom.overworld[i].x = (byte)(i % 10);
					LoadedRoom.overworld[i].y = (byte)(i / 10);
				}
			}
		}

		public void DecompressDungeon()
		{
			LoadedRoom.decompressed = new byte[176]; //the appearance is actually 15x11, but the game uses 0xb0 (16x11) for some reason.
			//3a27
			gb.BufferLocation = LoadedRoom.dataLocation; //(room.dataLocation % 0x4000) + room.bank * 0x4000;
			byte[] ce = gb.ReadBytes(0xB0);
			int srcIndex = 0;
			int destIndex = 0;
			byte left = 1;
			byte b = 0;
			LoadedRoom.dungeon = new Room.DungeonBlock[176];
			while (destIndex < 176)
			{
				//ld b,8
				left--;
				b >>= 1;
				LoadedRoom.dungeon[destIndex] = new Room.DungeonBlock();
				LoadedRoom.dungeon[destIndex].set = (byte)((destIndex - 1) / 8);
				if (left == 0)
				{
					left = 8;
					b = ce[srcIndex++];
				}
				if ((b & 1) == 0)
				{
					LoadedRoom.dungeon[destIndex].x = (byte)(destIndex % 16);
					LoadedRoom.dungeon[destIndex].y = (byte)(destIndex / 16);
					LoadedRoom.decompressed[destIndex++] = ce[srcIndex++];
					LoadedRoom.dungeon[destIndex - 1].id = LoadedRoom.decompressed[destIndex - 1];
					LoadedRoom.dungeon[destIndex - 1].size = 1;
					LoadedRoom.dungeon[destIndex - 1].type = 0;
					if (destIndex == 0xB0)
						break;
				}
				else
				{
					int srcD = ce[srcIndex++] + (ce[srcIndex++] * 0x100); //gb.BufferLocation = room.dictionaryLocation + ce[srcIndex++] + ((ce[srcIndex++] - 0x40) * 0x100);
					int size = (srcD >> 12) + 3; //ld a,d  swap a  and a,f. equal to >> 8, >> 4
					srcD &= 0xFFF;
					gb.BufferLocation = LoadedRoom.dictionaryLocation + srcD;
					for (int i = 0; i < size; i++)
					{
						LoadedRoom.dungeon[destIndex].x = (byte)(destIndex % 16);
						LoadedRoom.dungeon[destIndex].y = (byte)(destIndex / 16);
						LoadedRoom.decompressed[destIndex++] = gb.ReadByte();
						LoadedRoom.dungeon[destIndex - 1].id = LoadedRoom.decompressed[destIndex - 1];
						LoadedRoom.dungeon[destIndex - 1].size = (byte)size;
						LoadedRoom.dungeon[destIndex - 1].type = 1;
						if (destIndex == 0xB0)
							return;
					}
				}
			}
		}

		public bool LoadMapInformation(int index)
		{
			//3986
			gb.BufferLocation = 0x10000 + (gb.GameType == GameTypes.Ages ? 0x0F6C : 0x0C4C) + LoadedRoom.area.in_mapGroup * 8;
			byte roomType = gb.ReadByte();
			LoadedRoom.minorBank = gb.ReadByte();
			LoadedRoom.relativeDataPointerM = gb.ReadByte() + ((gb.ReadByte() - 0x40) * 0x100);
			LoadedRoom.bank = gb.ReadByte();
			LoadedRoom.relativeDataPointer = gb.ReadByte() + ((gb.ReadByte() - 0x40) * 0x100);

			if (roomType != 0) //Small map
			{
				LoadedRoom.type = Room.RoomTypes.Small;
				gb.BufferLocation = LoadedRoom.minorBank * 0x4000 + LoadedRoom.relativeDataPointerM + (index * 2);
				LoadedRoom.pointerLocation = gb.BufferLocation;
				int pointer = gb.ReadByte() + gb.ReadByte() * 0x100;
				if (pointer == 0xFFFF)
				{
					LoadedRoom.relativeDataPointer = 0;
					gb.BufferLocation = (0x40 + LoadedRoom.area.in_mapGroup) * 0x4000 + index * 3;
					LoadedRoom.bank = gb.ReadByte();
					pointer = (gb.ReadByte() + gb.ReadByte() * 0x100) & 0x3FFF;
					LoadedRoom.compressionType = 0xFF;
				}
				else
					LoadedRoom.compressionType = (byte)(pointer >> 14);
				gb.BufferLocation = LoadedRoom.bank * 0x4000 + (pointer & 0x3FFF);
				gb.BufferLocation += LoadedRoom.relativeDataPointer;
				LoadedRoom.dataLocation = gb.BufferLocation;
			}
			else
			{
				LoadedRoom.type = Room.RoomTypes.Dungeon;
				//3928 - dictionary loading
				gb.BufferLocation = LoadedRoom.minorBank * 0x4000 + LoadedRoom.relativeDataPointerM + 0x1000 + (index * 2);
				int dictionaryLocation = gb.BufferLocation - 0x1000 - (index * 2);
				LoadedRoom.pointerLocation = gb.BufferLocation;
				int pointer = gb.ReadByte() + gb.ReadByte() * 0x100; //Found to be below 4000
				if (pointer == 0xFFFF)
				{
					LoadedRoom.relativeDataPointer = 0;
					gb.BufferLocation = (0x40 + LoadedRoom.area.in_mapGroup) * 0x4000 + index * 3;
					LoadedRoom.dataLocation = gb.ReadByte() * 0x4000 + gb.ReadByte() + (gb.ReadByte() - 0x40) * 0x100;
					gb.BufferLocation = LoadedRoom.dataLocation;
					LoadedRoom.decompressed = gb.ReadBytes(0xB0);
					LoadedRoom.compressionType = 0xFF;
					return true;
				}
				if (pointer > 0x4000)
				{
					pointer -= 0x4000;
					LoadedRoom.bank++;
				}
				gb.BufferLocation = LoadedRoom.bank * 0x4000 + (ushort)((pointer + (LoadedRoom.relativeDataPointer + 0x4000)) + 0xFE00) - 0x4000;
				LoadedRoom.dataLocation = gb.BufferLocation;
				LoadedRoom.dictionaryLocation = dictionaryLocation;
				//gb.BufferLocation = room.minorBank * 0x4000 + gb.ReadByte() + ((gb.ReadByte() - 0x40) * 0x100);
			}

			return false;
		}

		public bool LoadMap(int index, int group, int season, int address = -1)
		{
			try
			{
				if (address == -1)
				{
					LoadedRoom = new Room();
					LoadedRoom.area = areaLoader.LoadArea(index, group, season);
					LoadedRoom.mapIndex = (byte)index;
					LoadedRoom.group = (byte)group;
					LoadedRoom.season = (byte)season;
					if (!LoadMapInformation(index))
					{
						if (LoadedRoom.type == Room.RoomTypes.Small)
							DecompressSmall();
						else
							DecompressDungeon();
					}
				}
				else
				{
					LoadedRoom.dataLocation = address;
					DecompressSmall();
				}
			}
			catch (Exception)
			{
				return false;
			}

			return true;
		}

		public Bitmap DrawMap(Bitmap srcTileset, bool[] flags)
		{
			if (srcTileset == null)
				return null;
			Bitmap b;
			if (LoadedRoom.type == Room.RoomTypes.Small)
				b = new Bitmap(160, 128);
			else
				b = new Bitmap(240, 176);
			FastPixel fp = new FastPixel(b);
			FastPixel src = new FastPixel(srcTileset);
			if (LoadedRoom.type == Room.RoomTypes.Small)
				fp.rgbValues = new byte[160 * 128 * 4];
			else
				fp.rgbValues = new byte[240 * 176 * 4];
			src.rgbValues = new byte[256 * 256 * 4];
			fp.Lock();
			src.Lock();
			//Graphics g = Graphics.FromImage(b);

			if (LoadedRoom.type == Room.RoomTypes.Small)
			{
				for (int y = 0; y < 8; y++)
				{
					for (int x = 0; x < 10; x++)
					{
						byte v = LoadedRoom.decompressed[x + y * 10];
						for (int yy = 0; yy < 16; yy++)
						{
							for (int xx = 0; xx < 16; xx++)
							{
								fp.SetPixel(x * 16 + xx, y * 16 + yy, src.GetPixel((v % 16) * 16 + xx, (v / 16) * 16 + yy));
							}
						}
						//g.DrawImage(srcTileset, new Rectangle(x * 16, y * 16, 16, 16), (room.decompressed[x + (y * 10)] % 16) * 16, (room.decompressed[x + (y * 10)] / 16) * 16, 16, 16, GraphicsUnit.Pixel);
					}
				}
			}
			else
			{
				for (int y = 0; y < 11; y++)
				{
					for (int x = 0; x < 15; x++)
					{
						byte v = LoadedRoom.decompressed[x + y * 16];
						for (int yy = 0; yy < 16; yy++)
						{
							for (int xx = 0; xx < 16; xx++)
							{
								fp.SetPixel(x * 16 + xx, y * 16 + yy, src.GetPixel((v % 16) * 16 + xx, (v / 16) * 16 + yy));
							}
						}
					}
				}
			}
			src.Unlock(false);
			fp.Unlock(true);

			return b;
		}

		public void DrawMap(ref FastPixel dest, int xStart, int yStart, FastPixel src, bool[] flags, FastPixel grayscale = null)
		{
			if (LoadedRoom.type == Room.RoomTypes.Small)
			{
				for (int y = 0; y < 8; y++)
				{
					for (int x = 0; x < 10; x++)
					{
						byte v = LoadedRoom.decompressed[x + y * 10];
						for (int yy = 0; yy < 16; yy++)
						{
							for (int xx = 0; xx < 16; xx++)
							{
								if (y == 7 && yy == 15 && xx % 8 >= 4)
								{
									dest.SetPixel(x * 16 + xx + xStart, y * 16 + yy + yStart, Color.White);
									if (grayscale != null)
										grayscale.SetPixel(x * 16 + xx + xStart, y * 16 + yy + yStart, Color.White);
								}
								else if (x == 9 && xx == 15 && yy % 8 >= 4)
								{
									dest.SetPixel(x * 16 + xx + xStart, y * 16 + yy + yStart, Color.White);
									if (grayscale != null)
										grayscale.SetPixel(x * 16 + xx + xStart, y * 16 + yy + yStart, Color.White);
								}
								else
								{
									Color c = src.GetPixel((v % 16) * 16 + xx, (v / 16) * 16 + yy);
									dest.SetPixel(x * 16 + xx + xStart, y * 16 + yy + yStart, c);
									if (grayscale != null)
									{
										int a = (c.R + c.G + c.B) / 3;
										grayscale.SetPixel(x * 16 + xx + xStart, y * 16 + yy + yStart, Color.FromArgb(a, a, a));
									}
								}
							}
						}
					}
				}
			}
			else
			{
				for (int y = 0; y < 11; y++)
				{
					for (int x = 0; x < 15; x++)
					{
						byte v = LoadedRoom.decompressed[x + y * 16];
						for (int yy = 0; yy < 16; yy++)
						{
							for (int xx = 0; xx < 16; xx++)
							{
								if (y == 10 && yy == 15 && xx % 8 >= 4)
								{
									dest.SetPixel(x * 16 + xx + xStart, y * 16 + yy + yStart, Color.White);
									if (grayscale != null)
										grayscale.SetPixel(x * 16 + xx + xStart, y * 16 + yy + yStart, Color.White);
								}
								else if (x == 14 && xx == 15 && yy % 8 >= 4)
								{
									dest.SetPixel(x * 16 + xx + xStart, y * 16 + yy + yStart, Color.White);
									if (grayscale != null)
										grayscale.SetPixel(x * 16 + xx + xStart, y * 16 + yy + yStart, Color.White);
								}
								else
								{
									Color c = src.GetPixel((v % 16) * 16 + xx, (v / 16) * 16 + yy);
									dest.SetPixel(x * 16 + xx + xStart, y * 16 + yy + yStart, c);
									if (grayscale != null)
									{
										int a = (c.R + c.G + c.B) / 3;
										grayscale.SetPixel(x * 16 + xx + xStart, y * 16 + yy + yStart, Color.FromArgb(a, a, a));
									}
								}
							}
						}
					}
				}
			}
		}
	}
}