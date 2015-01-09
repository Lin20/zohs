using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Zelda_Oracles_Suite
{
	public partial class frmExport : Form
	{
		bool setClose = false;
		public frmExport()
		{
			InitializeComponent();
		}

		private void frmExportMaps_Load(object sender, EventArgs e)
		{
			setValue(0, pBar.Maximum);
		}

		public void setValue(int map, int max)
		{
			setPBarValue(map, max);
			setLabelText("Map " + map + "/" + max + " - " + (int)((decimal)((decimal)map / (decimal)max) * (decimal)100) + "%");
		}

		private void setPBarValue(int value, int max)
		{
			if (pBar.InvokeRequired)
			{
				pBar.BeginInvoke(new MethodInvoker(delegate() { setPBarValue(value, max); }));
			}
			else
			{
				if (pBar.Maximum != max)
					pBar.Maximum = max;
				pBar.Value = value;
			}
		}

		private void setLabelText(string text)
		{
			if (pBar.InvokeRequired)
			{
				pBar.BeginInvoke(new MethodInvoker(delegate() { setLabelText(text); }));
			}
			else
			{
				lblStatus.Text = text;
			}
		}

		public void close()
		{
			if (this.InvokeRequired)
			{
				this.BeginInvoke(new MethodInvoker(delegate() { close(); }));
			}
			else
			{
				this.Close();
			}
		}

		private void frmExportMaps_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (!setClose)
				e.Cancel = true;
		}
	}
}
