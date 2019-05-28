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



            //// place for smelly code
            //MouseDown += delegate (object sender, MouseButtonEventArgs e)
            //{
            //	Gameplay.GameOver.Visibility = Visibility.Hidden;


            //};
            //KeyDown += delegate (object sender, KeyEventArgs e)
            //{
            //	Gameplay.GameOver.Visibility = Visibility.Hidden;
            //};
            //KeyUp += delegate (object sender, KeyEventArgs e)
            //{
            //	Gameplay.GameOver.Visibility = Visibility.Hidden;
            //};
            //// end place for smelly code

            //mwvm.GameEngine.GameOverEvent += (object sender, EventArgs e) =>
            //{
            //	Gameplay.GameOver.Visibility = Visibility.Visible;
            //	Gameplay.Go.Content = "GAME OVER!";
            //	Gameplay.LabelScore.Visibility = Visibility.Visible;
            //	Gameplay.LabelScore.Content = "Score: " + mwvm.GameEngine.speed.Text;


            //};

            //// now for window
            //Gameplay.GameOver.Visibility = Visibility.Visible;
            //Gameplay.Go.Content = "NEW GAME";
            //Gameplay.LabelScore.Visibility = Visibility.Collapsed;





			//GameMenu.IsActive = layoutManager.IsMenu = true;

			//mwvm.OnGameContinueEvent += Mwvm_OnGameContinueEvent;

			mwvm.CurrentPlayer.OnPlayerHealthChange += CurrentPlayer_OnPlayerHealthChange;


			//Gameplay.GameBar.DataContext = mwvm.CurrentPlayer.Score;

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



        private void CurrentPlayer_OnPlayerHealthChange(object sender, EventArgs e)
		{
			var t = sender as Player;

			//var f = t.HealthPoints * 252 / 100;
			//Gameplay.GameBar.PlayerHealthIndicator.Width = f > 0 ? f : 0;

			//if(f > 230)
			//	Gameplay.GameBar.PlayerHealthIndicator.Background = new BrushConverter().ConvertFromString("#8BC34A") as SolidColorBrush;
			//if(f > 126 & f < 230)
			//	Gameplay.GameBar.PlayerHealthIndicator.Background = new BrushConverter().ConvertFromString("#F9AA33") as SolidColorBrush;
			//else if(f <= 126)
			//	Gameplay.GameBar.PlayerHealthIndicator.Background = new BrushConverter().ConvertFromString("#df4e56") as SolidColorBrush;

		}


		private void MainWindow_KeyDown(object sender, KeyEventArgs e)
		{
            mwvm.KeyDown(e.Key);
        }

        private void CustomWindow_KeyUp(object sender, KeyEventArgs e)
        {

        }
    }
}

