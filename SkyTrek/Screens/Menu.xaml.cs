using SkyTrekVisual;
using SkyTrekVisual.Controls;
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
			set { SetValue(IsActiveProperty, value); MessageBox.Show("Test");}
		}

		public static readonly DependencyProperty IsActiveProperty =
			DependencyProperty.Register("IsActive", typeof(bool), typeof(Menu), new PropertyMetadata(false, OnIsActiveChanged));

		private static void OnIsActiveChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if((bool)e.NewValue)
				(d as Menu).ScreensaverTimer.Start();
			else
				(d as Menu).ScreensaverTimer.Stop();
		}






		DispatcherTimer ScreensaverTimer;

		public ObservableCollection<StarShip> StarShips { get; }

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

            StarShips = new ObservableCollection<StarShip>();

            if(TextureManager.Ship_previews.Length==0)
            {
                MessageBox.Show("Ship preview did not found!");
            }

            foreach (var item in TextureManager.Ship_previews)
            {
                StarShips.Add(new StarShip(item));
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
            foreach (IGameItem star in ScreensaverCanvas.Children)
            {
                if (star.CoordLeft < -64 + 1)
                {
                    star.CoordLeft += Width;
                    star.CoordBottom = r.Next() % Height;

                    star.GenerateType();
                    star.GenerateSize();
                }

                star.CoordLeft -= ((100 * .15 / 250 * (star as UserControl).ActualHeight)) % Width;
            }
        }







    }
}
