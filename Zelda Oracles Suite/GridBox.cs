using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;

using System.Text;
using System.Windows.Forms;

namespace Zelda_Oracles_Suite
{
	public partial class GridBox : InterpolationPicturebox
	{
		private Color hoverColor = Color.White;
		private Color selectedColor = Color.Red;
		private Size selectionSize = new Size(12, 10);
		private int selectedMap = 0;
		private bool canSelect = true;
		private bool hoverBox = true;
		private bool allowMultiSelection = false;
		private Rectangle selectionRectangle = new Rectangle(0, 0, 1, 1);
		private int hoverIndex = -1;
		private int lastHoverIndex = -1;
		private int startSelection = -1;
		private Size canvas = new Size(160, 128);
		private Pen selectionPen = new Pen(Color.Red, 2);
		private bool pannable = false;
		private Point lastPanPoint;

		public GridBox()
		{
			InitializeComponent();
			SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
		}

		[Description("The hover color."), Browsable(true)]
		public Color HoverColor
		{
			get { return hoverColor; }
			set { hoverColor = value; this.Invalidate(); }
		}

		[Description("The selection color."), Browsable(true)]
		public Color SelectionColor
		{
			get { return selectedColor; }
			set { selectedColor = value; selectionPen = new Pen(selectedColor, 2); this.Invalidate(); }
		}

		[Description("The box size."), Browsable(true)]
		public Size BoxSize
		{
			get { return selectionSize; }
			set { selectionSize = value; this.Invalidate(); }
		}

		[Description("The selected index."), Browsable(true)]
		public int SelectedIndex
		{
			get { return selectedMap; }
			set { selectedMap = value; selectionRectangle = new Rectangle((value % (canvas.Width / selectionSize.Width)), (value / (canvas.Height / selectionSize.Height)), 1, 1); this.Invalidate(); }
		}

		[Description("Determines whether or not items can be selected."), Browsable(true)]
		public bool Selectable
		{
			get { return canSelect; }
			set { canSelect = value; this.Invalidate(); }
		}

		[Description("Determines whether or not the hoverbox will be shown."), Browsable(true)]
		public bool HoverBox
		{
			get { return hoverBox; }
			set { hoverBox = value; this.Invalidate(); }
		}

		[Description("The canvas size."), Browsable(true)]
		public Size CanvasSize
		{
			get { if (SizeMode == PictureBoxSizeMode.AutoSize) return this.ClientSize; return canvas; }
			set { canvas = value; }
		}

		[Browsable(false)]
		public int HoverIndex
		{
			get { return hoverIndex; }
		}

		[Browsable(false)]
		public Rectangle SelectionRectangle
		{
			get { return selectionRectangle; }
			set { selectionRectangle = value; this.Invalidate(); }
		}

		[Description("Determines whether or not more than one items can be selected."), Browsable(true)]
		public bool AllowMultiSelection
		{
			get { return allowMultiSelection; }
			set { allowMultiSelection = value; }
		}

		[Description("Determines whether or not the control is pannable."), Browsable(true)]
		public bool Pannable
		{
			get { return pannable; }
			set { pannable = value; }
		}

		[Description("Determines whether or not selections are made by right-clicking."), Browsable(true)]
		public bool RightClickSelection { get; set; }

		private void GridBox_Paint(object sender, PaintEventArgs e)
		{
			if (canSelect)
			{
				if (!allowMultiSelection)
				{
					if (selectedMap != -1)
					{
						Point p = GetIndexPoint(selectedMap);
						e.Graphics.DrawRectangle(selectionPen, p.X, p.Y, selectionSize.Width, selectionSize.Height);
					}
				}
				else
				{
					Point p = GetIndexPoint(selectedMap);
					int x = p.X / selectionSize.Width;
					int y = p.Y / selectionSize.Height;
					int width = selectionRectangle.Width;
					int height = selectionRectangle.Height;
					if (width < 0)
					{
						int oldW = width;
						x += width;
						width *= -1;
						width++;
					}
					if (height < 0)
					{
						int oldH = height;
						y += height;
						height *= -1;
						height++;
					}
					e.Graphics.DrawRectangle(selectionPen, x * selectionSize.Width, y * selectionSize.Height, width * selectionSize.Width, height * selectionSize.Height);
				}
			}

			if (hoverBox)
			{
				if (hoverIndex != -1)
				{
					Point p = GetIndexPoint(hoverIndex);
					e.Graphics.DrawRectangle(new Pen(hoverColor), p.X, p.Y, selectionSize.Width - 1, selectionSize.Height - 1);
				}
			}
		}

		public Point GetIndexPoint(int i)
		{
			int width = (canvas.Width / selectionSize.Width);
			int height = (canvas.Height / selectionSize.Height);
			int x = i % width;
			int y = i / width;
			return new Point(x * selectionSize.Width, y * selectionSize.Height);
		}

		private void GridBox_MouseMove(object sender, MouseEventArgs e)
		{
			if (pannable)
			{
				if (e.Button == System.Windows.Forms.MouseButtons.Right)
				{
					if (lastPanPoint.X != e.X || lastPanPoint.Y != e.Y)
					{
						int changeX = (int)((double)(e.X - lastPanPoint.X) * 1.9f);
						int changeY = (int)((double)(e.Y - lastPanPoint.Y) * 1.9f);
						if (this.Left + changeX > 0)
							changeX = -this.Left;
						else if (this.Right + changeX < Parent.ClientSize.Width)
							changeX = Parent.ClientSize.Width - this.Right;
						this.Left += changeX;
						if (this.Top + changeY > 0)
							changeY = -this.Top;
						else if (this.Bottom + changeY < Parent.ClientSize.Height)
							changeY = Parent.ClientSize.Height - this.Bottom;
						this.Top += changeY;
						lastPanPoint = new Point(e.X - changeX, e.Y - changeY);
					}
				}
			}

			int x;
			int y;
			int width;
			int height;
			if (allowMultiSelection && startSelection != -1)
			{
				x = e.X / selectionSize.Width;
				y = e.Y / selectionSize.Height;
				width = x - selectionRectangle.X + 1;
				height = y - selectionRectangle.Y + 1;
				int w = selectionRectangle.Width + 1;
				int h = selectionRectangle.Height + 1;

				if (width != 0)
					selectionRectangle.Width = width - (width < 0 ? 1 : 0);
				else
					selectionRectangle.Width = -1;
				if (height != 0)
					selectionRectangle.Height = height - (height < 0 ? 1 : 0);
				else
					selectionRectangle.Height = -1;

				if (selectionRectangle.X + selectionRectangle.Width > canvas.Width / selectionSize.Width)
					selectionRectangle.Width = (canvas.Width / selectionSize.Width) - selectionRectangle.X;
				if (selectionRectangle.Y + selectionRectangle.Height > canvas.Height / selectionSize.Height)
					selectionRectangle.Height = (canvas.Height / selectionSize.Height) - selectionRectangle.Y;

				this.Invalidate();// (new Rectangle(selectionRectangle.X * selectionSize.Width - 1, selectionRectangle.Y * selectionSize.Height - 1, (w > selectionRectangle.Width ? w * selectionSize.Width : selectionRectangle.Width * selectionSize.Width) + 1, (h > selectionRectangle.Height ? h * selectionSize.Height : selectionRectangle.Height * selectionSize.Height) + 1));
				return;
			}

			if (e.X < 0 || e.Y < 0 || e.X >= canvas.Width || e.Y >= canvas.Height)
			{
				if (hoverIndex != -1)
				{
					hoverIndex = -1;
					lastHoverIndex = -1;
					this.Invalidate();
				}
				return;
			}

			width = (canvas.Width / selectionSize.Width);
			height = (canvas.Height / selectionSize.Height);
			x = e.X / selectionSize.Width;
			y = e.Y / selectionSize.Height;
			hoverIndex = x + y * width;

			if (hoverBox)
			{
				if (lastHoverIndex != hoverIndex)
				{
					if (lastHoverIndex >= 0)
					{
						Point a = GetIndexPoint(lastHoverIndex);
						this.Invalidate(new Rectangle(a.X, a.Y, BoxSize.Width, BoxSize.Height));
					}
					lastHoverIndex = hoverIndex;
					Point p = GetIndexPoint(hoverIndex);
					this.Invalidate(new Rectangle(p.X, p.Y, BoxSize.Width, BoxSize.Height));
				}
			}
		}

		private void GridBox_MouseLeave(object sender, EventArgs e)
		{
			if (hoverBox)
			{
				hoverIndex = -1;
				lastHoverIndex = -1;
				this.Invalidate();
			}
		}

		private void GridBox_MouseDown(object sender, MouseEventArgs e)
		{
			if (((e.Button == System.Windows.Forms.MouseButtons.Left && !RightClickSelection) || (e.Button == System.Windows.Forms.MouseButtons.Right && RightClickSelection)) && canSelect && hoverIndex != -1)
			{
				startSelection = hoverIndex;
				selectionRectangle = new Rectangle((e.X / selectionSize.Width), (e.Y / selectionSize.Height), 1, 1);
				selectedMap = hoverIndex;
				this.Invalidate();
			}
			if (pannable && e.Button == System.Windows.Forms.MouseButtons.Right)
			{
				lastPanPoint = new Point(e.X, e.Y);
			}
		}

		private void GridBox_MouseUp(object sender, MouseEventArgs e)
		{
			startSelection = -1;
		}

		public byte[,] GetSelectedIndexes()
		{
			Point p = GetIndexPoint(selectedMap);
			int x = p.X / selectionSize.Width;
			int y = p.Y / selectionSize.Height;
			int width = selectionRectangle.Width;
			int height = selectionRectangle.Height;
			int cw = canvas.Width / selectionSize.Width;
			if (width < 0)
			{
				int oldW = width;
				x += width;
				width *= -1;
				width++;
			}
			if (height < 0)
			{
				int oldH = height;
				y += height;
				height *= -1;
				height++;
			}
			byte[,] b = new byte[width, height];
			for (int yy = y; yy < y + height; yy++)
			{
				for (int xx = x; xx < x + width; xx++)
				{
					b[xx - x, yy - y] = (byte)(xx + yy * cw);
				}
			}
			return b;
		}
	}
}