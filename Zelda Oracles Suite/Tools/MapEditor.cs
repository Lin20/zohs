using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Zelda_Oracles_Suite.Tools
{
	public partial class MapEditor : UserControl, MapEditingComponent
	{
		public MapEditor()
		{
			InitializeComponent();
		}
		
		public ROM ROM { get; set; }
		public int Group { get; set; }
        public int Index { get; set; }
        public byte Season { get; set; }
		public Room LoadedRoom { get; set; }
		public string Title { get; set; }
		public Image Icon { get; set; }

		private Point _lastMapPoint;

		#region EditingComponents

		public void MapChanged(ROM r, int group, int index, int season, Room room)
		{
			this.ROM = r;
			this.Group = group;
			this.Index = index;
            this.Season = (byte)season;
			this.LoadedRoom = room;

			_lastMapPoint = new Point(-1, -1);

			TilesetChanged();
		}

		public void MapMouseDraw(Graphics g, GridBox pMap)
		{
		}

		public void MapMouseDown(MouseEventArgs e, GridBox pMap)
		{
			/*if (Index == -1)
				return;
			if (e.Button == System.Windows.Forms.MouseButtons.Left)
			{
				int x = e.X / pMap.BoxSize.Width;
				int y = e.Y / pMap.BoxSize.Height;
				if (x == _lastMapPoint.X && y == _lastMapPoint.Y)
					return;

				Graphics g = Graphics.FromImage(pMap.Image);
				int width = LoadedRoom.type == Room.RoomTypes.Dungeon ? 16 : 10;
				int height = LoadedRoom.type == Room.RoomTypes.Dungeon ? 11 : 8;
				if (rbPencil.Checked || rbLine.Checked)
				{
					byte[,] tiles = (chkClipboard.Checked ? pClipboard.GetSelectedIndexes() : pTileset.GetSelectedIndexes());
					for (int xx = 0; xx < tiles.GetLength(0); xx++)
					{
						for (int yy = 0; yy < tiles.GetLength(1); yy++)
						{
							if (x + xx < 0 || y + yy < 0 || x + xx >= width || y + yy >= height)
								continue;
							byte t = (chkClipboard.Checked ? Clipboard[tiles[xx, yy]] : tiles[xx, yy]);
							LoadedRoom.decompressed[(x + xx) + (y + yy) * width] = t;
							g.DrawImage(pTileset.Image, new Rectangle((x + xx) * 16, (y + yy) * 16, 16, 16), (t % 16) * 16, (t / 16) * 16, 16, 16, GraphicsUnit.Pixel);
						}
					}

					_lastMapPoint = new Point(x, y);
					pMap.Invalidate();
				}
				if (rbLine.Checked)
				{
					_mapLinePoint = new Point(x, y);
					_mapPreviousImage = pMap.Image;
				}
			}
			if (e.Button == System.Windows.Forms.MouseButtons.Left || e.Button == System.Windows.Forms.MouseButtons.Right)
			{
				if (rbPaint.Checked)
				{
					int x = e.X / pMap.BoxSize.Width;
					int y = e.Y / pMap.BoxSize.Height;
					int width = LargeMap() ? 16 : 10;
					int height = LargeMap() ? 11 : 8;
					if (x < 0 || y < 0 || x >= width || y >= height)
						return;

					Graphics g = Graphics.FromImage(pMap.Image);
					if (e.Button == System.Windows.Forms.MouseButtons.Left)
					{
						g.DrawImage(pTileset.Image, new Rectangle(x * 16, y * 16, 16, 16), (PaintTiles[5] % 16) * 16, (PaintTiles[5] / 16) * 16, 16, 16, GraphicsUnit.Pixel);
						mapLoader.LoadedRoom.decompressed[x + y * width] = PaintTiles[5];
					}
					else
					{
						g.DrawImage(pTileset.Image, new Rectangle(x * 16, y * 16, 16, 16), 0, 0, 16, 16, GraphicsUnit.Pixel);
						mapLoader.LoadedRoom.decompressed[x + y * width] = 0;
					}
					_lastMapPoint = new Point(x, y);
					pMap.Invalidate();
				}
			}
			if (e.Button == System.Windows.Forms.MouseButtons.Middle)
			{
				int x = e.X / pMap.BoxSize.Width;
				int y = e.Y / pMap.BoxSize.Height;
				int width = LargeMap() ? 16 : 10;
				int height = LargeMap() ? 11 : 8;
				FillTile(x, y, mapLoader.LoadedRoom.decompressed[x + y * width], (byte)(chkClipboard.Checked ? Clipboard[pClipboard.SelectedIndex] : pTileset.SelectedIndex));
				pMap.Image = mapLoader.DrawMap((Bitmap)pTileset.Image, null);
			}
			if (e.Button == System.Windows.Forms.MouseButtons.Right)
			{
				if (rbPencil.Checked || rbLine.Checked)
				{
					chkClipboard.Checked = false;
					int width = LargeMap() ? 16 : 10;
					pTileset.SelectedIndex = mapLoader.LoadedRoom.decompressed[e.X / pMap.BoxSize.Width + e.Y / pMap.BoxSize.Height * width];
				}
			}*/
		}

		public void MapMouseMove(MouseEventArgs e, GridBox pMap)
		{
		}

		public void MapMouseUp(MouseEventArgs e, GridBox pMap)
		{
		}

		public void TilesetChanged()
		{
			if (Index == -1)
			{
				pTileset.Image = null;
				return;
			}
            if (Program.Window.Renderer != null)
                pTileset.Image = Program.Window.Renderer.GetTileset(LoadedRoom.area.index, Season);
		}

		public void UpdateMap()
		{
		}

		#endregion
	}
}
