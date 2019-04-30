using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyTrekVisual.GameItems
{
	public static class CollisionDetector
	{
		public static int CanvasHeight { get; set; } = 0;


		public static bool IsCollision(IDestructibleItem element, IDestructibleItem item2)
		{
			if(element.CoordY < 0)
				return true;
			if(element.ItemHeight > CanvasHeight)
				return true;

			return element.ItemWidth > item2.CoordX && element.CoordX < item2.ItemWidth && 
				element.ItemHeight > item2.CoordY && element.CoordY < item2.ItemHeight;
		}


	}




}
