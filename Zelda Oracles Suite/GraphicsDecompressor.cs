using System;
using System.Collections.Generic;

namespace Zelda_Oracles_Suite
{
	public class GraphicsDecompressor
	{
		public ROM GB { get; set; }

		public GraphicsDecompressor(ROM g)
		{
			GB = g;
		}

		public int GetVRAMBaseAddress(byte id)
		{
			GB.BufferLocation = (GB.GameType == GameTypes.Ages ? 0x69DA : 0x6926) + id * 2;
			return GB.ReadByte() + GB.ReadByte() * 0x100;
		}

		public int GetVRAMUniqueAddress(byte id)
		{
			GB.BufferLocation = (GB.GameType == GameTypes.Ages ? 0x1B28 : 0x195E) + 0x10000 + id * 2;
			return GB.ReadByte() + (GB.ReadByte() - 0x40) * 0x100 + 0x10000;
		}

		public void DecompressGraphics(int location, ref byte[] output)
		{
			GB.BufferLocation = location;
			try
			{
			LoadGFXFromLocation:
				byte gfxType = GB.ReadByte();
				byte bank = (byte)(gfxType & 0x3F);
				if (bank == 0)
					return;
				gfxType >>= 6;
				int graphicsAddress = (GB.ReadByte() - 0x40) * 0x100 + GB.ReadByte();
				int writeLocation = GB.ReadByte() * 0x100 + (GB.PeakByte() & 0xF0);
				byte vramBank = (byte)(GB.ReadByte() & 0x01);
				writeLocation &= 0x1FF0;
				writeLocation %= 0x1800;
				int tileCount = (byte)((GB.PeakByte() & 0x7F) + 1);
				int temp = GB.BufferLocation;
				GB.BufferLocation = graphicsAddress + (bank * 0x4000);

				if (gfxType == 0) //Load raw tile
				{
					for (; tileCount > 0; tileCount--)
					{
						for (int i = 0; i < 0x10; i++)
						{
							output[(vramBank * 0x1800) + writeLocation++] = GB.ReadByte();
						}
					}
				}
				else if (gfxType == 0x1 || gfxType == 0x3)
				{
					tileCount *= 16;
					bool flag = false; //flag = FF8E != 0
					if (gfxType == 0x3)
						flag = true;
					byte maskBitsLeft = 1; //FF8B
					byte mask = 0;
					while (tileCount > 0)
					{
						mask <<= 1; //The only reason this is up here is because the default value is 0, and at the end of the mask check,
						//the game adds it to itself (<< 1) and immediately checks bit 7, so this is doing the exact same thing.
						if (--maskBitsLeft == 0)
						{
							maskBitsLeft = 8;
							mask = GB.ReadByte();
						}
						if ((mask & 0x80) != 0)
						{
							byte shift1 = 0; //FF92
							byte shift2 = 0; //FF93
							int length = 0; //FF8F
							if (!flag)
							{
								shift1 = (byte)(GB.PeakByte() & 0x1F);
								//This part right here is really intimidating at first, but all the procedure is doing
								//is checking if the upper 3 bits aren't zero, and if they aren't then shift right 5 and add 1. The swap instruction is >> 4 and the
								//RRCA is doing the extra >> 1. It's optimal space usage.
								if ((GB.PeakByte() >> 5) == 0)
								{
									GB.ReadByte();
									length = GB.ReadByte();
									if (length == 0)
										length = 256;
								}
								else
								{
									length = (GB.ReadByte() >> 5) + 1;
								}
							}
							else
							{
								shift1 = GB.ReadByte();
								shift2 = (byte)(GB.PeakByte() & 7);
								if ((GB.PeakByte() & 0xF8) == 0)
								{
									GB.ReadByte();
									length = GB.ReadByte();
									if (length == 0)
										length = 256;
								}
								else
								{
									length = (GB.ReadByte() >> 3) + 2;
								}
							}

							short shift = (short)(shift1 + (shift2 << 8) + 1);
							int source = (vramBank * 0x1800) + writeLocation - shift;
							for (int i = 0; i < length; i++)
							{
								output[(vramBank * 0x1800) + writeLocation++] = output[source++];
								if (--tileCount == 0)
									break;
							}
						}
						else
						{
							output[(vramBank * 0x1800) + writeLocation++] = GB.ReadByte();
							tileCount--;
						}
					}
				}
				else
				{
					for (int k = 0; k < tileCount; k++)
					{
						ushort mask = (ushort)(GB.ReadByte() * 0x100 + GB.ReadByte());
						//The mask is a 16-bit insertion map that tells the game whether to insert the compression byte (1) or the next byte from the buffer (0)
						//The decompression starts by reading the highest bit and working its way down
						if (mask == 0) //We don't even need this, but the game has it and we'll see super duper tiny speed improvements, so why not
						{
							for (int i = 0; i < 16; i++)
							{
								output[(vramBank * 0x1800) + writeLocation++] = GB.ReadByte();
							}
						}
						else
						{
							byte insert = GB.ReadByte();
							for (int i = 0; i < 16; i++)
							{
								if ((mask & 0x8000) != 0)
									output[(vramBank * 0x1800) + writeLocation++] = insert;
								else
									output[(vramBank * 0x1800) + writeLocation++] = GB.ReadByte();
								mask <<= 1;
							}
						}
					}
				}

				GB.BufferLocation = temp;
				if ((GB.ReadByte() & 0x80) != 0)
					goto LoadGFXFromLocation;
			}
			catch (Exception)
			{
			}
		}
	}
}
