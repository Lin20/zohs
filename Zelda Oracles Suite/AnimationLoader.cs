using System;
using System.Collections.Generic;
using System.Text;

namespace Zelda_Oracles_Suite
{
	public class AnimationLoader
	{
		ROM gb;

		public AnimationLoader(ROM g)
		{
			gb = g;
		}

		public void loadAnimatedTiles(int animation, ref byte[] destination)
		{
			byte a;
			try
			{
				a = (byte)animation;
				if (a == 0xFF)
					return;
				byte bank = 4;
				int addr1 = (gb.GameType == GameTypes.Ages) ? 0x1B52 : 0x19B0;
				int addr2 = (gb.GameType == GameTypes.Ages) ? 0x1BE9 : 0x1A48;

				gb.BufferLocation = (bank * 0x4000) + addr1 + (a * 2);
				int data1 = bank * 0x4000 + gb.ReadByte() + ((gb.ReadByte() - 0x40) * 0x100);
				gb.BufferLocation = data1;

				int[] anim_speed = new int[4];
				int[] anim_addr = new int[4];
				int[] anim_number = new int[4];

				int tile_animation_flags = gb.ReadByte();
				for (int i = 0; i < 4; i++)
				{
					int a2_addr = bank * 0x4000 + gb.ReadByte() + ((gb.ReadByte() - 0x40) * 0x100);
					int temp = gb.BufferLocation;
					gb.BufferLocation = a2_addr;
					if (a2_addr >= bank * 0x4000 && a2_addr < 0x4000 + bank * 0x4000)
					{
						anim_speed[i] = gb.ReadByte();
						anim_addr[i] = a2_addr + 1;
					}
					else
					{
						if ((tile_animation_flags & (1 << i)) != 0)
						{
							anim_addr[i] = -1;
							anim_speed[i] = -1;
							tile_animation_flags &= ~(1 << i);
						}
					}
					gb.BufferLocation = temp;
				}

				for (int i = 0; i < 4; i++)
				{
					if ((tile_animation_flags & (1 << i)) != 0)
					{
						int addr = anim_addr[i];
						if (addr >= bank * 0x4000 && addr < 0x4000 + bank * 0x4000)
						{
							gb.BufferLocation = addr;
							int nextspeed = 0;
							int n;
							int frame_count = 0;

							while (nextspeed != 255)
							{
								n = gb.ReadByte();
								nextspeed = gb.ReadByte();
								frame_count++;
							}
							gb.BufferLocation -= 2;
							for (int frame = frame_count - 1; frame >= 0; frame--)
							{
								n = gb.ReadByte();
								nextspeed = gb.ReadByte();
								gb.BufferLocation -= 4;
								anim_number[i] = n;
								anim_speed[i] = nextspeed;
								int temp = gb.BufferLocation;

								gb.BufferLocation = bank * 0x4000 + addr2 + n * 6;
								byte src_bank = gb.ReadByte();
								int src_addr = src_bank * 0x4000 + ((gb.ReadByte() - 0x40) * 0x100) + gb.ReadByte();
								int dest_addr = gb.ReadByte() * 0x100 + gb.ReadByte();
								dest_addr &= ~0xF;
								int length = gb.ReadByte();
								gb.BufferLocation = src_addr;
								if (dest_addr >= 0x8000 && dest_addr < 0x9800)
								{
									int length16 = (length + 1) * 16;
									while (length16 > 0)
									{
										if (dest_addr == 0x28C0)
										{
											i = 0;
										}
										destination[dest_addr - 0x8000 + 0x1800] = gb.ReadByte();
										dest_addr++;
										length16--;
									}
								}
								gb.BufferLocation = temp;
							}
						}
						else
						{
							i = 0;
						}
					}
				}
			}
			catch (OutOfMemoryException)
			{
				System.Windows.Forms.MessageBox.Show(".");
			}
		}
	}
}
