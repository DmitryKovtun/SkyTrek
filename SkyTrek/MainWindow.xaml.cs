using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using SkyTrekVisual.Controls;
using SkyTrekVisual.GameItems;

namespace SkyTrek
{
	




	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>


	public partial class MainWindow : CustomWindow
	{


		#region Background items 

		private static int StarCount = 300;
		private static int PlanetCount = 7;
		private static int AsteriodCount = 17;

		/// <summary>
		/// actually dunno what is this
		/// </summary>
		private int straight_counter = 100;

		/// <summary>
		/// Defines how much background items will change their position every tick
		/// </summary>
		double BackgroundSpeedModifier = .15;     // def 1.5

		#endregion




		Random r = new Random();

        private double SpaceCanvasWidth;
        private double SpaceCanvasHeight;

        private DispatcherTimer SpaceCanvasTimer;

        MainWindowViewModel mwvm;

        public MainWindow()
		{
			InitializeComponent();

            mwvm = new MainWindowViewModel();
            DataContext = mwvm;


			mwvm.Event_BackgroundTimerChangeStatus += delegate (object sender, EventArgs e)
			{
				if(((bool)sender) == true)
					SpaceCanvasTimer.Start();
				else
					SpaceCanvasTimer.Stop();
			};



		}





		void MainWindow_Loaded(object s, RoutedEventArgs f)
		{
            SpaceCanvasTimer = new DispatcherTimer() { Interval = TimeSpan.FromSeconds(.01) };
            SpaceCanvasTimer.Tick += SpaceCanvasTimerUpdater;

            SpaceCanvasHeight = SpaceCanvas.Height;
            SpaceCanvasWidth = (SpaceCanvas.Width + 16);

            for (int i = 0; i < 300; i++)
                SpaceCanvas.Children.Add(new Star(r.Next() % (SpaceCanvasWidth + 16) - 16, r.Next() % SpaceCanvasHeight));

            SpaceCanvasTimer.Start();
		}





        private void SpaceCanvasTimerUpdater(object sender, EventArgs e)
        {
            foreach (IGameItem star in SpaceCanvas.Children)
            {
                if (star.CoordLeft < -16)
                {
                    star.CoordLeft += SpaceCanvasWidth + 32;
                    star.CoordBottom = r.Next() % Height;

                    star.GenerateType();
                    star.GenerateSize();
                }

                star.CoordLeft -= ((100 * .5 / 250 * (star as UserControl).ActualHeight)) % SpaceCanvasWidth;
            }
        }






		private void CustomWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			mwvm.SaveFiles();
		}





		private void MainWindow_KeyDown(object sender, KeyEventArgs e)
		{
            mwvm.KeyDown(e.Key);
        }

        private void MainWindow_KeyUp(object sender, KeyEventArgs e)
        {
            mwvm.KeyUp(e.Key);
        }

	
	}
}

