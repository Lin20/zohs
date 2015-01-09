using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Net;

namespace Zelda_Oracles_Suite
{
	[DefaultEvent("StatusChanged")]
	public partial class DownloadableListBox : ComboBox
	{
		private string listName;
		[Browsable(true), DefaultValue("")]
		public string ListName { get { return listName; } set { listName = value; } }

		public delegate void StatusChangedEventHandler(object sender, StatusEventArgs e);
		public event StatusChangedEventHandler StatusChanged;

		private int forceIndexCount = 256;
		[Browsable(true), DefaultValue(256)]
		public int ForceIndexCount { get { return forceIndexCount; } set { forceIndexCount = Math.Max(1, value); ParseListText(); } }

		private string displayedCategory = "";
		[Browsable(true), DefaultValue("")]
		public string DisplayedCategory { get { return displayedCategory; } set { displayedCategory = value; ParseListText(); } }

		private string listText = "";

		public DownloadableListBox()
			: base()
		{
			InitializeComponent();
			this.DropDownStyle = ComboBoxStyle.DropDownList;
		}

		public void DownloadList()
		{
			try
			{
				if (!Directory.Exists("./lists"))
					Directory.CreateDirectory("./lists");
				WebClient wb = new WebClient();
				wb.DownloadFileCompleted += new AsyncCompletedEventHandler(wb_DownloadFileCompleted);
				wb.DownloadProgressChanged += new DownloadProgressChangedEventHandler(wb_DownloadProgressChanged);
				wb.DownloadFileAsync(new Uri(Settings.ListsLocation + listName + ".ini"), "./lists/" + listName + ".ini");
			}
			catch (Exception ex)
			{
				if (StatusChanged != null)
					StatusChanged(this, new StatusEventArgs(ex, "Failed to retrieve " + listName + ".ini"));
			}
		}

		private void wb_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
		{
			if (e.Cancelled && StatusChanged != null)
				StatusChanged(this, new StatusEventArgs(e.Error, "Download cancelled for " + listName + ".ini"));
			else if (e.Error != null && StatusChanged != null)
				StatusChanged(this, new StatusEventArgs(e.Error, "Failed to retrieve " + listName + ".ini"));
			else if (StatusChanged != null)
				StatusChanged(this, new StatusEventArgs(null, "Successfully retrieved " + listName + ".ini"));
			listText = "";
			ParseListText();
		}

		private void wb_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
		{
			if (StatusChanged != null)
				StatusChanged(this, new StatusEventArgs(null, "Downloading list " + listName + ".ini - " + (int)((double)e.BytesReceived / (double)e.TotalBytesToReceive * 100) + "%"));
		}

		public void ParseListText()
		{
			if (listText.Length == 0)
			{
				try
				{
				Load:
					if (File.Exists("./lists/" + listName + ".ini"))
					{
						StreamReader sr = new StreamReader(File.OpenRead("./lists/" + listName + ".ini"));
						listText = sr.ReadToEnd();
						sr.Close();
					}
					else if (Settings.AutoDownloadLists)
					{
						DownloadList();
						goto Load;
					}
				}
				catch (Exception)
				{

				}
			}

			Items.Clear();

			string[] cboLines = new string[forceIndexCount];
			if (listText.Length > 0)
			{
				string[] lines = listText.Replace("\r", "").Split('\n');
				string currentCategory = "";
				foreach (string line in lines)
				{
					if (line.Length == 0)
						continue;
					if (line.StartsWith("["))
					{
						currentCategory = line.Substring(1, line.Length - 2);
						continue;
					}
					if (displayedCategory != "" && currentCategory.ToLower() != displayedCategory.ToLower())
						continue;
					if (!line.Contains("="))
						continue;
					string[] parts = line.Split('=');
					if (parts.Length < 1)
						continue;

					int index = -1;
					int.TryParse(parts[0], System.Globalization.NumberStyles.HexNumber, null, out index);
					if (index > -1 && index < forceIndexCount)
					{
						string hex = index.ToString("X");
						if (hex.Length == 1)
							hex = "0" + hex;
						if (parts.Length > 1)
							cboLines[index] = hex + " = " + parts[1];
						else
							cboLines[index] = hex;
					}
				}
			}

			for (int i = 0; i < forceIndexCount; i++)
			{
				if (cboLines[i] == null || cboLines[i].Length == 0)
				{
					string hex = i.ToString("X");
					if (hex.Length == 1)
						hex = "0" + hex;
					cboLines[i] = hex;
				}
				Items.Add(cboLines[i]);
			}
		}
	}

	public class StatusEventArgs : EventArgs
	{
		public Exception Exception { get; set; }
		public string Status { get; set; }
		public StatusEventArgs()
			: base()
		{

		}

		public StatusEventArgs(Exception ex, string s)
			: this()
		{
			this.Exception = ex;
			this.Status = s;
		}
	}
}
