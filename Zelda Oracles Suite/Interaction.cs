using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Zelda_Oracles_Suite
{
	public class Interaction
	{
		public byte MainID { get; set; }
		public byte SubID { get; set; }
		public ushort ID { get { return (ushort)((MainID << 8) | SubID); } }
		public byte Type { get; set; }
		public byte[] Data { get; set; }

		public string DefinedType { get; set; }

		public void Draw(Graphics g)
		{

		}
	}
}
