namespace Zelda_Oracles_Suite.Tools
{
	partial class MapEditor
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.chkClipboard = new System.Windows.Forms.CheckBox();
			this.pTileset = new Zelda_Oracles_Suite.GridBox();
			this.pClipboard = new Zelda_Oracles_Suite.GridBox();
			((System.ComponentModel.ISupportInitialize)(this.pTileset)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pClipboard)).BeginInit();
			this.SuspendLayout();
			// 
			// chkClipboard
			// 
			this.chkClipboard.AutoSize = true;
			this.chkClipboard.Location = new System.Drawing.Point(3, 266);
			this.chkClipboard.Name = "chkClipboard";
			this.chkClipboard.Size = new System.Drawing.Size(70, 17);
			this.chkClipboard.TabIndex = 31;
			this.chkClipboard.Text = "Clipboard";
			this.chkClipboard.UseVisualStyleBackColor = true;
			// 
			// pTileset
			// 
			this.pTileset.AllowMultiSelection = true;
			this.pTileset.BackColor = System.Drawing.Color.Black;
			this.pTileset.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.pTileset.BoxSize = new System.Drawing.Size(16, 16);
			this.pTileset.CanvasSize = new System.Drawing.Size(256, 256);
			this.pTileset.Cursor = System.Windows.Forms.Cursors.Default;
			this.pTileset.HoverBox = true;
			this.pTileset.HoverColor = System.Drawing.Color.White;
			this.pTileset.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Default;
			this.pTileset.Location = new System.Drawing.Point(0, 0);
			this.pTileset.Name = "pTileset";
			this.pTileset.Pannable = false;
			this.pTileset.RightClickSelection = false;
			this.pTileset.Selectable = true;
			this.pTileset.SelectedIndex = 0;
			this.pTileset.SelectionColor = System.Drawing.Color.Red;
			this.pTileset.SelectionRectangle = new System.Drawing.Rectangle(0, 0, 1, 1);
			this.pTileset.Size = new System.Drawing.Size(260, 260);
			this.pTileset.TabIndex = 29;
			this.pTileset.TabStop = false;
			// 
			// pClipboard
			// 
			this.pClipboard.AllowMultiSelection = true;
			this.pClipboard.BackColor = System.Drawing.Color.Black;
			this.pClipboard.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.pClipboard.BoxSize = new System.Drawing.Size(16, 16);
			this.pClipboard.CanvasSize = new System.Drawing.Size(256, 144);
			this.pClipboard.Cursor = System.Windows.Forms.Cursors.Default;
			this.pClipboard.HoverBox = true;
			this.pClipboard.HoverColor = System.Drawing.Color.White;
			this.pClipboard.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Default;
			this.pClipboard.Location = new System.Drawing.Point(0, 282);
			this.pClipboard.Name = "pClipboard";
			this.pClipboard.Pannable = false;
			this.pClipboard.RightClickSelection = true;
			this.pClipboard.Selectable = true;
			this.pClipboard.SelectedIndex = 0;
			this.pClipboard.SelectionColor = System.Drawing.Color.Red;
			this.pClipboard.SelectionRectangle = new System.Drawing.Rectangle(0, 0, 1, 1);
			this.pClipboard.Size = new System.Drawing.Size(262, 148);
			this.pClipboard.TabIndex = 30;
			this.pClipboard.TabStop = false;
			// 
			// MapEditor
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.Transparent;
			this.Controls.Add(this.pTileset);
			this.Controls.Add(this.pClipboard);
			this.Controls.Add(this.chkClipboard);
			this.Name = "MapEditor";
			this.Size = new System.Drawing.Size(262, 432);
			((System.ComponentModel.ISupportInitialize)(this.pTileset)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pClipboard)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private GridBox pTileset;
		private GridBox pClipboard;
		private System.Windows.Forms.CheckBox chkClipboard;

	}
}
