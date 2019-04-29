using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using SkyTrekVisual.GameItems;

namespace SkyTrek
{
	/// <summary>
	/// How it works nobody knows
	/// </summary>
	public class Engine
	{
		#region TODO - place textblocks somewhere outside of engine


		public TextBlock topScoretext;
		public TextBlock score;



		#endregion


		private int straight_counter = 0;

		private double topScore = 0;

		private bool spacedown = false;

		private readonly int mouseupthreshold = 30;

		private double Partitions = 3.0;


		#region Background items 

		private List<IGameItem> BackgroundItems = new List<IGameItem>();

		private static int StarCount = 300;
		private static int PlanetCount = 7;
		private static int AsteriodCount = 17;


		#endregion



		



		#region Obstacles - LEGACY (flappy mode)

		private readonly double ob_GapEnd = 60;
		private readonly double ob_Width = 50.0;
		private readonly double ob_GapBase = 200.0;
		private readonly double ob_Speed = 4.0;

		#endregion





		#region Player properties	TODO - make a class for player and his ship

	













		#endregion

		/// <summary>
		/// Gravity of flapping. less == longer fly			-- TODO is it emportant now??
		/// </summary>
		private double Gravitation = 0.3;







		public Canvas WindowCanvas { get; set; }








		private DispatcherTimer GameplayTimer;


		private int Counter = 0;

		/// <summary>
		/// Last click counter
		/// </summary>
		private int LastMouseCounter = 0;

		/// <summary>
		/// Random for all generation things
		/// </summary>
		private readonly Random r = new Random();

		/// <summary>
		/// List of obstacles			-- TODO fix?
		/// </summary>
		private List<Obstacle> ObstactleList = new List<Obstacle>();

		/// <summary>
		/// Defines whether to show startup screen
		/// </summary>
		private bool isNewGame = true;

		/// <summary>
		/// Height of updatable screen area
		/// </summary>
		private int Height;

		/// <summary>
		/// Width of updatable screen area
		/// </summary>
		private int Width;


		/// <summary>
		/// Defines whether to use obstacle generation and updating
		/// </summary>
		private bool isObstacleEnabled = false;


		/// <summary>
		/// Defines maximum background object size.
		/// Updatable screen area will be expanded by this value
		/// </summary>
		private int MaxObjectSize = 64;


		/// <summary>
		/// Defines how much background items will change their position every tick
		/// </summary>
		double StarSpeedModifier = 3.0;     // def 1.5




		/// <summary>
		/// Flappy bird mode
		/// </summary>
		private bool isFlapping = false;

		/// <summary>
		/// Defines if there is flicker of player on startup
		/// </summary>
		private bool isStartupFlicker = false;		




		/// <summary>
		/// Player sprite
		/// </summary>
		ImageBrush IB;

		/// <summary>
		/// Just a player
		/// </summary>
		public Player CurrentPlayer;

		/// <summary>
		/// Is raised when player loses a game
		/// </summary>
		public event EventHandler GameOverEvent;




		/// <summary>
		/// Ctor
		/// </summary>
		/// <param name="window"></param>
		public Engine(MainWindow window)
		{
			WindowCanvas = window.WindowCanvas;

			Height = (int)(WindowCanvas.ActualHeight + MaxObjectSize);
			Width = (int)(WindowCanvas.ActualWidth + MaxObjectSize);


			window.KeyUp += Window_KeyUp;
			window.KeyDown += Window_KeyDown;
			window.MouseDown += Window_MouseDown;

		}





		/// <summary>
		/// Init all variables of engine
		/// </summary>
		public void Initialize()
		{
			for(int i = 0; i < StarCount; i++)
				BackgroundItems.Add(new Star(r.Next() % (Width + MaxObjectSize) - MaxObjectSize, r.Next() % Height));

			for(int i = 0; i < PlanetCount; i++)
				BackgroundItems.Add(new Planet(r.Next() % (Width + MaxObjectSize) - MaxObjectSize, r.Next() % Height));

			for(int i = 0; i < AsteriodCount; i++)
				BackgroundItems.Add(new Asteriod(r.Next() % (Width + MaxObjectSize) - MaxObjectSize, r.Next() % Height));

			GameplayTimer = new DispatcherTimer() { Interval = TimeSpan.FromSeconds(DefaultGameplaySpeed) };
			GameplayTimer.Tick += BackgroundUpdater;

			


			CurrentPlayer = new Player();
		}


	
		/// <summary>
		/// Updates players` position on canvas
		/// </summary>
		private void UpdatePlayerPosition()
		{
			CurrentPlayer.SetValue(Canvas.TopProperty, Height - CurrentPlayer.Player_LiftPosition);
			CurrentPlayer.SetValue(Canvas.LeftProperty, CurrentPlayer.Player_ForwardPosition);
		}



		/// <summary>
		/// Resets game
		/// </summary>
		public void ResetAll()
		{
			Counter = 0;

			ObstactleList.Clear();
			for(int i = 0; i < Partitions; i++)
				ObstactleList.Add(new Obstacle() { Height = r.NextDouble(), Left = 500 + (Width + ob_Width) * (i / Partitions), Neg = (r.Next() % 2) * 2 - 1 });

			CurrentPlayer.Player_LiftPosition = 200.0;
			CurrentPlayer.Player_Speed = 0.0;
		}

		/// <summary>
		/// Collision detection method
		/// </summary>
		/// <param name="r1"></param>
		/// <param name="r2"></param>
		/// <returns></returns>
		bool IsCollision(Rectangle r1, Rectangle r2)
		{
			double r1L = (double)r1.GetValue(Canvas.LeftProperty);
			double r1T = (double)r1.GetValue(Canvas.TopProperty);
			double r1R = r1L + r1.Width;
			double r1B = r1T + r1.Height;

			double r2L = (double)r2.GetValue(Canvas.LeftProperty);
			double r2T = (double)r2.GetValue(Canvas.TopProperty);
			double r2R = r2L + r2.Width;
			double r2B = r2T + r2.Height;

			if(r1T < 0)
				return true;
			if(r1B > WindowCanvas.ActualHeight)
				return true;

			return r1R > r2L && r1L < r2R && r1B > r2T && r1T < r2B;
		}














		#region EXPERIMENTAL part - do not touch the RED button


		/// <summary>
		/// EXPERIMENTAL screen updater
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public void BackgroundUpdater(object sender, EventArgs e)
		{
			#region Counters

			straight_counter++;
			Counter++;

			topScore = Counter > topScore ? Counter : topScore;

			#endregion

			WindowCanvas.Children.Clear();

			#region Background updating

			//-32 ------ Width + 32
			//Width + 32 -------- Width + 40

			foreach(var gameplayItem in BackgroundItems)
			{
				if(gameplayItem.CoordX - straight_counter * StarSpeedModifier < -MaxObjectSize)
				{
					gameplayItem.CoordX += Width;
					gameplayItem.CoordY = r.Next() % Height;

					gameplayItem.GenerateType();
					gameplayItem.GenerateSize();
				}

				(gameplayItem as UIElement).SetValue(Canvas.TopProperty, (double)gameplayItem.CoordY);
				(gameplayItem as UIElement).SetValue(Canvas.LeftProperty, (gameplayItem.CoordX - straight_counter * StarSpeedModifier) % (Width));
				WindowCanvas.Children.Add(gameplayItem as UIElement);
			}

			#endregion

			#region SPEED

			// TODO view model

			var speed = new TextBlock();
			speed.Background = new SolidColorBrush(Colors.Transparent);
			speed.Margin = new Thickness(5, 35, 0, 0);
			speed.FontSize = 20.0;
			speed.Foreground = new SolidColorBrush(Colors.White);
			speed.Text = "SPEED: " + ((int)CurrentPlayer.Player_CurrentSpeed).ToString() + "  ";

			WindowCanvas.Children.Add(speed);

			#endregion

			CurrentPlayer.GenerateType();

			UpdatePlayerPosition();

			#region Startup flicker

			if(isStartupFlicker)
			{
				if(Counter > 30 || (Counter < 30 && Counter % 5 < 3))
					WindowCanvas.Children.Add(CurrentPlayer);
			}
			else
				WindowCanvas.Children.Add(CurrentPlayer);

			#endregion

			#region Obstacle updating

			if(isObstacleEnabled)
				foreach(Obstacle obstacle in ObstactleList)
				{
					double ob_gap = ob_GapBase * obstacle.Left / Width + ob_GapEnd;
					double top_height = (Height - ob_gap) * Math.Pow(Math.Sin((obstacle.Height + obstacle.Neg * 2 * obstacle.Left / Width)), 2.0);

					Color color = obstacle.IsHit ? Colors.Red : Colors.SlateGray;

					Rectangle top = new Rectangle()
					{
						Width = ob_Width,
						Height = top_height,
						Stroke = new SolidColorBrush(Colors.White),
						StrokeThickness = 0,
						Fill = new SolidColorBrush(color)
					};
					top.SetValue(Canvas.TopProperty, 0.0);
					top.SetValue(Canvas.LeftProperty, obstacle.Left);

					Rectangle bottom = new Rectangle()
					{
						Width = ob_Width,
						Height = Height - top_height - ob_gap,
						Stroke = new SolidColorBrush(Colors.White),
						StrokeThickness = 0,
						Fill = new SolidColorBrush(color)
					};
					bottom.SetValue(Canvas.TopProperty, top_height + ob_gap);
					bottom.SetValue(Canvas.LeftProperty, obstacle.Left);

					obstacle.VisualRect_top = top;
					obstacle.VisualRect_bottom = bottom;
					WindowCanvas.Children.Add(top);
					WindowCanvas.Children.Add(bottom);

					obstacle.Left -= ob_Speed;

					if(obstacle.Left + ob_Width < 0.0)
					{
						obstacle.Left = Width;
						obstacle.Height = r.NextDouble();
						obstacle.Neg = (r.Next() % 2) * 2 - 1;
						obstacle.IsHit = false;
					}


					// TODO -- fix collision detection

					//////////// collision detection 
					//////////if(!obstacle.IsHit && IsCollision(CurrentPlayer, obstacle.VisualRect_top) || IsCollision(CurrentPlayer, obstacle.VisualRect_bottom))
					//////////{
					//////////	obstacle.IsHit = true;
					//////////	isNewGame = false;

					//////////	GameOverEvent(this, EventArgs.Empty);

					//////////	GameplayTimer.Stop();
					//////////	return;
					//////////}
				}

			#endregion

		}



		void SetGameplayTimerToDefault()
		{
			GameplayTimer.Interval = TimeSpan.FromSeconds(DefaultGameplaySpeed);
		}

		void SetGameplayTimerToForewardSpeed()
		{
			GameplayTimer.Interval = TimeSpan.FromSeconds(ForewardGameplaySpeed);
		}







		// TEMPORARY vars 
		private double DefaultGameplaySpeed = 0.5;
		private double ForewardGameplaySpeed = 0.01;

		int User_KeyPressedTime = 0;

		double Player_BasePosition;

		



		private double Player_LiftSpeedBoost = 4.0;
		private double Player_ForwardSpeedBoost = 14.0;
		private double Player_BackwardSpeedBoost = 4.0;


		private double Inertia = 2.0;

		// END TEMPORARY vars


		DispatcherTimer ForewardTimer = new DispatcherTimer() { Interval = TimeSpan.FromSeconds(0.01) };

		DispatcherTimer BackwardTimer = new DispatcherTimer() { Interval = TimeSpan.FromSeconds(0.001) };


		DispatcherTimer UpwardTimer = new DispatcherTimer() { Interval = TimeSpan.FromSeconds(0.001) };
		DispatcherTimer DownwardTimer = new DispatcherTimer() { Interval = TimeSpan.FromSeconds(0.001) };



		double InertiaIterator = 0;


		/// <summary>
		/// Locks multiple instances of timer
		/// </summary>
		bool Lock = true;









		public void ForewardTimer_Tick(object sender, EventArgs e)
		{
			var f = Player_BasePosition + CurrentPlayer.MaximumSpeed -
				CurrentPlayer.MaximumSpeed * Math.Exp(-((InertiaIterator+=0.5)) * Player.ForewardSpeedModifier);
			
			if(f >= CurrentPlayer.MaximumSpeed)
			{
				ForewardTimer.Stop();
				return;
			}

			CurrentPlayer.Player_CurrentSpeed = CurrentPlayer.Player_ForwardPosition - Player.Player_DefaultForwardPosition;
			CurrentPlayer.Player_ForwardPosition = f;
			UpdatePlayerPosition();
		}

		public void BackwardTimer_Tick(object sender, EventArgs e)
		{
			var v = Player_BasePosition * Math.Exp(-(InertiaIterator += 0.5) * Player.BackwardSpeedModifier);
			CurrentPlayer.Player_CurrentSpeed = v;

			if((int)CurrentPlayer.Player_CurrentSpeed <= CurrentPlayer.MinimumSpeed)
			{
				BackwardTimer.Stop();
				CurrentPlayer.Player_CurrentSpeed = CurrentPlayer.MinimumSpeed;
				StarSpeedModifier = 2.0;

				Lock = false;
			}

			CurrentPlayer.Player_ForwardPosition = v;

			UpdatePlayerPosition();
		}





		void ForwardMovement()
		{
			if(BackwardTimer.IsEnabled)
			{
				Lock = false;
				BackwardTimer.Stop();
			}

			if(Lock)
				return;
			Lock = true;

			Player_BasePosition = CurrentPlayer.Player_ForwardPosition;

			if(Player_BasePosition != CurrentPlayer.MaximumSpeed)
				ForewardTimer.Start();
		}


		void BackwardMovement()
		{
			Player_BasePosition = CurrentPlayer.Player_ForwardPosition;

			InertiaIterator = 0;
			BackwardTimer.Start();
		}



		int maxUpwardMovementStep;
		int maxDownwardMovementStep;

		public void UpwardTimer_Tick(object sender, EventArgs e)
		{
			var f = Player_BasePosition + maxUpwardMovementStep -
				maxUpwardMovementStep * Math.Exp(-((InertiaIterator += 0.5)) * 0.5);

			if(f >= Height-10 || f<=10)
			{
				UpwardTimer.Stop();
				return;
			}

			CurrentPlayer.Player_LiftPosition = f;
			UpdatePlayerPosition();
		}




		public void DownwardTimer_Tick(object sender, EventArgs e)
		{
			var f = Player_BasePosition + 0 -
				2 * Math.Exp(-((InertiaIterator -= 0.5)) * 0.2);

			if(f <= 110)
			{
				DownwardTimer.Stop();
				return;
			}

			Debug.WriteLine(f.ToString());

			CurrentPlayer.Player_LiftPosition = f;
			UpdatePlayerPosition();
		}







		private void UpwardMovement()
		{
			Player_BasePosition = CurrentPlayer.Player_LiftPosition;

			InertiaIterator = 0;
			UpwardTimer.Start();

			maxUpwardMovementStep = Height / 50;
		}



		private void DownwardMovement()
		{
			Player_BasePosition = CurrentPlayer.Player_LiftPosition;

			InertiaIterator = 0;
			DownwardTimer.Start();

			maxDownwardMovementStep = Height / 100;
		}







		private void Window_KeyDown(object sender, KeyEventArgs e)
		{
			if(e.Key == Key.Right)
				ForwardMovement();

			if(e.Key == Key.Up)
				UpwardMovement();


			if(e.Key == Key.Down)
				DownwardMovement();






			if(isNewGame)
			{
				ResetAll();
				GameplayTimer.Start();
				isNewGame = false;
			}
		}

	
		private void Window_KeyUp(object sender, KeyEventArgs e)
		{

			if(e.Key == Key.Right)
			{
				ForewardTimer.Stop();

				BackwardMovement();
			}


			if(e.Key == Key.Up)
				UpwardTimer.Stop();


			if(e.Key == Key.Down)
				DownwardTimer.Stop();

		}




		private void Window_MouseDown(object sender, MouseButtonEventArgs e)
		{
			if(isNewGame)
			{

				BackwardTimer.Tick += BackwardTimer_Tick;
				ForewardTimer.Tick += ForewardTimer_Tick;
				UpwardTimer.Tick += UpwardTimer_Tick;
				DownwardTimer.Tick += DownwardTimer_Tick;


				ResetAll();

				GameplayTimer.Start();  // DONT TOUCH THE		

				StarSpeedModifier = 2.0;
				//Timer_Tick(null, null);

				isNewGame = false;
			}
		}



		#endregion











	}

}
