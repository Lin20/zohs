using System;
using System.Collections.Generic;
using System.Text;

namespace Zelda_Oracles_Suite
{
	public static class MapConstants
	{
		public const int AGES_LARGE_START = 4;
		public const int SEASONS_LARGE_START = 5;

		public static int GetRealGroup(GameTypes g, int selectedIndex)
		{
			if (g == GameTypes.Ages)
			{
				if (selectedIndex < 4)
					return selectedIndex;
				selectedIndex -= 4;
				if (selectedIndex >= 26)
					return 4 + (selectedIndex & 1);
				if (selectedIndex >= 14)
					return 5;
				else
					return 4;
			}
			else
			{
				if (selectedIndex < 4)
					return 0;
				if (selectedIndex == 4)
					return 1;
				if (selectedIndex < 19)
					return 4;
				if (selectedIndex < 27)
					return 5;
				if (selectedIndex == 27)
					return 4;
				return 5;
			}
		}

		public static bool LargeMap(GameTypes g, int group)
		{
			return group >= 4;
		}
	}
}
