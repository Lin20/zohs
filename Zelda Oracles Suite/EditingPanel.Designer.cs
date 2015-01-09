namespace Zelda_Oracles_Suite
{
	partial class EditingPanel
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditingPanel));
			this.groupBox4 = new System.Windows.Forms.GroupBox();
			this.pPainting = new Zelda_Oracles_Suite.GridBox();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.rbPaint = new System.Windows.Forms.RadioButton();
			this.rbLine = new System.Windows.Forms.RadioButton();
			this.rbPencil = new System.Windows.Forms.RadioButton();
			this.groupBox4.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pPainting)).BeginInit();
			this.groupBox2.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox4
			// 
			this.groupBox4.Controls.Add(this.pPainting);
			this.groupBox4.Location = new System.Drawing.Point(96, 3);
			this.groupBox4.Name = "groupBox4";
			this.groupBox4.Size = new System.Drawing.Size(122, 91);
			this.groupBox4.TabIndex = 31;
			this.groupBox4.TabStop = false;
			this.groupBox4.Text = "Painting";
			// 
			// pPainting
			// 
			this.pPainting.AllowMultiSelection = false;
			this.pPainting.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pPainting.BackgroundImage")));
			this.pPainting.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.pPainting.BoxSize = new System.Drawing.Size(16, 16);
			this.pPainting.CanvasSize = new System.Drawing.Size(128, 64);
			this.pPainting.Cursor = System.Windows.Forms.Cursors.Default;
			this.pPainting.HoverBox = true;
			this.pPainting.HoverColor = System.Drawing.Color.White;
			this.pPainting.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Default;
			this.pPainting.Location = new System.Drawing.Point(6, 19);
			this.pPainting.Name = "pPainting";
			this.pPainting.Pannable = false;
			this.pPainting.RightClickSelection = true;
			this.pPainting.Selectable = false;
			this.pPainting.SelectedIndex = 0;
			this.pPainting.SelectionColor = System.Drawing.Color.Red;
			this.pPainting.SelectionRectangle = new System.Drawing.Rectangle(0, 0, 1, 1);
			this.pPainting.Size = new System.Drawing.Size(52, 52);
			this.pPainting.TabIndex = 26;
			this.pPainting.TabStop = false;
			this.pPainting.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pPainting_MouseDown);
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.rbPaint);
			this.groupBox2.Controls.Add(this.rbLine);
			this.groupBox2.Controls.Add(this.rbPencil);
			this.groupBox2.Location = new System.Drawing.Point(3, 3);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(87, 91);
			this.groupBox2.TabIndex = 30;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Editing Mode";
			// 
			// rbPaint
			// 
			this.rbPaint.AutoSize = true;
			this.rbPaint.Location = new System.Drawing.Point(6, 65);
			this.rbPaint.Name = "rbPaint";
			this.rbPaint.Size = new System.Drawing.Size(75, 17);
			this.rbPaint.TabIndex = 2;
			this.rbPaint.Text = "Paintbrush";
			this.rbPaint.UseVisualStyleBackColor = true;
			this.rbPaint.CheckedChanged += new System.EventHandler(this.rbPaint_CheckedChanged);
			// 
			// rbLine
			// 
			this.rbLine.AutoSize = true;
			this.rbLine.Location = new System.Drawing.Point(6, 42);
			this.rbLine.Name = "rbLine";
			this.rbLine.Size = new System.Drawing.Size(45, 17);
			this.rbLine.TabIndex = 1;
			this.rbLine.Text = "Line";
			this.rbLine.UseVisualStyleBackColor = true;
			this.rbLine.CheckedChanged += new System.EventHandler(this.rbLine_CheckedChanged);
			// 
			// rbPencil
			// 
			this.rbPencil.AutoSize = true;
			this.rbPencil.Checked = true;
			this.rbPencil.Location = new System.Drawing.Point(6, 19);
			this.rbPencil.Name = "rbPencil";
			this.rbPencil.Size = new System.Drawing.Size(54, 17);
			this.rbPencil.TabIndex = 0;
			this.rbPencil.TabStop = true;
			this.rbPencil.Text = "Pencil";
			this.rbPencil.UseVisualStyleBackColor = true;
			this.rbPencil.CheckedChanged += new System.EventHandler(this.rbPencil_CheckedChanged);
			// 
			// EditingPanel
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.groupBox4);
			this.Controls.Add(this.groupBox2);
			this.Name = "EditingPanel";
			this.Size = new System.Drawing.Size(230, 101);
			this.groupBox4.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.pPainting)).EndInit();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox groupBox4;
		private GridBox pPainting;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.RadioButton rbPaint;
		private System.Windows.Forms.RadioButton rbLine;
		private System.Windows.Forms.RadioButton rbPencil;

	}
}
