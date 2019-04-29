using System.Windows.Controls;
using static SkyTrekVisual.GameItems.Player;

namespace SkyTrekVisual.GameItems
{

	public static class PlayerShot
	{
		public static int Height { get; set; }


		public static void GenerateBullets(ShipType type, Canvas canvas, Player player)
		{
			var size = player.PlayerSize / 2;
			switch(type)
			{
				case ShipType.Ship1:
					var t = new Bullet(player);

					t.SetValue(Canvas.LeftProperty, t.CoordX - size + 8.0);
					t.SetValue(Canvas.TopProperty, Height - t.CoordY - 15.0);

					canvas.Children.Add(t);

					t = new Bullet(player);

					t.SetValue(Canvas.LeftProperty, t.CoordX - size + 8.0);
					t.SetValue(Canvas.TopProperty, Height - t.CoordY + 13.0);

					canvas.Children.Add(t);
					break;

				case ShipType.Ship2:
					t = new Bullet(player);

					t.SetValue(Canvas.LeftProperty, t.CoordX - size - 4.0);
					t.SetValue(Canvas.TopProperty, Height - t.CoordY + 3.0);

					canvas.Children.Add(t);
					break;

				case ShipType.Ship3:
					t = new Bullet(player);

					t.SetValue(Canvas.LeftProperty, t.CoordX - 20.0);
					t.SetValue(Canvas.TopProperty, Height - t.CoordY - 1.0);

					canvas.Children.Add(t);
					break;

				case ShipType.Ship4:
					t = new Bullet(player);

					t.SetValue(Canvas.LeftProperty, t.CoordX - size + 30.0);
					t.SetValue(Canvas.TopProperty, Height - t.CoordY + 11.0);

					canvas.Children.Add(t);
					break;

				case ShipType.Ship5:
					t = new Bullet(player);

					t.SetValue(Canvas.LeftProperty, t.CoordX - size + 30.0);
					t.SetValue(Canvas.TopProperty, Height - t.CoordY - 11.0);

					canvas.Children.Add(t);

					t = new Bullet(player);

					t.SetValue(Canvas.LeftProperty, t.CoordX - size + 30.0);
					t.SetValue(Canvas.TopProperty, Height - t.CoordY + 10.0);

					canvas.Children.Add(t);

					t = new Bullet(player);

					t.SetValue(Canvas.LeftProperty, t.CoordX - size + 14.0);
					t.SetValue(Canvas.TopProperty, Height - t.CoordY - 21.0);

					canvas.Children.Add(t);

					t = new Bullet(player);

					t.SetValue(Canvas.LeftProperty, t.CoordX - size + 14.0);
					t.SetValue(Canvas.TopProperty, Height - t.CoordY + 20.0);

					canvas.Children.Add(t);
					break;

				case ShipType.Ship6:
					t = new Bullet(player);

					t.SetValue(Canvas.LeftProperty, t.CoordX - size + 30 + .1);
					t.SetValue(Canvas.TopProperty, Height - t.CoordY - 11 + .1);

					canvas.Children.Add(t);

					t = new Bullet(player);

					t.SetValue(Canvas.LeftProperty, t.CoordX - size + 30 + .1);
					t.SetValue(Canvas.TopProperty, Height - t.CoordY + 11 + .1);

					canvas.Children.Add(t);
					break;

				default:
					break;
			}
		}



	}





}
