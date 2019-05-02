using SkyTrekVisual.GameItems.Helpers;
using System.Diagnostics;
using System.Windows;

namespace SkyTrek
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            //SplashScreen splashScreen = new SplashScreen();
            //splashScreen.Show();

            //Stopwatch timer = new Stopwatch();
            //timer.Start();


            TextureManager.LoadTextures();

            //timer.Stop();

            //splashScreen.Close();

        }


    }
}



