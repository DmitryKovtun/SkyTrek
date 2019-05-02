using System;
using System.IO;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace SkyTrekVisual.GameItems.Helpers
{
    public static class TextureManager
    {
        public static BitmapSource[] Rocket_sprites;
        public static BitmapSource[] Rocket_explosion;

        public static BitmapSource[] Ship_previews;

        public static void LoadTextures()
        {
            
            string[] rocket_sprites = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory + "Sprites/Rocket/Sprites/", "*.png");

            Rocket_sprites = new BitmapSource[rocket_sprites.Length];

            for (int i = 0; i < rocket_sprites.Length; i++)
                Rocket_sprites[i] = new BitmapImage(new Uri(rocket_sprites[i], UriKind.Absolute));


            string[] rocket_explosion = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory + "Sprites/Rocket/Explosion/", "*.png");

            Rocket_explosion = new BitmapSource[rocket_explosion.Length];

            for (int i = 0; i < rocket_explosion.Length; i++)
                Rocket_explosion[i] = new BitmapImage(new Uri(rocket_explosion[i], UriKind.Absolute));


            //Ship previews

            string[] ship_previews = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory + "Ships/", "*.png");

            Ship_previews = new BitmapSource[ship_previews.Length];

            for (int i = 0; i < ship_previews.Length; i++)
                Ship_previews[i] = new BitmapImage(new Uri(ship_previews[i], UriKind.Absolute));

        }

    }
}

