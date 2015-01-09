using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Zelda_Oracles_Suite
{
	public class Settings
	{
		public static bool Japanese { get; set; }
		public static string[] AgesNames { get; set; }
		public static string[] SeasonsNames { get; set; }
		public static bool Grayscale { get; set; }
		public static bool Zoom { get; set; }
		public static bool VisualStyles { get; set; }
		public static string ListsLocation { get; set; }
		public static bool AutoDownloadLists { get; set; }

		public static List<Patch[]> Patches { get; set; }

		private static void ApplyDefault()
		{
			//Japanese = true;
			AgesNames = new string[] { "Present Overworld", "Past Overworld", "Present Underwater", "Past Underwater", "Present Maku Path", "Past Maku Path", "Level 1 1F", "Level 2 B1", "Level 2 1F", "Level 3 B1", "Level 3 1F", "Level 4 B1", "Level 4 1F", "Level 5 B1", "Level 5 1F", "Hero's Cave B1", "Hero's Cave 1F", "Black Tower", "Level 6 Present 1F", "Level 6 Past B1", "Level 6 Past 1F", "Level 7 1F", "Level 7 2F", "Level 7 3F", "Level 8 B3", "Level 8 B2", "Level 8 B1", "Level 8 1F", "Indoor Big", "Final Dungeon", "Unmapped", "Unmapped" };
			SeasonsNames = new string[] { "Overworld Spring", "Overworld Summer", "Overworld Fall", "Overworld Winter", "Subrosia", "Hero's Cave", "Level 1 1F", "Level 2 1F", "Level 3 1F", "Level 3 2F", "Level 4 B2", "Level 4 B1", "Level 4 1F", "Level 5 1F", "Level 6 1F", "Level 6 2F", "Level 6 3F", "Level 6 4F", "Level 6 5F", "Hero's Cave", "Level 7 B2", "Level 7 B1", "Level 7 1F", "Level 8 B1", "Level 8 1F", "Final Dungeon", "Ganon's Dungeon", "Unmapped", "Unmapped" };
			Grayscale = true;
			Zoom = true;
			VisualStyles = true;
			ListsLocation = "http://www.zeldahacking.net/downloads/zohs/lists/";
			AutoDownloadLists = true;
		}

		public static bool LoadSettings()
		{
			ApplyDefault();
			if (!File.Exists("./settings.ini"))
			{
				Save();
				return true;
			}
			try
			{
				StreamReader sr = new StreamReader(File.OpenRead("./settings.ini"));
				string line;
				string category = "";
				while (!sr.EndOfStream)
				{
					line = sr.ReadLine();
					if (line.StartsWith("["))
					{
						category = line.Substring(1, line.Length - 2); 
						continue;
					}
					if (!line.Contains("="))
						continue;
					string[] parts = line.Split('=');
					LoadKey(category, parts[0], parts[1]);
				}
			}
			catch (Exception)
			{
				return false;
			}

			return true;
		}

		private static void LoadKey(string category, string key, string value)
		{
			switch (category)
			{
				case "ages_maps":
					AgesNames[int.Parse(key)] = value;
					break;

				case "seasons_maps":
					SeasonsNames[int.Parse(key)] = value;
					break;

				case "program":
					switch (key)
					{
						case "grayscale":
							if (value != "0")
								Grayscale = true;
							else
								Grayscale = false;
							break;
						case "zoom":
							if (value != "0")
								Zoom = true;
							else
								Zoom = false;
							break;
						case "visualstyles":
							if (value != "0")
								VisualStyles = true;
							else
								VisualStyles = false;
							break;
						case "listslocation":
							ListsLocation = value;
							break;
						case "autodownloadlists":
							if (value != "0")
								AutoDownloadLists = true;
							else
								AutoDownloadLists = false;
							break;
					}
					break;
			}
		}

		public static bool Save()
		{
			try
			{
				StreamWriter sw = new StreamWriter(File.Open("./settings.ini", FileMode.OpenOrCreate));
				sw.WriteLine("[program]");
				sw.WriteLine("zoom=" + (Zoom ? 1 : 0));
				sw.WriteLine("grayscale=" + (Grayscale ? 1 : 0));
				sw.WriteLine("visualstyles=" + (VisualStyles ? 1 : 0));
				sw.WriteLine("listslocation=" + ListsLocation);
				sw.WriteLine("autodownloadlists=" + (AutoDownloadLists ? 1 : 0));
				sw.WriteLine();

				sw.WriteLine("[ages_maps]");
				for (int i = 0; i < AgesNames.Length; i++)
					sw.WriteLine(i.ToString() + "=" + AgesNames[i]);
				sw.WriteLine();
				sw.WriteLine("[seasons_maps]");
				for (int i = 0; i < SeasonsNames.Length; i++)
					sw.WriteLine(i.ToString() + "=" + SeasonsNames[i]);
				sw.Close();
			}
			catch (Exception)
			{
				return false;
			}

			return true;
		}
	}
}
