using System.Windows.Controls;
using static SkyTrekVisual.GameItems.Player;

namespace SkyTrekVisual.GameItems
{

	public static class PlayerShot
	{



		public static void GenerateBullets(Canvas canvas, Player player)
		{
			var leftCenter = player.CoordLeft + player.Width / 2;
			var bottomCenter = player.CoordBottom + player.Height / 2;

			switch(player.CurrentShipType)
			{
				case ShipType.Ship1:
					canvas.Children.Add(new Bullet(leftCenter + 28 * ShipScale, bottomCenter + 19));
					canvas.Children.Add(new Bullet(leftCenter + 28 * ShipScale, bottomCenter - 21));
					break;

				case ShipType.Ship2:
					canvas.Children.Add(new Bullet(leftCenter + 80 * ShipScale, bottomCenter -7));
					break;

				case ShipType.Ship3:
					canvas.Children.Add(new Bullet(leftCenter + 40 * ShipScale, bottomCenter +40));
					canvas.Children.Add(new Bullet(leftCenter + 52 * ShipScale, bottomCenter ));
					canvas.Children.Add(new Bullet(leftCenter + 52 * ShipScale, bottomCenter -1));
					canvas.Children.Add(new Bullet(leftCenter + 40 * ShipScale, bottomCenter - 41));

					break;

				case ShipType.Ship4:
					canvas.Children.Add(new Bullet(leftCenter + 96 * ShipScale, bottomCenter -20));
					break;

				case ShipType.Ship5:
					canvas.Children.Add(new Bullet(leftCenter + 34 * ShipScale, bottomCenter +28));
					canvas.Children.Add(new Bullet(leftCenter + 90 * ShipScale, bottomCenter + 13));
					canvas.Children.Add(new Bullet(leftCenter + 90 * ShipScale, bottomCenter - 15));
					canvas.Children.Add(new Bullet(leftCenter + 34 * ShipScale, bottomCenter -30));
					break;

				case ShipType.Ship6:
					canvas.Children.Add(new Bullet(leftCenter + 28 * ShipScale, bottomCenter - 11));
					canvas.Children.Add(new Bullet(leftCenter + 28 * ShipScale, bottomCenter + 11));
					break;

				default:
					break;
			}
		}



	}





}
