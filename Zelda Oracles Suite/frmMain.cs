using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Zelda_Oracles_Suite
{
	public partial class frmMain : Form
	{
		public ROM ROM { get; set; }

		public Saving Saving { get; set; }
		public GraphicsDecompressor Decompressor { get; set; }
		public TilesetLoader tilesetLoader;
		public AreaLoader areaLoader;
		public PaletteLoader paletteLoader;
		public AnimationLoader animationLoader;
		public MapLoader mapLoader;
        public Renderer Renderer { get; set; }

		private bool _nMapDisable;
		private Point _lastMapPoint;
		private Point _lastClipboardPoint;
		private Point _mapLinePoint;
		private Image _mapPreviousImage;
		//private int _displayedMaps;

		private bool Shift { get; set; }
		public MapEditingComponent ActiveComponent { get; set; }

		#region Editing

		private byte[] Clipboard;
		private byte[] PaintTiles;

		#endregion

		public frmMain()
		{
			InitializeComponent();
			//tabsSecondary.TabPages.Add(new EditorTab(new InteractionEditor(), "Interactions"));
			tabsSecondary.TabPages.Add(new EditorTab(new Tools.MapEditor(), "Map EditorB"));
		}

		private void openROMToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (openDialogue.ShowDialog() != DialogResult.OK)
				return;
			try
			{
				if ((File.GetAttributes(openDialogue.FileName) & FileAttributes.ReadOnly) != 0)
				{
					if (MessageBox.Show("The ROM you selected is read-only. Would you like to fix it?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
					{
						File.SetAttributes(openDialogue.FileName, File.GetAttributes(openDialogue.FileName) ^ FileAttributes.ReadOnly);
					}
				}

				BinaryReader br = new BinaryReader(File.OpenRead(openDialogue.FileName));
				ROM = new ROM(br.ReadBytes((int)br.BaseStream.Length), GameTypes.Ages, openDialogue.FileName);
				br.Close();
			}
			catch (Exception)
			{
				MessageBox.Show("Error.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
			Initialize();
			lblROM.Text = "ROM: " + openDialogue.SafeFileName;
			hexBox1.Bytes = ROM.Buffer;

			//pWholeMap.Image = new Renderer(ROM, this).
			//DrawTiles();
			//pWholeMap.Size = pWholeMap.CanvasSize = pWholeMap.Image.Size;
			//pTileset.Image = new Renderer(ROM, this).GetTileset(2, 0);
		}

		private void Initialize()
		{
			if (ROM.ReadByte(0x13A) != 0x4E)
				ROM.GameType = GameTypes.Seasons;

			Saving = new Saving(ROM);
			if (ROM.Buffer.Length < 0x400000)
			{
				Saving.ApplyFirstOpens();
			}

			cboArea.Items.Clear();
			if (ROM.GameType == GameTypes.Ages)
				foreach (string s in Settings.AgesNames)
					cboArea.Items.Add(s);
			else
				foreach (string s in Settings.SeasonsNames)
					cboArea.Items.Add(s);

			Decompressor = new GraphicsDecompressor(ROM);
			areaLoader = new AreaLoader(ROM);
			mapLoader = new MapLoader(ROM, areaLoader);
			tilesetLoader = new TilesetLoader(ROM);
			paletteLoader = new PaletteLoader(ROM);
			animationLoader = new AnimationLoader(ROM);
			Renderer = new Renderer(ROM, this);
			Renderer.Grayscale = grayscaleMapToolStripMenuItem.Checked;

			pWholeMap.Enabled = true;
			ResetClipboard();

			cboArea.SelectedIndex = 0;
			_lastMapPoint = new Point(-1, -1);
			_lastClipboardPoint = new Point(-1, -1);
			PaintTiles = new byte[12];

			cboMusic.DisplayedCategory = (ROM.GameType == GameTypes.Ages ? "ages" : "seasons");
			cboMusic.ParseListText();
		}

		private void ResetClipboard()
		{
			Clipboard = new byte[16 * 9];
		}

		private void DrawClipboard(FastPixel fp)
		{
			Bitmap bmp = new Bitmap(256, 144);
			FastPixel dest = new FastPixel(bmp);
			dest.Lock();
			fp.Lock();
			for (int i = 0; i < Clipboard.Length; i++)
			{
				for (int x = 0; x < 16; x++)
				{
					for (int y = 0; y < 16; y++)
					{
						dest.SetPixel((i % 16) * 16 + x, (i / 16) * 16 + y, fp.GetPixel((Clipboard[i] % 16) * 16 + x, (Clipboard[i] / 16) * 16 + y));
					}
				}
			}
			fp.Unlock(false);
			dest.Unlock(true);
			pClipboard.Image = bmp;
		}

		

		private void DrawTiles()
		{
			Bitmap tilesBmp = new Bitmap(128, 128);
			Area a = areaLoader.LoadArea(0x63, 0, 0);
			Color[,] palette = paletteLoader.LoadPalette(a.palette);
			tilesetLoader.LoadTileset((byte)a.tileset);
			tilesetLoader.UnpackRawTiles();
			byte[] graphics = tilesetLoader.LoadGraphics(a.vram, a.unique, Decompressor);
			animationLoader.loadAnimatedTiles(a.animation, ref graphics);

			byte[, ,] gfx = tilesetLoader.UnpackGraphics(graphics);
			FastPixel fp = new FastPixel(tilesBmp);
			fp.Lock();
			for (int y = 0; y < 16; y++)
			{
				for (int x = 0; x < 16; x++)
				{
					for (int yy = 0; yy < 8; yy++)
					{
						for (int xx = 0; xx < 8; xx++)
						{
							int j = gfx[x + y * 16 + 0x200, xx, yy];
							fp.SetPixel(x * 8 + xx, y * 8 + yy, palette[2, j]);
						}
					}
				}
			}
			fp.Unlock(true);
			pMap.Image = tilesBmp;
		}

		public void LoadMap(int group = 0, int map = 0, bool fix = true, int address = -1)
		{
			pWholeMap.SelectedIndex = map;
			_nMapDisable = true;
			if (map == -1)
			{
				if (pMap.Visible)
					pWholeMap.Image = Renderer.DrawGroup(mapLoader, tilesetLoader, cboArea.SelectedIndex, null, cboArea.SelectedIndex % 4, false);
				pMap.Visible = false;
				nMap.Value = -1;
				pTileset.Image = null;
				pClipboard.Image = null;
				//pPainting.Image = null;
				nArea.Value = nVRAM.Value = nUnique.Value = nTileset.Value = nAnimation.Value = nPalette.Value;
				_nMapDisable = false;
				if (ActiveComponent != null)
					ActiveComponent.MapChanged(ROM, group, map, cboArea.SelectedIndex % 4, mapLoader.LoadedRoom);
				
				if (cboMusic.Items.Count > 0)
					cboMusic.SelectedIndex = 0;
				return;
			}
			if (!pMap.Visible && grayscaleMapToolStripMenuItem.Checked)
			{
				LoadWholeGroup();
				pWholeMap.Image = Renderer.DrawGroup(mapLoader, tilesetLoader, cboArea.SelectedIndex, null, cboArea.SelectedIndex % 4, true);
			}
			if (fix)
				map = mapLoader.GetFormationIndex(cboArea.SelectedIndex, map);
			mapLoader.LoadMap(map, group, cboArea.SelectedIndex % 4, address);
			if (address == -1)
				nAddress.Value = mapLoader.LoadedRoom.dataLocation;

			if (ActiveComponent != null)
				ActiveComponent.MapChanged(ROM, group, map, cboArea.SelectedIndex % 4, mapLoader.LoadedRoom);
			
			Bitmap tileset = Renderer.GetTileset(group, map, cboArea.SelectedIndex % 4);
			DrawClipboard(Renderer.GetTilesetFP(group, map, cboArea.SelectedIndex % 4));
			editingPanel1.LoadMap(Renderer, group, map, cboArea.SelectedIndex % 4); //DrawPaintTiles(Renderer.GetTilesetFP(group, map, cboArea.SelectedIndex % 4));
			nMap.Value = map;
			pTileset.Image = tileset;
			pMap.Image = mapLoader.DrawMap(tileset, null);
			PositionMap();
			pMap.Visible = true;

			ROM.BufferLocation = (ROM.GameType == GameTypes.Ages ? 0x1095C : 0x1083C) + group * 2;
			ROM.BufferLocation = 0x10000 + ROM.ReadByte() + ((ROM.ReadByte() - 0x40) * 0x100);
			ROM.BufferLocation += map;
			cboMusic.SelectedIndex = ROM.ReadByte();

			nArea.Value = areaLoader.LastArea.index;

			_nMapDisable = false;
		}

		public void PositionMap()
		{
			if (pMap.Image == null)
				return;
			pMap.ClientSize = new Size(pMap.Image.Width * (zoomSelectedMapToolStripMenuItem.Checked ? 2 : 1) - 1, pMap.Image.Height * (zoomSelectedMapToolStripMenuItem.Checked ? 2 : 1) - 1);
			pMap.Location = new Point(pnlMap.Width / 2 - pMap.Size.Width / 2, pnlMap.Height / 2 - pMap.Size.Height / 2);
			pMap.CanvasSize = new Size(pMap.Image.Size.Width * (zoomSelectedMapToolStripMenuItem.Checked ? 2 : 1), pMap.Image.Size.Height * (zoomSelectedMapToolStripMenuItem.Checked ? 2 : 1));
			pMap.BoxSize = new System.Drawing.Size((zoomSelectedMapToolStripMenuItem.Checked ? 32 : 16), (zoomSelectedMapToolStripMenuItem.Checked ? 32 : 16));
		}

		public bool LargeMap()
		{
			return MapConstants.LargeMap(ROM.GameType, MapConstants.GetRealGroup(ROM.GameType, cboArea.SelectedIndex));
		}

		private void frmMain_Load(object sender, EventArgs e)
		{
			if (!Settings.LoadSettings())
				MessageBox.Show("Error loading settings.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			zoomSelectedMapToolStripMenuItem.Checked = Settings.Zoom;
			grayscaleMapToolStripMenuItem.Checked = Settings.Grayscale;
			Patch.LoadPatches(patchesToolStripMenuItem, GameTypes.Ages);
			Patch.LoadPatches(patchesToolStripMenuItem, GameTypes.Seasons);
			hexBox1.GrayOnFocusLost = true;
		}

		private void LoadWholeGroup()
		{
			if (Renderer.CachedGroups[cboArea.SelectedIndex] == null)
			{
				frmExport form = new frmExport();
				new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(ChangeGroup)).Start(new object[] { cboArea.SelectedIndex, form, cboArea.SelectedIndex % 4, false });
				form.ShowDialog();
			}
			if (Renderer.Grayscale && Renderer.CachedGrayscaleGroups[cboArea.SelectedIndex] == null)
			{
				frmExport form = new frmExport();
				new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(ChangeGroup)).Start(new object[] { cboArea.SelectedIndex, form, cboArea.SelectedIndex % 4, true });
				form.ShowDialog();
			}
		}

		private void cboArea_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (ROM == null)
				return;
			LoadWholeGroup();
			pWholeMap.Image = Renderer.CachedGroups[cboArea.SelectedIndex];
			pWholeMap.Size = pWholeMap.CanvasSize = pWholeMap.Image.Size;
			if (LargeMap())
				pWholeMap.BoxSize = new Size(240, 176);
			else
				pWholeMap.BoxSize = new Size(160, 128);
			pMap.Visible = false;
			pWholeMap.Location = new Point(0, 0);
			LoadMap(MapConstants.GetRealGroup(ROM.GameType, cboArea.SelectedIndex), -1);
		}

		private void ChangeGroup(object p)
		{
			if (ROM == null)
				return;
			Renderer.DrawGroup(mapLoader, tilesetLoader, (int)((object[])p)[0], (frmExport)((object[])p)[1], (int)((object[])p)[2], (bool)((object[])p)[3]);
		}

		private void pWholeMap_MouseDown(object sender, MouseEventArgs e)
		{
			if (ROM == null)
				return;
			if (e.Button == System.Windows.Forms.MouseButtons.Left)
			{
				LoadMap(MapConstants.GetRealGroup(ROM.GameType, cboArea.SelectedIndex), pWholeMap.SelectedIndex);
			}
			else
			{
				LoadMap(0, -1);
			}
		}

		private void nMap_ValueChanged(object sender, EventArgs e)
		{
			if (ROM == null)
				return;
			if (_nMapDisable)
				return;
			LoadMap(MapConstants.GetRealGroup(ROM.GameType, cboArea.SelectedIndex), (int)nMap.Value, false);
			if (LargeMap())
			{
				byte[] formation = mapLoader.GetDungeonFormation(cboArea.SelectedIndex);
				for (int i = 0; i < formation.Length; i++)
				{
					if (formation[i] == (int)nMap.Value)
					{
						pWholeMap.SelectedIndex = i;
						return;
					}
				}
				pWholeMap.SelectedIndex = -1;
			}
		}

		private void pMap_MouseMove(object sender, MouseEventArgs e)
		{
			int x = e.X / pMap.BoxSize.Width;
			int y = e.Y / pMap.BoxSize.Height;
			lblHoverPos.Text = "X: " + x.ToString("X") + "  Y: " + y.ToString("X") + "  YX: " + y.ToString("X") + x.ToString("X");

			if (x == _lastMapPoint.X && y == _lastMapPoint.Y)
				return;
			if (ActiveComponent != null)
			{
				ActiveComponent.MapMouseMove(e, pMap);
				pMap.Invalidate();
				return;
			}
			/*if (e.Button == MouseButtons.Left)
			{
				if (rbPencil.Checked || rbPaint.Checked)
				{
					pMap_MouseDown(sender, e);
				}
				else if (rbLine.Checked)
				{
					int x1 = _mapLinePoint.X;
					int y1 = _mapLinePoint.Y;
					int x2 = x;
					int y2 = y;
					Point[] points = TileMaths.CalculatePoints(x1, y1, x2, y2);
					int width = LargeMap() ? 16 : 10;
					int height = LargeMap() ? 11 : 8;
					pMap.Image = new Bitmap(_mapPreviousImage);
					Graphics g = Graphics.FromImage(pMap.Image);
					foreach (Point p in points)
					{
						byte[,] tiles = (chkClipboard.Checked ? pClipboard.GetSelectedIndexes() : pTileset.GetSelectedIndexes());
						for (int xx = 0; xx < tiles.GetLength(0); xx++)
						{
							for (int yy = 0; yy < tiles.GetLength(1); yy++)
							{
								int pX = points[0].X + ((p.X - points[0].X) / (chkClipboard.Checked ? pClipboard.SelectionRectangle.Width : pTileset.SelectionRectangle.Width)) * (chkClipboard.Checked ? pClipboard.SelectionRectangle.Width : pTileset.SelectionRectangle.Width);
								int pY = points[0].Y + ((p.Y - points[0].Y) / (chkClipboard.Checked ? pClipboard.SelectionRectangle.Height : pTileset.SelectionRectangle.Height)) * (chkClipboard.Checked ? pClipboard.SelectionRectangle.Height : pTileset.SelectionRectangle.Height);
								if (pX + xx < 0 || pY + yy < 0 || pX + xx >= width || pY + yy >= height)
									continue;
								byte t = (chkClipboard.Checked ? Clipboard[tiles[xx, yy]] : tiles[xx, yy]);
								mapLoader.LoadedRoom.decompressed[(pX + xx) + (pY + yy) * width] = t;
								g.DrawImage(pTileset.Image, new Rectangle((pX + xx) * 16, (pY + yy) * 16, 16, 16), (t % 16) * 16, (t / 16) * 16, 16, 16, GraphicsUnit.Pixel);
							}
						}
					}
					_lastMapPoint = new Point(x, y);
					pMap.Invalidate();
				}
			}
			else if (e.Button == System.Windows.Forms.MouseButtons.Right && rbPaint.Checked)
				pMap_MouseDown(sender, e);*/
		}

		private void frmMain_Resize(object sender, EventArgs e)
		{
			if (pMap.Visible)
			{
				pMap.Location = new Point(pnlMap.Width / 2 - pMap.Size.Width / 2, pnlMap.Height / 2 - pMap.Size.Height / 2);
			}
			if (pWholeMap != null)
			{
				if (pWholeMap.Right < pnlMap.Width)
					pWholeMap.Left = pnlMap.Width - pWholeMap.Width - 1;
				if (pWholeMap.Bottom < pnlMap.Height)
					pWholeMap.Top = pnlMap.Height - pWholeMap.Height - 1;
			}
		}

		private void nArea_ValueChanged(object sender, EventArgs e)
		{
			if (ROM == null || nMap.Value == -1)
				return;
			bool b = _nMapDisable;
			_nMapDisable = true;
			nVRAM.Value = areaLoader.LastArea.vram;
			nTileset.Value = areaLoader.LastArea.tileset;
			nUnique.Value = areaLoader.LastArea.unique;
			nAnimation.Value = areaLoader.LastArea.animation;
			nPalette.Value = areaLoader.LastArea.palette;
			_nMapDisable = b;

			mapLoader.LoadedRoom.area.index = (int)nArea.Value;

			if (_nMapDisable)
				return;

			FastPixel tileset = Renderer.GetTilesetFP((int)nArea.Value, cboArea.SelectedIndex % 4);
			//pTileset.Image = tileset.Bitmap;
			DrawClipboard(tileset);
			pMap.Image = mapLoader.DrawMap(tileset.Bitmap, null);

			if (ActiveComponent != null)
				ActiveComponent.TilesetChanged();
		}

		private void fullscreenToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
		{
			if (fullscreenToolStripMenuItem.Checked)
			{
				this.WindowState = FormWindowState.Normal;
				this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
				this.WindowState = FormWindowState.Maximized;
			}
			else
			{
				this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
				this.WindowState = FormWindowState.Normal;
			}
		}

		private void zoomSelectedMapToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
		{
			if (ROM == null)
				return;
			PositionMap();
		}

		private void nVRAM_ValueChanged(object sender, EventArgs e)
		{
			if (ROM == null)
				return;
			if (_nMapDisable)
				return;
			FastPixel tileset = Renderer.GetTilesetFP((int)nVRAM.Value, (int)nUnique.Value, (int)nTileset.Value, (int)nAnimation.Value, (int)nPalette.Value);
			pTileset.Image = tileset.Bitmap;
			DrawClipboard(tileset);
			pMap.Image = mapLoader.DrawMap(tileset.Bitmap, null);
		}

		private void pMap_MouseDown(object sender, MouseEventArgs e)
		{
			if (ActiveComponent != null)
			{
				ActiveComponent.MapMouseDown(e, pMap);
				pMap.Invalidate();
				return;
			}
			/*if (e.Button == System.Windows.Forms.MouseButtons.Left)
			{
				int x = e.X / pMap.BoxSize.Width;
				int y = e.Y / pMap.BoxSize.Height;
				if (x == _lastMapPoint.X && y == _lastMapPoint.Y)
					return;

				Graphics g = Graphics.FromImage(pMap.Image);
				int width = LargeMap() ? 16 : 10;
				int height = LargeMap() ? 11 : 8;
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
							mapLoader.LoadedRoom.decompressed[(x + xx) + (y + yy) * width] = t;
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

		private void FillTile(int x, int y, byte search, byte selectedTile)
		{
			int width = LargeMap() ? 16 : 10;
			int height = LargeMap() ? 11 : 8;

			if (x < 0 || y < 0 || x >= width || y >= height)
				return;

			if (mapLoader.LoadedRoom.decompressed[x + y * width] == selectedTile)
				return;
			mapLoader.LoadedRoom.decompressed[x + y * width] = selectedTile;
			if (y > 0)
			{
				if (mapLoader.LoadedRoom.decompressed[x + (y - 1) * width] == search)
					FillTile(x, y - 1, search, selectedTile);
			}
			if (x > 0)
			{
				if (mapLoader.LoadedRoom.decompressed[x - 1 + y * width] == search)
					FillTile(x - 1, y, search, selectedTile);
			}
			if (x < width - 1)
			{
				if (mapLoader.LoadedRoom.decompressed[x + 1 + y * width] == search)
					FillTile(x + 1, y, search, selectedTile);
			}
			if (y < height - 1)
			{
				if (mapLoader.LoadedRoom.decompressed[x + (y + 1) * width] == search)
					FillTile(x, y + 1, search, selectedTile);
			}
		}

		private void pClipboard_MouseDown(object sender, MouseEventArgs e)
		{
			if (nMap.Value == -1)
				return;
			if (e.Button == System.Windows.Forms.MouseButtons.Left)
			{
				Graphics g = Graphics.FromImage(pClipboard.Image);
				byte[,] tiles = (Shift ? pClipboard.GetSelectedIndexes() : pTileset.GetSelectedIndexes());
				int x = e.X / 16;
				int y = e.Y / 16;
				int width = pClipboard.CanvasSize.Width / 16;
				int height = pClipboard.CanvasSize.Height / 16;
				for (int xx = 0; xx < tiles.GetLength(0); xx++)
				{
					for (int yy = 0; yy < tiles.GetLength(1); yy++)
					{
						if (x + xx < 0 || y + yy < 0 || x + xx >= width || y + yy >= height)
							continue;
						byte t = (Shift ? Clipboard[tiles[xx, yy]] : tiles[xx, yy]);
						Clipboard[(x + xx) + (y + yy) * width] = t;
						g.DrawImage(pTileset.Image, new Rectangle((x + xx) * 16, (y + yy) * 16, 16, 16), (t % 16) * 16, (t / 16) * 16, 16, 16, GraphicsUnit.Pixel);
					}
				}
				pClipboard.Invalidate();
			}
		}

		private void frmMain_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Shift)
				Shift = true;
			editingPanel1.Shift = Shift;
			if (ROM != null && e.KeyCode == Keys.Escape)
			{
				LoadMap(0, -1);
				e.Handled = true;
			}
		}

		private void frmMain_KeyUp(object sender, KeyEventArgs e)
		{
			if (!e.Shift)
				Shift = false;
			editingPanel1.Shift = Shift;
		}

		private void pMap_MouseUp(object sender, MouseEventArgs e)
		{
			_lastMapPoint = new Point(-1, -1);
			_mapLinePoint = new Point(-1, -1);
			if (ActiveComponent != null)
			{
				ActiveComponent.MapMouseUp(e, pMap);
				pMap.Invalidate();
				return;
			}
			/*if (rbPaint.Checked)
			{
				int width = LargeMap() ? 16 : 10;
				int height = LargeMap() ? 11 : 8;
				Painting.Paint(ref mapLoader.LoadedRoom.decompressed, PaintTiles, width, height);
				pMap.Image = mapLoader.DrawMap((Bitmap)pTileset.Image, null);
			}*/
		}

		private void pPainting_MouseDown(object sender, MouseEventArgs e)
		{
			/*if (e.Button == System.Windows.Forms.MouseButtons.Left)
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
			}*/
		}

		private void grayscaleMapToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
		{
			if (Renderer != null)
			{
				Renderer.Grayscale = grayscaleMapToolStripMenuItem.Checked;
			}
		}

		private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
		{
			Settings.Zoom = zoomSelectedMapToolStripMenuItem.Checked;
			Settings.Grayscale = grayscaleMapToolStripMenuItem.Checked;
			Settings.Save();
		}

		private void toolStripButton2_Click(object sender, EventArgs e)
		{
			if (ROM == null)
				return;
			if ((int)nMap.Value != -1)
			{
				Saving.WriteMapData(MapConstants.GetRealGroup(ROM.GameType, cboArea.SelectedIndex), (int)nMap.Value, mapLoader.LoadedRoom);
				nAddress.Value = mapLoader.LoadedRoom.dataLocation;

				int mapWidth = (!LargeMap() ? 160 : 240);
				int mapHeight = (!LargeMap() ? 128 : 176);
				//Graphics g = Graphics.FromImage(renderer.CachedGroups[cboArea.SelectedIndex]);
				//g.DrawImage(mapLoader.DrawMap(renderer.GetTileset((int)nArea.Value, cboArea.SelectedIndex % 4), null), pWholeMap.SelectedIndex % (LargeMap() ? 8 : 16) * mapWidth, pWholeMap.SelectedIndex / (LargeMap() ? 8 : 16) * mapHeight);
				Renderer.UpdateMap(cboArea.SelectedIndex, pWholeMap.SelectedIndex, mapLoader);
				pWholeMap.Image = (Renderer.Grayscale ? Renderer.CachedGrayscaleGroups[cboArea.SelectedIndex] : Renderer.CachedGroups[cboArea.SelectedIndex]);
			}
			hexBox1.Invalidate();

			//Saving.Save();
		}

		private void nAddress_ValueChanged(object sender, EventArgs e)
		{
			if (ROM == null || _nMapDisable)
				return;
			LoadMap(cboArea.SelectedIndex, (int)nMap.Value, false, (int)nAddress.Value);
		}

		private void dumpMapRangesToolStripMenuItem_Click(object sender, EventArgs e)
		{
			/*SaveFileDialog s = new SaveFileDialog();
			s.Title = "Dump Map Ranges";
			s.Filter = "Text Files|*.txt|All Files|*.*";
			if (s.ShowDialog() != System.Windows.Forms.DialogResult.OK)
				return;*/

		}

		private void exportHugePNGToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (ROM == null || nMap.Value == -1)
				return;
			int mSquare = 8;
			FastPixel src = new FastPixel(new Bitmap(pTileset.Image));
			src.Lock();

			Bitmap bmp = new Bitmap(160 * mSquare + mSquare, 128 * mSquare + mSquare);
			FastPixel dest = new FastPixel(bmp);

			int map = (int)nMap.Value;
			int group = MapConstants.GetRealGroup(ROM.GameType, cboArea.SelectedIndex);

			for (int i = 0; i < 100; i++)
			{
				dest.Lock();
				for (int y = 0; y < mSquare; y++)
				{
					for (int x = 0; x < mSquare; x++)
					{
						mapLoader.LoadMap(map, group, 0, 0xB0000 + (i * mSquare * mSquare) + x + (y * mSquare));
						mapLoader.DrawMap(ref dest, x * 160 + 1, y * 128 + 1, src, null);
					}
				}

				dest.Unlock(true);
				bmp.Save("E:\\Ages Dumps\\" + i + "_" + mapLoader.LoadedRoom.compressionType + ".png", System.Drawing.Imaging.ImageFormat.Png);
			}

			MessageBox.Show("Done.");
		}

		private void pMap_Paint(object sender, PaintEventArgs e)
		{
			if (ActiveComponent != null)
			{
				ActiveComponent.MapMouseDraw(e.Graphics, pMap);
				return;
			}
		}

		private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (ROM == null)
				return;
			if (tabsSecondary.SelectedTab is EditorTab)
			{
				ActiveComponent = ((EditorTab)tabsSecondary.SelectedTab).EditingComponent;
				ActiveComponent.MapChanged(ROM, MapConstants.GetRealGroup(ROM.GameType, cboArea.SelectedIndex), (int)nMap.Value, cboArea.SelectedIndex % 4, mapLoader.LoadedRoom);
			}
			else
				ActiveComponent = null;
			if (pMap.Visible)
				pMap.Invalidate();
		}

		private void gotoAddressToolStripMenuItem_Click(object sender, EventArgs e)
		{
			InputBox i = new InputBox("Enter address to navigate to:", "Hex Editor Goto", hexBox1.SelectionIndex / 2);
			if (i.ShowDialog() != System.Windows.Forms.DialogResult.OK)
				return;
			hexBox1.Goto(i.Value);
			hexBox1.Focus();
		}

		private void downloadableListStatusChanged(object sender, StatusEventArgs e)
		{
			this.Invoke((MethodInvoker)delegate
			{
				lblNetworkStatus.Text = e.Status;
			});
		}
	}
}
