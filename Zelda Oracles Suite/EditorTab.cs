using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Zelda_Oracles_Suite
{
	public class EditorTab : TabPage
	{
		public MapEditingComponent EditingComponent { get; set; }

		public EditorTab(MapEditingComponent e, string s)
			: base(s)
		{
			EditingComponent = e;

			ControlCollection controls = ((UserControl)e).Controls;
			for(int i = 0; controls.Count != 0; i++)
			{
				this.Controls.Add(controls[0]);
			}
		}
	}
}
