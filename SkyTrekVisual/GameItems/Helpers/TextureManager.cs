using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using SkyTrekVisual.GameItems.BonusItems;

namespace SkyTrekVisual.GameItems.Helpers
{
    public static class TextureManager
    {
        public static BitmapSource[] Rocket_sprites;
        public static BitmapSource[] Rocket_explosion;

		public static BitmapSource[] Ship_previews;


		public static Dictionary<int, List<ImageBrush>> Explosions = new Dictionary<int, List<ImageBrush>>();

		public static Dictionary<BonusType, ImageBrush> Bonuses = new Dictionary<BonusType, ImageBrush>();




		public static void InitializeImages()
		{

		}



		public static void LoadTextures()
		{
			try
			{


				string[] rocket_sprites = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory + "Sprites/Rocket/Sprites/", "*.png");

				Rocket_sprites = new BitmapSource[rocket_sprites.Length];

				for(int i = 0; i < rocket_sprites.Length; i++)
					Rocket_sprites[i] = new BitmapImage(new Uri(rocket_sprites[i], UriKind.Absolute));


				string[] rocket_explosion = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory + "Sprites/Rocket/Explosion/", "*.png");

				Rocket_explosion = new BitmapSource[rocket_explosion.Length];

				for(int i = 0; i < rocket_explosion.Length; i++)
					Rocket_explosion[i] = new BitmapImage(new Uri(rocket_explosion[i], UriKind.Absolute));


				//Ship previews

				string[] ship_previews = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory + "Ships/", "*.png");

				Ship_previews = new BitmapSource[ship_previews.Length];

				for(int i = 0; i < ship_previews.Length; i++)
					Ship_previews[i] = new BitmapImage(new Uri(ship_previews[i], UriKind.Absolute));


				//Explosion previews



				var files = Directory.GetDirectories(AppDomain.CurrentDomain.BaseDirectory + "Explosions/");

				int j = 1;
				foreach(var dir in files.ToList())
				{
					var l = new List<ImageBrush>();

					foreach(var image in Directory.GetFiles(dir))
						l.Add(new ImageBrush(new BitmapImage(new Uri(image, UriKind.Absolute))));

					Explosions.Add(j++, l);
				}




				Bonuses.Add(BonusType.Shield, new ImageBrush(new BitmapImage(new Uri(
							AppDomain.CurrentDomain.BaseDirectory + @"\Bonuses\shield.png", UriKind.Relative)))
				{ Stretch = Stretch.UniformToFill });

				Bonuses.Add(BonusType.ExtraAmmo, new ImageBrush(new BitmapImage(new Uri(
							AppDomain.CurrentDomain.BaseDirectory + @"\Bonuses\ammo.png", UriKind.Relative)))
				{ Stretch = Stretch.UniformToFill });

				Bonuses.Add(BonusType.ScoreMultiplier, new ImageBrush(new BitmapImage(new Uri(
							AppDomain.CurrentDomain.BaseDirectory + @"\Bonuses\score.png", UriKind.Relative)))
				{ Stretch = Stretch.UniformToFill });

				Bonuses.Add(BonusType.Health, new ImageBrush(new BitmapImage(new Uri(
							AppDomain.CurrentDomain.BaseDirectory + @"\Bonuses\health.png", UriKind.Relative)))
				{ Stretch = Stretch.UniformToFill });


			}
			catch(Exception ex)
			{

				MessageBox.Show(ex.ToString());
			}
		}

	}
}

