namespace Zelda_Oracles_Suite
{
	partial class frmExport
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

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.lblStatus = new System.Windows.Forms.Label();
			this.pBar = new System.Windows.Forms.ProgressBar();
			this.SuspendLayout();
			// 
			// lblStatus
			// 
			this.lblStatus.Location = new System.Drawing.Point(29, 69);
			this.lblStatus.Name = "lblStatus";
			this.lblStatus.Size = new System.Drawing.Size(188, 23);
			this.lblStatus.TabIndex = 3;
			this.lblStatus.Text = "Exporting Map";
			this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// pBar
			// 
			this.pBar.Location = new System.Drawing.Point(29, 22);
			this.pBar.MarqueeAnimationSpeed = 1;
			this.pBar.Name = "pBar";
			this.pBar.Size = new System.Drawing.Size(188, 23);
			this.pBar.Step = 1;
			this.pBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
			this.pBar.TabIndex = 2;
			// 
			// frmExport
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(246, 115);
			this.Controls.Add(this.lblStatus);
			this.Controls.Add(this.pBar);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.MaximizeBox = false;
			this.Name = "frmExport";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Rendering Group";
			this.Load += new System.EventHandler(this.frmExportMaps_Load);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Label lblStatus;
		public System.Windows.Forms.ProgressBar pBar;
	}
}