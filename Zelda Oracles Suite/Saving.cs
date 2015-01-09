using System;
using System.Collections.Generic;
using System.IO;

namespace Zelda_Oracles_Suite
{
	public class Saving
	{
		public ROM ROM { get; set; }

		public Saving(ROM r)
		{
			ROM = r;
		}

		public void ApplyFirstOpens()
		{
			Expand();
			if (ROM.GameType == GameTypes.Ages)
			{
				//Graphics outside of bank 3F
				/*List<Patch.DataChange> gfx = new List<Patch.DataChange>();
				gfx.Add(new Patch.DataChange(0x3EF8, new byte[] { 0x7E, 0xFE, 0xFF, 0x20, 0x04, 0x23, 0x2A, 0x18, 0x02, 0xE6, 0x3F, 0xE0, 0xFE, 0x2A, 0x4F, 0x2A, 0xC9 }));
				gfx.Add(new Patch.DataChange(0x63D, new byte[] { 0xCD, 0xF8, 0x3E }));
				gfx.Add(new Patch.DataChange(0x3843, new byte[] { 0x7E }));
				gfx.Add(new Patch.DataChange(0x3847, new byte[] { 0xCD, 0x41, 0x3F }));
				gfx.Add(new Patch.DataChange(0x3F41, new byte[] { 0xCD, 0xF8, 0x3E, 0x2B, 0x79, 0xE0, 0x8C, 0xC9 }));
				gfx.Add(new Patch.DataChange(0x67B, new byte[] { 0xF0, 0xFE, 0x00 }));
				gfx.Add(new Patch.DataChange(0x1685, new byte[] { 0xCD, 0xF8, 0x3E }));
				Patch p = new Patch("256-Bank Graphics", "Edits a bunch of assembly to allow compressed graphics to be in all 256 banks.", "1.0", "Lin", "ZOHS", GameTypes.Ages, gfx.ToArray());
				p.Save("gfx256banks");

				gfx.Clear();
				//RST 20 - for both map types
				gfx.Add(new Patch.DataChange(0x20, new byte[] { 0xFA, 0x24, 0xCD, 0xC6, 0x40, 0xE0, 0x97, 0xEA, 0x22, 0x22, 0xFA, 0x2F, 0xCC, 0x21, 0x00, 0x40, 0xDF, 0xD7, 0xC9 }));
				//Maps with independent banks
				gfx.Add(new Patch.DataChange(0x3997, new byte[] { 0xCD, 0x09, 0x3F }));
				gfx.Add(new Patch.DataChange(0x3F09, new byte[] { 0x2A, 0x4F, 0x7E, 0xA1, 0xFE, 0xFF, 0x7E, 0xC0, 0xE7, 0x2A, 0xE0, 0x8C, 0xE8, 0x04, 0x2A, 0x4F, 0x7E, 0x47, 0xC5, 0xE8, 0xFE, 0xAF, 0x47, 0x4F, 0xC9 }));

				//Dungeon maps 256-bank no-compression
				gfx.Add(new Patch.DataChange(0x393D, new byte[] { 0xC3, 0x22, 0x3F }));
				gfx.Add(new Patch.DataChange(0x3F22, new byte[] { 0x2A, 0x66, 0x6F, 0x7C, 0xA5, 0xFE, 0xFF, 0xC2, 0x40, 0x39, 0xC1, 0xE7, 0x2A, 0xF5, 0x2A, 0x66, 0x6F, 0xF1, 0xE0, 0x97, 0xEA, 0x22, 0x22, 0x11, 0x00, 0xCF, 0x06, 0x0B, 0xC3, 0xE0, 0x06 }));

				byte[] ff = new byte[256 * 3];
				for (int i = 0; i < ff.Length; i++)
					ff[i] = 0xFF;
				for (int i = 0; i < 6; i++)
					gfx.Add(new Patch.DataChange((0x40 + i) * 0x4000, ff));

				p = new Patch("256-Bank Maps", "Edits a bunch of assembly to allow compressed maps to be in all 256 banks.", "1.0", "Lin", "ZOHS", GameTypes.Ages, gfx.ToArray());
				p.Save("map256banks");*/


			}
			else
			{

			}
			Save();

		}

		public void Expand()
		{
			byte[] newBuffer = new byte[0x400000];
			Array.Copy(ROM.Buffer, newBuffer, 0x100000);
			ROM.Buffer = newBuffer;
			//Fix the header
			ROM.WriteByte(0x148, 7); //Write new ROM size
			byte b = ROM.ReadByte(0x14D); //0x39 -> 0x38
			b -= 2;
			ROM.WriteByte(0x14D, b); //Fix checksum
		}

		public void WriteMapData(int group, int map, Room r)
		{
		Start:
			if (r.compressionType == 0xFF) //already decompressed)
			{
				ROM.WriteBytes(r.dataLocation, r.decompressed);
				return;
			}
			int address = ROM.FindFreeSpace(0x100000, (r.type == Room.RoomTypes.Small ? 80 : 176), -1);
			ROM.BufferLocation = (0x40 + r.area.in_mapGroup) * 0x4000 + map * 3;
			ROM.WriteByte((byte)(address / 0x4000));
			ROM.WriteByte((byte)(address & 0xFF));
			ROM.WriteByte((byte)(((address >> 8) & 0xFF) + 0x40));

			ROM.BufferLocation = r.pointerLocation;
			ROM.WriteWord(0xFFFF);
			r.dataLocation = address;
			r.compressionType = 0xFF;
			goto Start;
		}

		public bool Save()
		{
			try
			{
				BinaryWriter bw = new BinaryWriter(File.Open(ROM.Filename, FileMode.OpenOrCreate));
				bw.Write(ROM.Buffer);
				bw.Close();
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}
	}
}
