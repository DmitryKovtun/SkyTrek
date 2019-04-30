using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace SkyTrekVisual.GameItems
{
	public static class CollisionDetector
	{
		public static int CanvasHeight { get; set; } = 0;


		public static bool IsCollision(IDestructibleItem bullet, IDestructibleItem enemy)
		{



			//if(y1 < 0)
			//	return true;
			//if(bullet.ItemHeight > CanvasHeight)
			//	return true;





			//Debug.WriteLine("\nbullet > ");
			//Debug.WriteLine("x   > " + bullet.CoordLeft.ToString());
			//Debug.WriteLine("y1  > " + bullet.CoordBottom.ToString());

			//Debug.WriteLine("enemy > ");
			//Debug.WriteLine("x   > " + enemy.CoordLeft.ToString());
			//Debug.WriteLine("y1  > " + enemy.CoordBottom.ToString());



			//return element.ItemWidth > item2.CoordX && element.CoordX < item2.ItemWidth && 
			//	element.ItemHeight > y2 && y1 < item2.ItemHeight;





			return bullet.CoordLeft >= enemy.CoordLeft && enemy.CoordBottom + enemy.ItemHeight >= bullet.CoordBottom
				&& bullet.CoordBottom >= enemy.CoordBottom;
		}





	}




}
