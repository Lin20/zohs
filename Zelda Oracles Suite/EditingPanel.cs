using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Zelda_Oracles_Suite
{
	public enum DrawTypes { Pencil, Line, Paintbrush }

	public partial class EditingPanel : UserControl
	{
		public byte[] PaintTiles { get; set; }
		public DrawTypes DrawType { get; set; }
		public bool Shift { get; set; }

		public EditingPanel()
		{
			InitializeComponent();

			PaintTiles = new byte[12];
		}

		public void LoadMap(Renderer renderer, int group, int map, int season)
		{
			DrawPaintTiles(renderer.GetTilesetFP(group, map, season));
		}

		public void DrawPaintTiles(FastPixel fp)
		{
			Bitmap bmp = new Bitmap(64, 48);
			FastPixel dest = new FastPixel(bmp);
			dest.Lock();
			fp.Lock();
			for (int i = 0; i < PaintTiles.Length; i++)
			{
				for (int x = 0; x < 16; x++)
				{
					for (int y = 0; y < 16; y++)
					{
						dest.SetPixel((i % 4) * 16 + x, (i / 4) * 16 + y, fp.GetPixel((PaintTiles[i] % 16) * 16 + x, (PaintTiles[i] / 16) * 16 + y));
					}
				}
			}
			fp.Unlock(false);
			dest.Unlock(true);
			pPainting.Image = bmp;
		}

		private void rbPencil_CheckedChanged(object sender, EventArgs e)
		{
			if(rbPencil.Checked)
				DrawType = DrawTypes.Pencil;
		}

		private void rbLine_CheckedChanged(object sender, EventArgs e)
		{
			if (rbLine.Checked)
				DrawType = DrawTypes.Line;
		}

		private void rbPaint_CheckedChanged(object sender, EventArgs e)
		{
			if (rbPaint.Checked)
				DrawType = DrawTypes.Paintbrush;
		}

		private void pPainting_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == System.Windows.Forms.MouseButtons.Left)
			{
				if (pPainting.Image == null)
					return;
				Graphics g = Graphics.FromImage(pPainting.Image);
				byte[,] tiles = (Shift ? pClipboard.GetSelectedIndexes() : pTileset.GetSelectedIndexes());
				int x = e.X / 16;
				int y = e.Y / 16;
				int width = 3;
				int height = 3;
				for (int xx = 0; xx < tiles.GetLength(0); xx++)
				{
					for (int yy = 0; yy < tiles.GetLength(1); yy++)
					{
						if (x + xx < 0 || y + yy < 0 || x + xx >= width || y + yy >= height)
							continue;
						byte t = (Shift ? Clipboard[tiles[xx, yy]] : tiles[xx, yy]);
						PaintTiles[(x + xx) + (y + yy) * width] = t;
						g.DrawImage(pTileset.Image, new Rectangle((x + xx) * 16, (y + yy) * 16, 16, 16), (t % 16) * 16, (t / 16) * 16, 16, 16, GraphicsUnit.Pixel);
					}
				}
				pPainting.Invalidate();
			}
		}
	}
}
