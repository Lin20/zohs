using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;

using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace Zelda_Oracles_Suite
{
	public partial class InterpolationPicturebox : PictureBox
	{
		public InterpolationPicturebox()
		{
			InitializeComponent();
		}

		InterpolationMode m = InterpolationMode.Default;
		public InterpolationMode InterpolationMode
		{
			get { return m; }
			set { m = value; this.Invalidate(); }
		}

		protected override void OnPaint(PaintEventArgs pe)
		{
			pe.Graphics.InterpolationMode = m;
			base.OnPaint(pe);
		}
	}
}
