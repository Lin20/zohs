using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Zelda_Oracles_Suite
{
	static class Program
	{
		public static frmMain Window { get; set; }

		public static ROM OpenROM { get { if (Window != null) return Window.ROM; return null; } }
		[STAThread]
		static void Main()
		{
			Settings.LoadSettings();
			if (Settings.VisualStyles)
				Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Window = new frmMain();
			Application.Run(Window);
		}
	}
}
