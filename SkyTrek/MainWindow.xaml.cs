using System;
using System.Collections.Generic;
using System.IO;
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
using SkyTrekVisual.Controls;
using SkyTrekVisual.GameItems.StarShipList;

namespace SkyTrek
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : CustomWindow
    {

        MainWindowViewModel mwvm = new MainWindowViewModel();
        Engine GameEngine;


        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;


            DataContext = mwvm;

        }


        private void Menu_SelectedShipEvent(object sender, EventArgs e)
        {
            MessageBox.Show((sender as StarShip).ToString());
        }




        void MainWindow_Loaded(object s, RoutedEventArgs f)
        {
            // place for smelly code
            MouseDown += delegate (object sender, MouseButtonEventArgs e)
            { GameOver.Visibility = Visibility.Hidden; };
            KeyDown += delegate (object sender, KeyEventArgs e)
            { GameOver.Visibility = Visibility.Hidden; };
            KeyUp += delegate (object sender, KeyEventArgs e)
            { GameOver.Visibility = Visibility.Hidden; };
			// end place for smelly code




			// some initialization after we have actual window loaded
			GameEngine = new Engine(this);

            GameEngine.GameOverEvent += (object sender, EventArgs e) =>
            {
                GameOver.Visibility = Visibility.Visible;
                Go.Content = "GAME OVER!";
                LabelScore.Visibility = Visibility.Visible;
                LabelScore.Content = "Score: " + GameEngine.speed.Text;


			};

            // now for window
            GameOver.Visibility = Visibility.Visible;
            Go.Content = "NEW GAME";
            LabelScore.Visibility = Visibility.Collapsed;

            GameEngine.ResetAll();


            KeyDown += MainWindow_KeyDown;

			GameMenu.IsActive = layoutManager.IsMenu = true;
		}



		private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.S)
            {
                layoutManager.IsGameplay = true;
				GameMenu.IsActive = layoutManager.IsMenu = false;

                if (!GameEngine.IsActive())
                    GameEngine.Resume();
            }

            if (e.Key == Key.P && layoutManager.IsGameplay)
            {
				if(layoutManager.IsPause = !layoutManager.IsPause)
                    GameEngine.Pause();
                else
                    GameEngine.Resume();
            }

            if (e.Key == Key.Escape && layoutManager.IsGameplay && !layoutManager.IsPause)
            {
                layoutManager.IsGameplay = false;
				GameMenu.IsActive = layoutManager.IsMenu = true;

				GameEngine.Pause();
            }





        }

       
    }
}

