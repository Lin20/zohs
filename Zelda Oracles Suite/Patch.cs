using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace Zelda_Oracles_Suite
{
	public class Patch
	{
		public struct DataChange
		{
			public int Address;
			public byte[] Data;

			public DataChange(int address, byte[] data)
			{
				this.Address = address;
				this.Data = data;
			}

			public void Write(ROM r)
			{
				if (Address + Data.Length > r.Buffer.Length)
				{
					byte[] buffer = new byte[Address + Data.Length];
					Array.Copy(r.Buffer, buffer, r.Buffer.Length);
					r.Buffer = buffer;
				}
				r.WriteBytes(Address, Data);
			}
		}

		public string Name { get; set; }
		public string Description { get; set; }
		public string Version { get; set; }
		public string Author { get; set; }
		public string Category { get; set; }
		public GameTypes Compatibility { get; set; }
		public List<DataChange> Changes { get; set; }

		public Patch(string name, string description = "", string version = "1.0", string author = "Unknown", string category = "Other", GameTypes compatibility = GameTypes.Ages, params DataChange[] changes)
		{
			this.Name = name;
			this.Description = description;
			this.Compatibility = compatibility;
			this.Version = version;
			this.Author = author;
			this.Category = category;
			if (changes != null)
				Changes = new List<DataChange>(changes);
			else
				Changes = new List<DataChange>();
		}

		public bool Apply(ROM r)
		{
			if ((r.GameType & Compatibility) != 0)
			{
				foreach (DataChange d in Changes)
				{
					d.Write(r);
				}
			}
			else
				return false;
			return true;
		}

		public static Patch LoadPatch(ToolStripMenuItem menu, GameTypes t, string filename)
		{
			string name = Path.GetFileNameWithoutExtension(filename);
			string description = "";
			string version = "1.0";
			string author = "Unknown";
			string category = "Other";
			if (File.Exists(filename.Substring(0, filename.Length - 4) + ".desc"))
			{
				try
				{
					StreamReader sr = new StreamReader(filename.Substring(0, filename.Length - 4) + ".desc");
					string s = sr.ReadToEnd();
					sr.Close();
					string[] parts = s.Split('\t');
					name = parts[0];
					description = parts[1];
					version = parts[2];
					author = parts[3];
					category = parts[4];
				}
				catch (Exception)
				{

				}
			}

			BinaryReader br = new BinaryReader(File.OpenRead(filename));
			ROM r = new ROM(br.ReadBytes((int)br.BaseStream.Length), GameTypes.Ages, filename);
			r.BufferLocation += 5; //PATCH header

			List<DataChange> changes = new List<DataChange>();

			while (true)
			{
				if (r.BufferLocation + 3 >= r.Buffer.Length)
					break;
				int offset = (r.ReadByte() << 16) + (r.ReadByte() << 8) + (r.ReadByte());
				int size = (r.ReadByte() << 8) + r.ReadByte();
				if (size == 0) //RLE
				{
					size = (r.ReadByte() << 8) + r.ReadByte();
					byte insert = r.ReadByte();
					byte[] data = new byte[size];
					for (int i = 0; i < size; i++)
						data[i] = insert;
					changes.Add(new DataChange(offset, data));
					continue;
				}
				DataChange d = new DataChange(offset, r.ReadBytes(size));
				changes.Add(d);
			}

			Patch p = new Patch(name, description, version, author, category, t, changes.ToArray());
			if (menu == null)
				return p;
			ToolStripMenuItem parent = null;
			foreach (ToolStripMenuItem m in menu.DropDownItems)
			{
				if (m.Text.ToLower() == category.ToLower())
				{
					parent = m;
					goto AddChild;
				}
			}
			parent = new ToolStripMenuItem(category);
			menu.DropDownItems.Add(parent);
		AddChild:
			parent.DropDownItems.Add(new PatchMenuItem(p, name, "Author - " + author + "\nVersion - " + version + "\n" + description));
			return p;
		}

		public static void LoadPatches(ToolStripMenuItem menu, GameTypes type)
		{
			if (!Directory.Exists("./patches/" + (type == GameTypes.Ages ? "ages" : "seasons")))
			{
				Directory.CreateDirectory("./patches/" + (type == GameTypes.Ages ? "ages" : "seasons"));
				return;
			}

			foreach (string s in Directory.GetFiles("./patches/" + (type == GameTypes.Ages ? "ages" : "seasons")))
			{
				try
				{
					if (s.ToLower().EndsWith(".ips"))
						LoadPatch((ToolStripMenuItem)menu.DropDownItems[(int)type - 1], type, s);
				}
				catch (Exception)
				{
				}
			}
		}

		public Exception Save(string filename)
		{
			StreamWriter sw = null;
			BinaryWriter bw = null;
			try
			{
				bw = new BinaryWriter(File.Open("./patches/" + (Compatibility == GameTypes.Ages ? "ages" : "seasons") + "/" + filename + ".ips", FileMode.OpenOrCreate));
				bw.Write(new byte[] { 0x50, 0x41, 0x54, 0x43, 0x48 }); //PATCH
				foreach (DataChange d in Changes)
				{
					bw.Write((byte)((d.Address >> 16) & 0xFF));
					bw.Write((byte)((d.Address >> 8) & 0xFF));
					bw.Write((byte)(d.Address & 0xFF));

					bw.Write((byte)((d.Data.Length >> 8) & 0xFF));
					bw.Write((byte)(d.Data.Length & 0xFF));

					bw.Write(d.Data);
				}

				bw.Write(new byte[] { 0x45, 0x4F, 0x46 });
				bw.Flush();

				sw = new StreamWriter(File.Open("./patches/" + (Compatibility == GameTypes.Ages ? "ages" : "seasons") + "/" + filename + ".desc", FileMode.OpenOrCreate));
				sw.Write(Name + "\t");
				sw.Write(Description + "\t");
				sw.Write(Version + "\t");
				sw.Write(Author + "\t");
				sw.Write(Category + "\t");
				sw.Flush();
				sw.Close();
			}
			catch (Exception ex)
			{
				return ex;
			}
			finally
			{
				if (bw != null)
					bw.Close();
				if (sw != null)
					sw.Close();
			}

			return null;
		}
	}
}
