using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace Zelda_Oracles_Suite
{
	public class InteractionEditor : UserControl, MapEditingComponent
	{
		private ComboBox cboInteractions;
		private HexBox hexBox1;

		public const int NoLocation = 0x49977;

		public ROM ROM { get; set; }
		public int Group { get; set; }
		public int Index { get; set; }
		public byte Season { get; set; }
		public Room LoadedRoom { get; set; }
		public string Title { get; set; }
		public Image Icon { get; set; }

		#region EditingComponents
		public void MapChanged(ROM r, int group, int index, int season, Room room)
		{
			this.ROM = r;
			this.Group = group;
			this.Index = index;
			this.Season = (byte)season;
			this.LoadedRoom = room;
		}

		public void MapMouseDraw(Graphics g, GridBox pMap)
		{
		}

		public void MapMouseDown(MouseEventArgs e, GridBox pMap)
		{
		}

		public void MapMouseMove(MouseEventArgs e, GridBox pMap)
		{
		}

		public void MapMouseUp(MouseEventArgs e, GridBox pMap)
		{
		}

		public void TilesetChanged()
		{
		}

		public void UpdateMap()
		{
		}
		#endregion

		public InteractionEditor()
		{
			InitializeComponent();
		}

		private void InitializeComponent()
		{
			this.cboInteractions = new System.Windows.Forms.ComboBox();
			this.hexBox1 = new Zelda_Oracles_Suite.HexBox();
			this.SuspendLayout();
			// 
			// cboInteractions
			// 
			this.cboInteractions.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboInteractions.FormattingEnabled = true;
			this.cboInteractions.Location = new System.Drawing.Point(3, 3);
			this.cboInteractions.Name = "cboInteractions";
			this.cboInteractions.Size = new System.Drawing.Size(257, 21);
			this.cboInteractions.TabIndex = 0;
			// 
			// hexBox1
			// 
			this.hexBox1.BackColor = System.Drawing.SystemColors.Window;
			this.hexBox1.Bytes = new byte[] {
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0))};
			this.hexBox1.Font = new System.Drawing.Font("Courier New", 12F);
			this.hexBox1.Location = new System.Drawing.Point(33, 108);
			this.hexBox1.Name = "hexBox1";
			this.hexBox1.SelectionIndex = 0;
			this.hexBox1.Size = new System.Drawing.Size(204, 20);
			this.hexBox1.TabIndex = 1;
			// 
			// InteractionEditor
			// 
			this.Controls.Add(this.hexBox1);
			this.Controls.Add(this.cboInteractions);
			this.Name = "InteractionEditor";
			this.Size = new System.Drawing.Size(263, 312);
			this.Load += new System.EventHandler(this.InteractionEditor_Load);
			this.ResumeLayout(false);

		}

		private void InteractionEditor_Load(object sender, EventArgs e)
		{

		}
	}
}
