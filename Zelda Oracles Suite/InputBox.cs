using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;

using System.Text;
using System.Windows.Forms;

namespace Zelda_Oracles_Suite
{
	public partial class InputBox : Form
	{
		private bool sh = false;

		public InputBox(string text, string caption, int value = 0, bool useInt = true)
		{
			InitializeComponent();
			lblText.Text = text;
			this.Text = caption;
			sh = useInt;
			textBox1.Text = value.ToString("X");
		}

		public int Value
		{
			get
			{
				int i = 0;
				int.TryParse(textBox1.Text, System.Globalization.NumberStyles.HexNumber, null, out i);
				if (i < 0)
					i = 0;
				if (!sh && i > 255)
					i = 255;
				else if (i > 0xFFFFFF)
					i = 0xFFFFFF;
				return i;
			}
			set
			{
				textBox1.Text = ((int)value).ToString("X");
			}
		}

		private void InputBox_Load(object sender, EventArgs e)
		{

		}

		private void button1_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.OK;
			this.Close();
		}

		private void button2_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
			this.Close();
		}

		private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (!((e.KeyChar >= '0' && e.KeyChar <= '9') || (e.KeyChar >= 'a' && e.KeyChar <= 'f') || (e.KeyChar >= 'A' && e.KeyChar <= 'F')))
				e.Handled = true;

			if (e.KeyChar == (char)Keys.Enter || e.KeyChar == (char)Keys.Return)
			{
				button1_Click(sender, e);
			}
		}

		private void InputBox_Shown(object sender, EventArgs e)
		{
			textBox1.Focus();
			textBox1.SelectAll();
		}
	}
}
