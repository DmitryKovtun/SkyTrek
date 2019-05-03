using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using SkyTrekVisual.GameItems;
using SkyTrekVisual.GameItems.StarShipList;

namespace SkyTrek
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>

	public partial class MainWindow : CustomWindow
	{

		MainWindowViewModel mwvm = new MainWindowViewModel();



		public MainWindow()
		{
			InitializeComponent();
			Loaded += MainWindow_Loaded;


			DataContext = mwvm;

			GameMenu.DataContext = mwvm;

			mwvm.GameEngine.InitCanvases(this);


			mwvm.GameEngine.ResetAll();


		}





		void MainWindow_Loaded(object s, RoutedEventArgs f)
		{
			// place for smelly code
			MouseDown += delegate (object sender, MouseButtonEventArgs e)
			{
				Gameplay.GameOver.Visibility = Visibility.Hidden;

			
			};
			KeyDown += delegate (object sender, KeyEventArgs e)
			{
				Gameplay.GameOver.Visibility = Visibility.Hidden;
			};
			KeyUp += delegate (object sender, KeyEventArgs e)
			{
				Gameplay.GameOver.Visibility = Visibility.Hidden;
			};
			// end place for smelly code

			mwvm.GameEngine.GameOverEvent += (object sender, EventArgs e) =>
			{
				Gameplay.GameOver.Visibility = Visibility.Visible;
				Gameplay.Go.Content = "GAME OVER!";
				Gameplay.LabelScore.Visibility = Visibility.Visible;
				Gameplay.LabelScore.Content = "Score: " + mwvm.GameEngine.speed.Text;

			
			};

			//// now for window
			//Gameplay.GameOver.Visibility = Visibility.Visible;
			//Gameplay.Go.Content = "NEW GAME";
			//Gameplay.LabelScore.Visibility = Visibility.Collapsed;



			// some initialization after we have actual window loaded
			KeyDown += MainWindow_KeyDown;

			GameMenu.IsActive = layoutManager.IsMenu = true;

			mwvm.OnGameContinueEvent += Mwvm_OnGameContinueEvent;

			mwvm.CurrentPlayer.OnPlayerHealthChange += CurrentPlayer_OnPlayerHealthChange;


			Gameplay.GameBar.DataContext = mwvm.CurrentPlayer.Score;

		}






		private void CurrentPlayer_OnPlayerHealthChange(object sender, EventArgs e)
		{
			var t = sender as Player;

			var f = t.HealthPoints * 252 / 100;
			Gameplay.GameBar.PlayerHealthIndicator.Width = f > 0 ? f : 0;

			if(f > 230)
				Gameplay.GameBar.PlayerHealthIndicator.Background = new BrushConverter().ConvertFromString("#8BC34A") as SolidColorBrush;
			if(f > 126 & f < 230)
				Gameplay.GameBar.PlayerHealthIndicator.Background = new BrushConverter().ConvertFromString("#F9AA33") as SolidColorBrush;
			else if(f <= 126)
				Gameplay.GameBar.PlayerHealthIndicator.Background = new BrushConverter().ConvertFromString("#df4e56") as SolidColorBrush;

		}




		private void Mwvm_OnGameContinueEvent(object sender, EventArgs e)
		{
			layoutManager.IsGameplay = true;
			GameMenu.IsActive = layoutManager.IsMenu = false;

		}




		private void MainWindow_KeyDown(object sender, KeyEventArgs e)
		{
			if(e.Key == Key.P && layoutManager.IsGameplay)
			{
				if(layoutManager.IsPause = !layoutManager.IsPause)
					mwvm.GameEngine.Pause();
				else
					mwvm.GameEngine.Resume();
			}

			if(e.Key == Key.Escape && layoutManager.IsGameplay && !layoutManager.IsPause)
			{
				layoutManager.IsGameplay = false;
				GameMenu.IsActive = layoutManager.IsMenu = true;

				mwvm.GameEngine.Pause();
			}

		}


		private void Menu_SelectedShipEvent(object sender, EventArgs e)
		{
			MessageBox.Show((sender as StarShip).ToString());
		}

	}
}

