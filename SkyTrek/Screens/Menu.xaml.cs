using SkyTrekVisual;
using SkyTrekVisual.GameItems;
using SkyTrekVisual.GameItems.Helpers;
using SkyTrekVisual.GameItems.StarShipList;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Threading;

namespace SkyTrek.Screens
{
    /// <summary>
    /// Interaction logic for Menu.xaml
    /// </summary>
    public partial class Menu : UserControl
    {
        public bool IsActive
        {
            get { return (bool)GetValue(IsActiveProperty); }
            set { SetValue(IsActiveProperty, value); }
        }

        public static readonly DependencyProperty IsActiveProperty =
            DependencyProperty.Register("IsActive", typeof(bool), typeof(Menu), new PropertyMetadata(false, OnIsActiveChanged));

        private static void OnIsActiveChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue)
                (d as Menu).ScreensaverTimer.Start();
            else
                (d as Menu).ScreensaverTimer.Stop();
        }






        DispatcherTimer ScreensaverTimer;


        private ObservableCollection<StarShip> starShips;

        public ObservableCollection<StarShip> StarShips { get { return starShips; } }

        private StarShip selectedShip;

        public StarShip SelectedShip
        {
            get { return selectedShip; }
            set { selectedShip = value; SelectedShipEvent.Invoke(value, null); }
        }

        public event EventHandler SelectedShipEvent;

        Random r = new Random();

        public Menu()
        {
            InitializeComponent();

            starShips = new ObservableCollection<StarShip>();

            if(TextureManager.Ship_previews.Length==0)
            {
                MessageBox.Show("Ship preview did not found!");
            }

            foreach (var item in TextureManager.Ship_previews)
            {
                starShips.Add(new StarShip(item));
            }

            DataContext = this;



            ScreensaverTimer = new DispatcherTimer() { Interval = TimeSpan.FromSeconds(.01) };
            ScreensaverTimer.Tick += ScreensaverUpdater;

           

            for (int i = 0; i < 300; i++)
                ScreensaverCanvas.Children.Add(new Star(r.Next() % (ScreensaverCanvas.ActualWidth + 64) - 64, r.Next() % ScreensaverCanvas.ActualHeight));


            ScreensaverTimer.Start();

        }

        private void ScreensaverUpdater(object sender, EventArgs e)
        {
            foreach (IDestructibleItem gameplayItem in ScreensaverCanvas.Children)
            {
                if (gameplayItem.CoordLeft < -64 + 1)
                {
                    gameplayItem.CoordLeft += Width;
                    gameplayItem.CoordBottom = r.Next() % Height;

                    gameplayItem.GenerateType();
                    gameplayItem.GenerateSize();
                }

                gameplayItem.CoordLeft -= ((100 * .15 / 250 * gameplayItem.ItemWidth)) % Width;
            }
        }



        private bool myVar;

        public bool MyProperty
        {
            get { return myVar; }
            set { myVar = value; }
        }





    }
}
