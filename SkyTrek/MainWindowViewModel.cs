using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using SkyTrekVisual.Controls.Commands;

namespace SkyTrek
{
	public class MainWindowViewModel
	{
		public static string CurrentDirectory { private set; get; }


		public MainWindowViewModel()
		{
			CurrentDirectory = Directory.GetCurrentDirectory();





		}



		public Engine GameEngine;


		public event EventHandler OnGameContinueEvent;



		private ICommand _ContinueGameCommand;

		public ICommand ContinueGameCommand
		{
			get
			{
				if(_ContinueGameCommand == null)
					_ContinueGameCommand = new DelegateCommand(delegate ()
					{
						OnGameContinueEvent.Invoke(null,null);

						if(!GameEngine.IsActive())
							GameEngine.Resume();

					});
				return _ContinueGameCommand;
			}
		}


		private ICommand _NewGameCommand;

		public ICommand NewGameCommand
		{
			get
			{
				if(_NewGameCommand == null)
					_NewGameCommand = new DelegateCommand(delegate ()
					{


						MessageBox.Show("_NewGameCommand");


					});
				return _NewGameCommand;
			}
		}


		private ICommand _SettingsCommand;

		public ICommand SettingsCommand
		{
			get
			{
				if(_SettingsCommand == null)
					_SettingsCommand = new DelegateCommand(delegate ()
					{


						MessageBox.Show("_SettingsCommand");


					});
				return _SettingsCommand;
			}
		}


		private ICommand _AboutCommand;

		public ICommand AboutCommand
		{
			get
			{
				if(_AboutCommand == null)
					_AboutCommand = new DelegateCommand(delegate ()
					{


						MessageBox.Show("_AboutCommand");


					});
				return _AboutCommand;
			}
		}


		private ICommand _ExitCommand;

		public ICommand ExitCommand
		{
			get
			{
				if(_ExitCommand == null)
					_ExitCommand = new DelegateCommand(delegate ()
					{


						Application.Current.Shutdown();


					});
				return _ExitCommand;
			}
		}








	}
}
