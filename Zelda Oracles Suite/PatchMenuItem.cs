using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Zelda_Oracles_Suite
{
	public class PatchMenuItem : ToolStripMenuItem
	{
		public Patch Patch { get; set; }

		public PatchMenuItem(Patch p, string text, string tooltip)
			: base(text)
		{
			this.Patch = p;
			this.ToolTipText = tooltip;
			this.Click += new EventHandler(PatchMenuItem_Click);
		}

		private void PatchMenuItem_Click(object sender, EventArgs e)
		{
			try
			{
				if (Program.OpenROM != null)
				{
					if (!this.Patch.Apply(Program.OpenROM))
						MessageBox.Show("Patch uncompatible with ROM type.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					else
						MessageBox.Show("Patch successfully applied.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show("Error applying patch. " + ex.Message + "\n\n" + ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
	}
}
