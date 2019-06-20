using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Xml.Serialization;
using System.Linq;
using SkyTrek.Pages;
using SkyTrekVisual;
using SkyTrekVisual.GameItems;
using SkyTrekVisual.GameItems.ScoreItemList;

namespace SkyTrek
{
    public class MainWindowViewModel : NotifyPropertyChanged
    {
        public static string CurrentDirectory { private set; get; }

        Page_Menu page_Menu;
        Page_Creators page_Creators;
        Page_Settings page_Settings;
        Page_ShipSelecting page_ShipSelecting;
        Page_GameplayLayout page_GameplayLayout;
        Page_Score page_Score;





        private object currentPage;

        public object CurrentPage
        {
            get { return currentPage; }
            set { currentPage = value; OnPropertyChanged("CurrentPage"); }
        }

        public Player CurrentPlayer;
        public Engine GameEngine;




		public ObservableCollection<ScoreItem> HighScoreList { get; set; } = new ObservableCollection<ScoreItem>();



		private bool isGameActive;

		public bool IsGameActive
		{
			get { return isGameActive; }
			set { Event_BackgroundTimerChangeStatus.Invoke(isGameActive = value, null);}
		}



		public event EventHandler Event_BackgroundTimerChangeStatus;






		/// <summary>
		/// 
		/// </summary>
		public MainWindowViewModel()
        {
			


			CurrentDirectory = Directory.GetCurrentDirectory();

			SetPages();



			// finalizing with file reading 

			IsContinueEnabled(false);

			LoadFiles();


			page_Score.DataContext = this;
		}

		private void SetPages()
		{
			page_Menu = new Page_Menu();
			page_Creators = new Page_Creators();
			page_Settings = new Page_Settings();
			page_ShipSelecting = new Page_ShipSelecting();
			page_GameplayLayout = new Page_GameplayLayout();
			page_Score = new Page_Score();


			page_Menu.Event_NewGame += (object sender, EventArgs e) => CurrentPage = page_ShipSelecting;
			page_Menu.Event_ContinueGame += Page_Menu_Event_ContinueGame;
			page_Menu.Event_Creators += (object sender, EventArgs e) => CurrentPage = page_Creators;
			page_Menu.Event_Settings += (object sender, EventArgs e) => CurrentPage = page_Settings;
			page_Menu.Event_Score += (object sender, EventArgs e) => CurrentPage = page_Score;

			page_Menu.Event_Exit += (object sender, EventArgs e) => Application.Current.Shutdown();

			page_ShipSelecting.Event_StartNewGame += Page_ShipSelecting_Event_StartNewGame;

			page_Creators.Event_BackToMenu += Page_Event_BackToMenu;
			page_Settings.Event_BackToMenu += Page_Event_BackToMenu;
			page_ShipSelecting.Event_BackToMenu += Page_Event_BackToMenu;
			page_Score.Event_BackToMenu += Page_Event_BackToMenu;

			CurrentPage = page_Menu;
		}





		#region File handling


		/// <summary>
		/// 
		/// </summary>
		public void LoadFiles()
		{
			LoadScoreList();
			LoadLastGame();

		}

		/// <summary>
		/// 
		/// </summary>
		public void SaveFiles()
		{
			SaveScoreList();
			SaveLastGame();

		}


		/// <summary>
		/// 
		/// </summary>
		private void LoadScoreList()
		{
			if(File.Exists("scores.xml"))

				using(StreamReader sr = new StreamReader(new FileStream("scores.xml", FileMode.Open)))
					if(!sr.EndOfStream)
					{
						HighScoreList = new XmlSerializer(typeof(ObservableCollection<ScoreItem>)).Deserialize(sr) as ObservableCollection<ScoreItem>;
					}

		}


		/// <summary>
		/// 
		/// </summary>
		private void SaveScoreList()
		{
			using(StreamWriter sw = new StreamWriter(new FileStream("scores.xml", FileMode.Create)))
				new XmlSerializer(typeof(ObservableCollection<ScoreItem>)).Serialize(sw, HighScoreList);
		}


		/// <summary>
		/// 
		/// </summary>
		private void LoadLastGame()
		{
			return;

			if(File.Exists("savegame.xml"))
			{
				using(StreamReader sr = new StreamReader(new FileStream("savegame.xml", FileMode.Open)))
				{
					if(!sr.EndOfStream)
					{
						IsContinueEnabled(true);

						GameEngine = new XmlSerializer(typeof(Engine)).Deserialize(sr) as Engine;
						CurrentPlayer = GameEngine.CurrentPlayer;
					}
				}
			}
		}


		/// <summary>
		/// 
		/// </summary>
		private void SaveLastGame()
		{
			return;
			using(StreamWriter sw = new StreamWriter(new FileStream("savegame.xml", FileMode.Create)))
				new XmlSerializer(typeof(Engine)).Serialize(sw, this);

		}


		#endregion




		public void StartEngine()
		{	
			CurrentPlayer.Ship.OnHealthChange += CurrentPlayer_OnPlayerHealthChange;

			page_GameplayLayout.GameplayPanel.GameBar.DataContext = CurrentPlayer.Score;

			GameEngine = new Engine(CurrentPlayer);
			GameEngine.GameOverEvent += GameEngine_GameOverEvent;

			GameEngine.InitCanvases(page_GameplayLayout.GameplayPanel);
			GameEngine.ResetAll();

		}





		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void GameEngine_GameOverEvent(object sender, EventArgs e)
		{
			RemoveKeyEvents();

			IsGameActive = false;
            page_GameplayLayout.IsGameOver = true;
            page_GameplayLayout.GameOverScore.Content = CurrentPlayer.Score.ScoreString;

			HighScoreList.Add(new ScoreItem(CurrentPlayer.UserName, CurrentPlayer.Score.Score, DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString()));

			// order by desc
			var t = HighScoreList.OrderByDescending(i => i.Score);

			page_Score.DataContext = null;

			int b = 0;
			HighScoreList = new ObservableCollection<ScoreItem>();
			foreach(var item in t)
			{
				HighScoreList.Add(item);

				if(++b > 5)
					break;
			}

			page_Score.DataContext = this;

			CanPressKeys = false;
		}



		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void CurrentPlayer_OnPlayerHealthChange(object sender, EventArgs e)
        {
            var t = sender as SpaceShip;

            page_GameplayLayout.GameplayPanel.GameBar.SetPlayerHealthIndicator(t.HealthPoints);
        }



		#region PAGINATION

		private void Page_Event_BackToMenu(object sender, EventArgs e)
		{
			CurrentPage = page_Menu;
		}


		private void Page_Menu_Event_ContinueGame(object sender, EventArgs e)
        {

			CurrentPage = page_GameplayLayout;

			ResumeAll();
		}

		public void IsContinueEnabled(bool f)
		{

			page_Menu.Set_IsEnabled_Button_Continue(f);
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender">index of selected ship</param>
		/// <param name="e"></param>
        private void Page_ShipSelecting_Event_StartNewGame(object sender, EventArgs e)
        {
			

			if(sender != null)
			{
				CurrentPlayer = new Player();
				CurrentPlayer.Ship = new SpaceShip((int)sender, 0.1, 50);
				CurrentPlayer.UserName = page_ShipSelecting.UserName;
			}
			else
			{
				GC.Collect(200, GCCollectionMode.Forced, true);
			}







			StartEngine();

            CurrentPage = page_GameplayLayout;
            GameEngine.StartNewGame();

			ResumeAll();


			AddKeyEvents();


			CanPressKeys = true;
		}


		#endregion











		void PauseAll()
		{
			page_GameplayLayout.IsPause = true;

			IsGameActive = false;
			GameEngine.Pause();

		}


		void ResumeAll()
		{
			page_GameplayLayout.IsPause = false;
			page_GameplayLayout.IsGameOver = false;

			IsGameActive = true;
			GameEngine.Resume();

		}






		#region Key events


		public void AddKeyEvents()
		{


		}
		public void RemoveKeyEvents()
		{


		}

		bool CanPressKeys = false;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="keyDown"></param>
		public void KeyDown(Key keyDown)
        {
			if(CanPressKeys)
			{
				GameEngine.KeyDown(keyDown);

				if(keyDown == Key.P)
				{
					if(IsGameActive)
						PauseAll();
					else
						ResumeAll();
				}

				if(keyDown == Key.Escape)
				{
					PauseAll();
					page_GameplayLayout.IsPause = false;

					CurrentPage = page_Menu;
					Event_BackgroundTimerChangeStatus.Invoke(true, null);
				}
			}
			else if(!IsGameActive)
			{
				if(keyDown == Key.Escape)
					CurrentPage = page_Menu;
				else if(page_GameplayLayout.IsGameOver && CurrentPage == page_GameplayLayout)
					Page_ShipSelecting_Event_StartNewGame(null, null);
			}
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="keyUp"></param>
		public void KeyUp(Key keyUp)
        {
			if(CanPressKeys)
				GameEngine.KeyUp(keyUp);
        }

		#endregion


	}
}
