using System;
using System.Collections.Generic;
using System.Text;

namespace Zelda_Oracles_Suite
{
	public class Room
	{
		public enum RoomTypes
		{
			Small = 0,
			Dungeon = 1
		}

		public struct OverworldBlock
		{
			public byte id;
			public byte x;
			public byte y;
			public byte set;
			public byte type;
		}
		public struct DungeonBlock
		{
			public byte id;
			public byte x;
			public byte y;
			public byte size;
			public byte set;
			public byte type;
		}

		public byte[] decompressed;
		public OverworldBlock[] overworld;
		public DungeonBlock[] dungeon;
		public byte[] common;
		public bool[] bitsas1;
		public Area area;
		public RoomTypes type;
		public byte compressionType;
		public byte minorBank;
		public byte bank;
		public int relativeDataPointerM;
		public int relativeDataPointer;
		public int dataLocation;
		public int dictionaryLocation;
		public int pointerLocation;

		public byte mapIndex;
		public byte group;
		public byte season;


	}
}
