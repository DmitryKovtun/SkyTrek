using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using SkyTrek.Pages;
using SkyTrekVisual;
using SkyTrekVisual.Controls.Commands;
using SkyTrekVisual.GameItems;
using SkyTrekVisual.GameItems.StarShipList;

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

        public bool isPlaying = false;

        public MainWindowViewModel()
        {
            CurrentDirectory = Directory.GetCurrentDirectory();

            page_Menu = new Page_Menu();
            page_Creators = new Page_Creators();
            page_Settings = new Page_Settings();
            page_ShipSelecting = new Page_ShipSelecting();
            page_GameplayLayout = new Page_GameplayLayout();
            page_Score = new Page_Score();

            page_Menu.Event_NewGame += Page_Menu_Event_NewGame;
            page_Menu.Event_ContinueGame += Page_Menu_Event_ContinueGame;
            page_Menu.Event_Creators += Page_Menu_Event_Creators;
            page_Menu.Event_Settings += Page_Menu_Event_Settings;
            page_Menu.Event_Score += Page_Menu_Event_Score;
            page_Menu.Event_Exit += Page_Menu_Event_Exit;

            page_ShipSelecting.Event_StartNewGame += Page_ShipSelecting_Event_StartNewGame;

            page_Creators.Event_BackToMenu += Page_Event_BackToMenu;
            page_Settings.Event_BackToMenu += Page_Event_BackToMenu;
            page_ShipSelecting.Event_BackToMenu += Page_Event_BackToMenu;
            page_Score.Event_BackToMenu += Page_Event_BackToMenu;

            CurrentPage = page_Menu;

            CurrentPlayer = new Player();
            CurrentPlayer.OnPlayerHealthChange += CurrentPlayer_OnPlayerHealthChange;

            page_GameplayLayout.GameplayPanel.GameBar.DataContext = CurrentPlayer.Score;

            GameEngine = new Engine(CurrentPlayer);
            GameEngine.GameOverEvent += GameEngine_GameOverEvent;

            GameEngine.InitCanvases(page_GameplayLayout.GameplayPanel);

            GameEngine.ResetAll();
        }

        private void GameEngine_GameOverEvent(object sender, EventArgs e)
        {
            isPlaying = false;
            page_GameplayLayout.layoutManager.IsGameOver = true;
            page_GameplayLayout.GameOverScore.Content = CurrentPlayer.Score.ScoreString;
        }

        private void CurrentPlayer_OnPlayerHealthChange(object sender, EventArgs e)
        {
            var t = sender as Player;

            page_GameplayLayout.GameplayPanel.GameBar.SetPlayerHealthIndicator(t.HealthPoints);
        }

        private void Page_Menu_Event_ContinueGame(object sender, EventArgs e)
        {
            //НУЖНО ПРЕДУСМОТРЕТЬ ФЛАГ, который будет on|off доступность кнопки

            //page_Menu.Set_IsEnabled_Button_Continue(true);

        }

        private void Page_ShipSelecting_Event_StartNewGame(object sender, EventArgs e)
        {
            //ВЫБРАННЫЙ КОРАБЛЬ
            StarShip starShip = sender as StarShip;
            Debug.WriteLine(starShip);

            isPlaying = true;

            CurrentPage = page_GameplayLayout;
            GameEngine.StartGame();

            //if (!GameEngine.IsActive())
            //    GameEngine.Resume();

        }

        private void Page_Menu_Event_NewGame(object sender, EventArgs e)
        {
            CurrentPage = page_ShipSelecting;
        }

        private void Page_Menu_Event_Score(object sender, EventArgs e)
        {
            CurrentPage = page_Score;
        }

        private void Page_Event_BackToMenu(object sender, EventArgs e)
        {
            CurrentPage = page_Menu;
        }

        private void Page_Menu_Event_Exit(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Page_Menu_Event_Settings(object sender, EventArgs e)
        {
            CurrentPage = page_Settings;
        }

        private void Page_Menu_Event_Creators(object sender, EventArgs e)
        {
            CurrentPage = page_Creators;
        }

        public void KeyDown(Key keyDown)
        {
            GameEngine.KeyDown(keyDown);

            if (keyDown == Key.P && isPlaying)
            {
                if (page_GameplayLayout.SetPause())
                    GameEngine.Pause();
                else
                    GameEngine.Resume();
            }

            if (keyDown == Key.Escape && page_GameplayLayout.layoutManager.IsPause)
            {
                GameEngine.Pause();
                isPlaying = false;
                page_GameplayLayout.layoutManager.IsPause = false;
                CurrentPage = page_Menu;
            }

            if(page_GameplayLayout.layoutManager.IsGameOver && !isPlaying)
            {
                CurrentPage = page_Menu;
                page_GameplayLayout.layoutManager.IsGameOver = false;
            }
        }

        public void KeyUp(Key keyUp)
        {
            GameEngine.KeyUp(keyUp);
        }
    }
}
