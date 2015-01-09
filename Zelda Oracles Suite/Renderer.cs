using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Zelda_Oracles_Suite
{
	public class Renderer
	{
		ROM gb;
		frmMain form;

		public FastPixel[,] CachedTilesets { get; set; }
		public Bitmap[] CachedGroups { get; set; }
		public Bitmap[] CachedGrayscaleGroups { get; set; }

		public bool Grayscale { get; set; }

		public Renderer(ROM g, frmMain f)
		{
			gb = g;
			form = f;
			CachedTilesets = new FastPixel[4, 256];
			CachedGroups = new Bitmap[32];
			CachedGrayscaleGroups = new Bitmap[32];
		}

		public Bitmap DrawGroup(MapLoader m, TilesetLoader t, int cboGroup, frmExport form, int season = 0, bool grayscaleRender = false)
		{
			if (!grayscaleRender && CachedGroups[cboGroup] != null)
				return CachedGroups[cboGroup];
			else if (grayscaleRender && CachedGrayscaleGroups[cboGroup] != null)
				return CachedGrayscaleGroups[cboGroup];
			Bitmap bmp;
			Bitmap grayscale;
			int group = MapConstants.GetRealGroup(gb.GameType, cboGroup);
			if (!MapConstants.LargeMap(gb.GameType, group))
				bmp = new Bitmap(2560, 2048);
			else
				bmp = new Bitmap(1920, 1408);
			grayscale = new Bitmap(bmp.Width, bmp.Height);
			FastPixel fp = new FastPixel(bmp);
			fp.Lock();
			FastPixel gsfp = new FastPixel(grayscale);
			gsfp.Lock();

			int count = (group < 4 ? 256 : 64);
			int size = (group < 4 ? 16 : 8);
			int mapWidth = (group < 4 ? 160 : 240);
			int mapHeight = (group < 4 ? 128 : 176);
			byte[] formation = null;
			if (MapConstants.LargeMap(gb.GameType, group))
			{
				formation = m.GetDungeonFormation(cboGroup);
			}
			for (int i = 0; i < count; i++)
			{
				FastPixel src = GetTilesetFP(group, (formation != null ? formation[i] : i), (gb.GameType == GameTypes.Ages ? 0 : season));
				src.Lock();
				m.LoadMap((formation != null ? formation[i] : i), group, season);
				m.DrawMap(ref fp, (i % size) * mapWidth, (i / size) * mapHeight, src, null, (Grayscale ? gsfp : null));
				src.Unlock(false);
				if (form != null)
					form.setValue(i + 1, count);
			}

			fp.Unlock(true);
			gsfp.Unlock(true);
			CachedGroups[cboGroup] = bmp;
			if (Grayscale)
				CachedGrayscaleGroups[cboGroup] = grayscale;
			if (form != null)
				form.close();
			if (grayscaleRender)
				return grayscale;
			return bmp;
		}

		public FastPixel GetTilesetFP(int group, int index, int season = 0)
		{
			Area a = form.areaLoader.LoadArea(index, group, season);
			if (CachedTilesets[season, a.index] != null)
				return CachedTilesets[season, a.index];
			form.tilesetLoader.LoadTileset((byte)a.tileset);
			form.tilesetLoader.UnpackRawTiles();
			byte[] graphics = form.tilesetLoader.LoadGraphics(a.vram, a.unique, form.Decompressor);
			form.animationLoader.loadAnimatedTiles(a.animation, ref graphics);
			Bitmap bmp = form.tilesetLoader.drawTileset(form.tilesetLoader.UnpackGraphics(graphics), a.tileset >= 0x100, form.paletteLoader.LoadPalette(a.palette));
			CachedTilesets[season, a.index] = new FastPixel(bmp);
			return CachedTilesets[season, a.index];
		}

		public FastPixel GetTilesetFP(int index, int season)
		{
			try
			{
				Area a = form.areaLoader.LoadArea(index, season);
				if (CachedTilesets[season, a.index] != null)
					return CachedTilesets[season, a.index];
				form.tilesetLoader.LoadTileset((byte)a.tileset);
				form.tilesetLoader.UnpackRawTiles();
				byte[] graphics = form.tilesetLoader.LoadGraphics(a.vram, a.unique, form.Decompressor);
				form.animationLoader.loadAnimatedTiles(a.animation, ref graphics);
				Bitmap bmp = form.tilesetLoader.drawTileset(form.tilesetLoader.UnpackGraphics(graphics), a.tileset >= 0x100, form.paletteLoader.LoadPalette(a.palette));
				CachedTilesets[season, a.index] = new FastPixel(bmp);
				return CachedTilesets[season, a.index];
			}
			catch (Exception)
			{
				return null;
			}
		}

		public Bitmap GetTileset(int group, int index, int season = 0)
		{
			return GetTilesetFP(group, index, season).Bitmap;
		}

		public Bitmap GetTileset(int index, int season)
		{
			return GetTilesetFP(index, season).Bitmap;
		}

		public FastPixel GetTilesetFP(int baseTiles, int unique, int formation, int animation, int palette)
		{
			try
			{
				form.tilesetLoader.LoadTileset(formation);
				form.tilesetLoader.UnpackRawTiles();
				byte[] graphics = form.tilesetLoader.LoadGraphics(baseTiles, unique, form.Decompressor);
				form.animationLoader.loadAnimatedTiles(animation, ref graphics);
				Bitmap bmp = form.tilesetLoader.drawTileset(form.tilesetLoader.UnpackGraphics(graphics), formation >= 0x100, form.paletteLoader.LoadPalette(palette));
				return new FastPixel(bmp);
			}
			catch (Exception)
			{
				return null;
			}
		}

		public Bitmap GetTileset(int baseTiles, int unique, int formation, int animation, int palette)
		{
			try
			{
				form.tilesetLoader.LoadTileset(formation);
				form.tilesetLoader.UnpackRawTiles();
				byte[] graphics = form.tilesetLoader.LoadGraphics(baseTiles, unique, form.Decompressor);
				form.animationLoader.loadAnimatedTiles(animation, ref graphics);
				Bitmap bmp = form.tilesetLoader.drawTileset(form.tilesetLoader.UnpackGraphics(graphics), formation >= 0x100, form.paletteLoader.LoadPalette(palette));
				return bmp;
			}
			catch (Exception)
			{
				return null;
			}
		}

		public void UpdateMap(int cboGroup, int selectedIndex, MapLoader m)
		{
			int group = MapConstants.GetRealGroup(gb.GameType, cboGroup);
			int count = (group < 4 ? 256 : 64);
			int size = (group < 4 ? 16 : 8);
			int mapWidth = (group < 4 ? 160 : 240);
			int mapHeight = (group < 4 ? 128 : 176);
			byte[] formation = null;
			
			FastPixel src = GetTilesetFP(m.LoadedRoom.area.index, m.LoadedRoom.season);
			FastPixel colorDest = new FastPixel(CachedGroups[cboGroup]);
			FastPixel gsfp = null;
			if (Grayscale)
				gsfp = new FastPixel(CachedGrayscaleGroups[cboGroup]);

			if (MapConstants.LargeMap(gb.GameType, group))
			{
				formation = m.GetDungeonFormation(cboGroup);
			}

			src.Lock();
			colorDest.Lock();
			if (gsfp != null)
				gsfp.Lock();
			m.DrawMap(ref colorDest, (selectedIndex % size) * mapWidth, (selectedIndex / size) * mapHeight, src, null, gsfp);
			if (gsfp != null)
				gsfp.Unlock(true);
			colorDest.Unlock(true);
			src.Unlock(false);
		}

		/*public Bitmap DrawMaps(ref MapLoader loader, int baseMap, int baseGroup, int count)
		{
			Bitmap bmp;
			if (loader.room.type == MapLoader.RoomTypes.Small)
				bmp = new Bitmap(168 * count, 128 * count);
			else
				bmp = new Bitmap(240 * count, 176 * count);
			FastPixel fp = new FastPixel(bmp);
			fp.Lock();
		}*/
	}
}
