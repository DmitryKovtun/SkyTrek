using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SkyTrek.Screens
{
    /// <summary>
    /// Interaction logic for Menu.xaml
    /// </summary>
    public partial class Menu : UserControl
    {
        public Menu()
        {
            InitializeComponent();

            //ScreensaverTimer = new DispatcherTimer() { Interval = TimeSpan.FromSeconds(DefaultGameplaySpeed) };
            //ScreensaverTimer.Tick += ScreensaverUpdater;

            //for (int i = 0; i < StarCount; i++)
            //    ScreensaverCanvas.Children.Add(new Star(r.Next() % (Width + MaxObjectSize) - MaxObjectSize, r.Next() % Height));



        }

      


        //ScreensaverTimer = new DispatcherTimer() { Interval = TimeSpan.FromSeconds(DefaultGameplaySpeed) };
        //ScreensaverTimer.Tick += ScreensaverUpdater;


        //public void ScreensaverUpdater(object sender, EventArgs e)
        //{
        //    foreach (IGameItem gameplayItem in ScreensaverCanvas.Children)
        //    {
        //        if (gameplayItem.CoordLeft < -MaxObjectSize + 1)
        //        {
        //            gameplayItem.CoordLeft += Width;
        //            gameplayItem.CoordBottom = r.Next() % Height;

        //            gameplayItem.GenerateType();
        //            gameplayItem.GenerateSize();
        //        }

        //        var l = (gameplayItem as UserControl).ActualHeight;
        //        //gameplayItem.CoordLeft -= (straight_counter * BackgroundSpeedModifier / (gameplayItem as UserControl).ActualHeight) % Width;	// dist



        //        gameplayItem.CoordLeft -= ((straight_counter * BackgroundSpeedModifier / 250 * l)) % Width;
        //    }

        //}


    }
}
