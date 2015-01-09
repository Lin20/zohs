using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace Zelda_Oracles_Suite
{
	public interface MapEditingComponent
	{
		string Title { get; set; }
        Image Icon { get; set; }
		int Group { get; set; }
		int Index { get; set; }
		byte Season { get; set; }
		void MapChanged(ROM r, int group, int index, int season, Room room);

		void MapMouseDraw(Graphics g, GridBox pMap);
		void MapMouseDown(MouseEventArgs e, GridBox pMap);
		void MapMouseMove(MouseEventArgs e, GridBox pMap);
		void MapMouseUp(MouseEventArgs e, GridBox pMap);
		void TilesetChanged();
		void UpdateMap();
	}
}
