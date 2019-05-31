using System.Windows.Controls;
using SkyTrekVisual.GameItems.Rockets;
using static SkyTrekVisual.GameItems.Rockets.Rocket;

namespace SkyTrekVisual.GameItems
{

	public static class GunShot
	{

		public static Canvas DefaultRocketCanvas;


		public static void GenerateBullets(ISpaceShip player, double damage) => GenerateBullets(DefaultRocketCanvas, player, damage);


		public static void GenerateBullets(Canvas canvas, ISpaceShip player, double damage)
		{
			var leftCenter = player.CoordLeft + player.ItemWidth / 2;
			var bottomCenter = player.CoordBottom + player.ItemHeight / 2;

			switch(player.CurrentShipType)
			{
				case ShipType.Ship1:
					canvas.Children.Add(new Rocket(canvas, leftCenter + 28 * SpaceShip.ShipScale, bottomCenter + 19, damage));
					canvas.Children.Add(new Rocket(canvas, leftCenter + 28 * SpaceShip.ShipScale, bottomCenter - 21, damage));
					break;

				case ShipType.Ship2:
					canvas.Children.Add(new Rocket(canvas,leftCenter + 80 * SpaceShip.ShipScale, bottomCenter -7, damage));
					break;

				case ShipType.Ship3:
					canvas.Children.Add(new Rocket(canvas,leftCenter + 40 * SpaceShip.ShipScale, bottomCenter +40, damage));
					canvas.Children.Add(new Rocket(canvas,leftCenter + 52 * SpaceShip.ShipScale, bottomCenter, damage));
					canvas.Children.Add(new Rocket(canvas,leftCenter + 52 * SpaceShip.ShipScale, bottomCenter -1, damage));
					canvas.Children.Add(new Rocket(canvas,leftCenter + 40 * SpaceShip.ShipScale, bottomCenter - 41, damage));

					break;

				case ShipType.Ship4:
					canvas.Children.Add(new Rocket(canvas,leftCenter + 96 * SpaceShip.ShipScale, bottomCenter -20, damage));
					break;

				case ShipType.Ship5:
					canvas.Children.Add(new Rocket(canvas, leftCenter + 34 * SpaceShip.ShipScale, bottomCenter + 28, damage));
					canvas.Children.Add(new Rocket(canvas, leftCenter + 90 * SpaceShip.ShipScale, bottomCenter + 13, damage));
					canvas.Children.Add(new Rocket(canvas, leftCenter + 90 * SpaceShip.ShipScale, bottomCenter - 15, damage));
					canvas.Children.Add(new Rocket(canvas, leftCenter + 34 * SpaceShip.ShipScale, bottomCenter - 30, damage));
					break;

				case ShipType.Ship6:
					canvas.Children.Add(new Rocket(canvas,leftCenter + 28 * SpaceShip.ShipScale, bottomCenter - 11, damage));
					canvas.Children.Add(new Rocket(canvas,leftCenter + 28 * SpaceShip.ShipScale, bottomCenter + 11, damage));
					break;

				default:
					break;
			}
		}



		public static void GenerateBulletsRight(Enemy enemy, double damage) => GenerateBulletsRight(DefaultRocketCanvas, enemy, damage);

		public static void GenerateBulletsRight(Canvas canvas, Enemy enemy, double damage)
		{
			var leftCenter = enemy.CoordLeft + enemy.ItemWidth / 2;
			var bottomCenter = enemy.CoordBottom + enemy.ItemHeight / 2;

			canvas.Children.Add(new Rocket(canvas, leftCenter - 100, bottomCenter - 11, RocketDirection.Right) { SpriteAngle = 180, Speed = 5, Damage = 10 } );
		}













	}





}
