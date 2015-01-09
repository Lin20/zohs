using System;
using System.Collections.Generic;

namespace Zelda_Oracles_Suite
{
	public class LZ77
	{
		public static byte[] CompressGraphics(byte[] decompressed)
		{
			ROM raw = new ROM(decompressed, GameTypes.Ages);
			ROM buffer = new ROM(new byte[decompressed.Length + 1], GameTypes.Ages);

			while (raw.BufferLocation < buffer.Buffer.Length)
			{
				byte nextByte = raw.PeakByte();

			}

			return buffer.Buffer;
		}

		private static int FindBytes(ROM decompressed, int limit, byte[] find)
		{
			int last = -1;
			int at = decompressed.BufferLocation;
			decompressed.BufferLocation = 0;
			while (decompressed.BufferLocation < limit)
			{
				int findIndex = 0;
				findIndex++;
				while (decompressed.BufferLocation < limit)
				{
					if (decompressed.ReadByte() != find[findIndex++])
						break;
				}
			}

			return last;
		}
	}
}
