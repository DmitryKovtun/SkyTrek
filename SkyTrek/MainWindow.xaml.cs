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

                star.CoordLeft -= ((100 * .15 / 250 * (star as UserControl).ActualHeight)) % SpaceCanvasWidth;
            }
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

