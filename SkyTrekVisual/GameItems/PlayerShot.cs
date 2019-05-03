using System.Windows.Controls;
using static SkyTrekVisual.GameItems.Player;
using SkyTrekVisual.GameItems.Rockets;
using System;
using static SkyTrekVisual.GameItems.Rockets.Rocket;

namespace SkyTrekVisual.GameItems
{

	public static class PlayerShot
	{

		public static Canvas DefaultRocketCanvas;


		public static void GenerateBullets(ISpaceShip player) => GenerateBullets(DefaultRocketCanvas, player);


		public static void GenerateBullets(Canvas canvas, ISpaceShip player)
		{
			var leftCenter = player.CoordLeft + player.ItemWidth / 2;
			var bottomCenter = player.CoordBottom + player.ItemHeight / 2;

			switch(player.CurrentShipType)
			{
				case ShipType.Ship1:
					//canvas.Children.Add(new Rocket(canvas,leftCenter + 28 * ShipScale, bottomCenter + 19));
					canvas.Children.Add(new Rocket(canvas, leftCenter + 28 * ShipScale, bottomCenter + 19));
					canvas.Children.Add(new Rocket(canvas, leftCenter + 28 * ShipScale, bottomCenter - 21));
					break;

				case ShipType.Ship2:
					canvas.Children.Add(new Rocket(canvas,leftCenter + 80 * ShipScale, bottomCenter -7));
					break;

				case ShipType.Ship3:
					canvas.Children.Add(new Rocket(canvas,leftCenter + 40 * ShipScale, bottomCenter +40));
					canvas.Children.Add(new Rocket(canvas,leftCenter + 52 * ShipScale, bottomCenter ));
					canvas.Children.Add(new Rocket(canvas,leftCenter + 52 * ShipScale, bottomCenter -1));
					canvas.Children.Add(new Rocket(canvas,leftCenter + 40 * ShipScale, bottomCenter - 41));

					break;

				case ShipType.Ship4:
					canvas.Children.Add(new Rocket(canvas,leftCenter + 96 * ShipScale, bottomCenter -20));
					break;

				case ShipType.Ship5:
					canvas.Children.Add(new Rocket(canvas, leftCenter + 34 * ShipScale, bottomCenter + 28));
					canvas.Children.Add(new Rocket(canvas, leftCenter + 90 * ShipScale, bottomCenter + 13));
					canvas.Children.Add(new Rocket(canvas, leftCenter + 90 * ShipScale, bottomCenter - 15));
					canvas.Children.Add(new Rocket(canvas, leftCenter + 34 * ShipScale, bottomCenter - 30));
					break;

				case ShipType.Ship6:
					canvas.Children.Add(new Rocket(canvas,leftCenter + 28 * ShipScale, bottomCenter - 11));
					canvas.Children.Add(new Rocket(canvas,leftCenter + 28 * ShipScale, bottomCenter + 11));
					break;

				default:
					break;
			}
		}



		public static void GenerateBulletsRight(Enemy enemy) => GenerateBulletsRight(DefaultRocketCanvas, enemy);

		public static void GenerateBulletsRight(Canvas canvas, Enemy enemy)
		{
			var leftCenter = enemy.CoordLeft + enemy.ItemWidth / 2;
			var bottomCenter = enemy.CoordBottom + enemy.ItemHeight / 2;

			canvas.Children.Add(new Rocket(canvas, leftCenter - 100, bottomCenter - 11, RocketDirection.Right) { SpriteAngle = 180, Speed = 5, Damage = 10 } );
		}













	}





}
