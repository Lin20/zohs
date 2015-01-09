namespace Zelda_Oracles_Suite.Tools
{
	partial class GeneralEditor
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
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.cboMusic = new Zelda_Oracles_Suite.DownloadableListBox();
			this.nAddress = new System.Windows.Forms.NumericUpDown();
			this.label8 = new System.Windows.Forms.Label();
			this.label12 = new System.Windows.Forms.Label();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.nPalette = new System.Windows.Forms.NumericUpDown();
			this.label5 = new System.Windows.Forms.Label();
			this.nAnimation = new System.Windows.Forms.NumericUpDown();
			this.label4 = new System.Windows.Forms.Label();
			this.nUnique = new System.Windows.Forms.NumericUpDown();
			this.label3 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.nTileset = new System.Windows.Forms.NumericUpDown();
			this.nVRAM = new System.Windows.Forms.NumericUpDown();
			this.label7 = new System.Windows.Forms.Label();
			this.groupBox3.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.nAddress)).BeginInit();
			this.groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.nPalette)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nAnimation)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nUnique)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nTileset)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nVRAM)).BeginInit();
			this.SuspendLayout();
			// 
			// groupBox3
			// 
			this.groupBox3.Controls.Add(this.cboMusic);
			this.groupBox3.Controls.Add(this.nAddress);
			this.groupBox3.Controls.Add(this.label8);
			this.groupBox3.Controls.Add(this.label12);
			this.groupBox3.Location = new System.Drawing.Point(3, 146);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(256, 68);
			this.groupBox3.TabIndex = 26;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "Map Information";
			// 
			// cboMusic
			// 
			this.cboMusic.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboMusic.FormattingEnabled = true;
			this.cboMusic.Items.AddRange(new object[] {
            "00",
            "01",
            "02",
            "03",
            "04",
            "05",
            "06",
            "07",
            "08",
            "09",
            "0A",
            "0B",
            "0C",
            "0D",
            "0E",
            "0F",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16",
            "17",
            "18",
            "19",
            "1A",
            "1B",
            "1C",
            "1D",
            "1E",
            "1F",
            "20",
            "21",
            "22",
            "23",
            "24",
            "25",
            "26",
            "27",
            "28",
            "29",
            "2A",
            "2B",
            "2C",
            "2D",
            "2E",
            "2F",
            "30",
            "31",
            "32",
            "33",
            "34",
            "35",
            "36",
            "37",
            "38",
            "39",
            "3A",
            "3B",
            "3C",
            "3D",
            "3E",
            "3F",
            "40",
            "41",
            "42",
            "43",
            "44",
            "45",
            "46",
            "47",
            "48",
            "49",
            "4A",
            "4B",
            "4C",
            "4D",
            "4E",
            "4F",
            "50",
            "51",
            "52",
            "53",
            "54",
            "55",
            "56",
            "57",
            "58",
            "59",
            "5A",
            "5B",
            "5C",
            "5D",
            "5E",
            "5F",
            "60",
            "61",
            "62",
            "63",
            "64",
            "65",
            "66",
            "67",
            "68",
            "69",
            "6A",
            "6B",
            "6C",
            "6D",
            "6E",
            "6F",
            "70",
            "71",
            "72",
            "73",
            "74",
            "75",
            "76",
            "77",
            "78",
            "79",
            "7A",
            "7B",
            "7C",
            "7D",
            "7E",
            "7F",
            "80",
            "81",
            "82",
            "83",
            "84",
            "85",
            "86",
            "87",
            "88",
            "89",
            "8A",
            "8B",
            "8C",
            "8D",
            "8E",
            "8F",
            "90",
            "91",
            "92",
            "93",
            "94",
            "95",
            "96",
            "97",
            "98",
            "99",
            "9A",
            "9B",
            "9C",
            "9D",
            "9E",
            "9F",
            "A0",
            "A1",
            "A2",
            "A3",
            "A4",
            "A5",
            "A6",
            "A7",
            "A8",
            "A9",
            "AA",
            "AB",
            "AC",
            "AD",
            "AE",
            "AF",
            "B0",
            "B1",
            "B2",
            "B3",
            "B4",
            "B5",
            "B6",
            "B7",
            "B8",
            "B9",
            "BA",
            "BB",
            "BC",
            "BD",
            "BE",
            "BF",
            "C0",
            "C1",
            "C2",
            "C3",
            "C4",
            "C5",
            "C6",
            "C7",
            "C8",
            "C9",
            "CA",
            "CB",
            "CC",
            "CD",
            "CE",
            "CF",
            "D0",
            "D1",
            "D2",
            "D3",
            "D4",
            "D5",
            "D6",
            "D7",
            "D8",
            "D9",
            "DA",
            "DB",
            "DC",
            "DD",
            "DE",
            "DF",
            "E0",
            "E1",
            "E2",
            "E3",
            "E4",
            "E5",
            "E6",
            "E7",
            "E8",
            "E9",
            "EA",
            "EB",
            "EC",
            "ED",
            "EE",
            "EF",
            "F0",
            "F1",
            "F2",
            "F3",
            "F4",
            "F5",
            "F6",
            "F7",
            "F8",
            "F9",
            "FA",
            "FB",
            "FC",
            "FD",
            "FE",
            "FF"});
			this.cboMusic.ListName = "sounds";
			this.cboMusic.Location = new System.Drawing.Point(50, 15);
			this.cboMusic.Name = "cboMusic";
			this.cboMusic.Size = new System.Drawing.Size(200, 21);
			this.cboMusic.TabIndex = 4;
			// 
			// nAddress
			// 
			this.nAddress.Hexadecimal = true;
			this.nAddress.Location = new System.Drawing.Point(60, 42);
			this.nAddress.Maximum = new decimal(new int[] {
            67108863,
            0,
            0,
            0});
			this.nAddress.Name = "nAddress";
			this.nAddress.Size = new System.Drawing.Size(68, 20);
			this.nAddress.TabIndex = 3;
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Location = new System.Drawing.Point(6, 44);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(48, 13);
			this.label8.TabIndex = 2;
			this.label8.Text = "Address:";
			// 
			// label12
			// 
			this.label12.AutoSize = true;
			this.label12.Location = new System.Drawing.Point(6, 21);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(38, 13);
			this.label12.TabIndex = 0;
			this.label12.Text = "Music:";
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.nPalette);
			this.groupBox1.Controls.Add(this.label5);
			this.groupBox1.Controls.Add(this.nAnimation);
			this.groupBox1.Controls.Add(this.label4);
			this.groupBox1.Controls.Add(this.nUnique);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.label6);
			this.groupBox1.Controls.Add(this.nTileset);
			this.groupBox1.Controls.Add(this.nVRAM);
			this.groupBox1.Controls.Add(this.label7);
			this.groupBox1.Location = new System.Drawing.Point(3, 3);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(115, 137);
			this.groupBox1.TabIndex = 25;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Area Information";
			// 
			// nPalette
			// 
			this.nPalette.Hexadecimal = true;
			this.nPalette.Location = new System.Drawing.Point(70, 111);
			this.nPalette.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
			this.nPalette.Name = "nPalette";
			this.nPalette.Size = new System.Drawing.Size(39, 20);
			this.nPalette.TabIndex = 23;
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(7, 113);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(43, 13);
			this.label5.TabIndex = 22;
			this.label5.Text = "Palette:";
			// 
			// nAnimation
			// 
			this.nAnimation.Hexadecimal = true;
			this.nAnimation.Location = new System.Drawing.Point(70, 88);
			this.nAnimation.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
			this.nAnimation.Name = "nAnimation";
			this.nAnimation.Size = new System.Drawing.Size(39, 20);
			this.nAnimation.TabIndex = 21;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(7, 90);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(56, 13);
			this.label4.TabIndex = 20;
			this.label4.Text = "Animation:";
			// 
			// nUnique
			// 
			this.nUnique.Hexadecimal = true;
			this.nUnique.Location = new System.Drawing.Point(70, 65);
			this.nUnique.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
			this.nUnique.Name = "nUnique";
			this.nUnique.Size = new System.Drawing.Size(39, 20);
			this.nUnique.TabIndex = 19;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(7, 67);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(44, 13);
			this.label3.TabIndex = 18;
			this.label3.Text = "Unique:";
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(7, 21);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(59, 13);
			this.label6.TabIndex = 14;
			this.label6.Text = "Base Tiles:";
			// 
			// nTileset
			// 
			this.nTileset.Hexadecimal = true;
			this.nTileset.Location = new System.Drawing.Point(70, 42);
			this.nTileset.Maximum = new decimal(new int[] {
            512,
            0,
            0,
            0});
			this.nTileset.Name = "nTileset";
			this.nTileset.Size = new System.Drawing.Size(39, 20);
			this.nTileset.TabIndex = 17;
			// 
			// nVRAM
			// 
			this.nVRAM.Hexadecimal = true;
			this.nVRAM.Location = new System.Drawing.Point(70, 19);
			this.nVRAM.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
			this.nVRAM.Name = "nVRAM";
			this.nVRAM.Size = new System.Drawing.Size(39, 20);
			this.nVRAM.TabIndex = 15;
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(7, 44);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(56, 13);
			this.label7.TabIndex = 16;
			this.label7.Text = "Formation:";
			// 
			// GeneralEditor
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.Transparent;
			this.Controls.Add(this.groupBox3);
			this.Controls.Add(this.groupBox1);
			this.Name = "GeneralEditor";
			this.Size = new System.Drawing.Size(262, 432);
			this.groupBox3.ResumeLayout(false);
			this.groupBox3.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.nAddress)).EndInit();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.nPalette)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nAnimation)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nUnique)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nTileset)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nVRAM)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox groupBox3;
		private DownloadableListBox cboMusic;
		private System.Windows.Forms.NumericUpDown nAddress;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.GroupBox groupBox1;
		public System.Windows.Forms.NumericUpDown nPalette;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.NumericUpDown nAnimation;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.NumericUpDown nUnique;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.NumericUpDown nTileset;
		private System.Windows.Forms.NumericUpDown nVRAM;
		private System.Windows.Forms.Label label7;
	}
}
