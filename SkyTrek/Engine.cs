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

		private List<IBackgroundGameItem> BackgroundItems = new List<IBackgroundGameItem>();

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

		/// <summary>
		/// BACKUP Start position in Canvas - horizontal						-- TODO - fix it
		/// </summary>
		private static readonly double Player_DefaultForwardPosition = 246.0;

		/// <summary>
		/// BACKUP Start position in Canvas - vertical		//default 200		-- TODO - fix it	
		/// </summary>
		private static readonly double Player_DefaultLiftPosition = 200.0;

		/// <summary>
		/// Start position in Canvas - horizontal
		/// </summary>
		private double Player_ForwardPosition = Player_DefaultForwardPosition;

		/// <summary>
		/// Start position in Canvas - vertical. Defines how high is player on canvas
		/// </summary>
		private double Player_LiftPosition = Player_DefaultLiftPosition;

		/// <summary>
		/// Size of a ship
		/// </summary>
		private double Player_Size = 48.0;

		/// <summary>
		/// ????
		/// </summary>
		private double Player_Speed = 0.0;



		private double Player_CurrentSpeed = 0.0;

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
		Rectangle Player;

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


			// temp mode 
			if(isFlapping)
			{
				window.KeyUp += KeyUpEventFlappy;
				window.KeyDown += KeyDownEventFlappy;
				window.MouseDown += WindowCanvasMouseDownEventFlappy;
			}
			else
			{
				window.KeyUp += Window_KeyUp;
				window.KeyDown += Window_KeyDown;
				window.MouseDown += Window_MouseDown;
			}
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

			IB = new ImageBrush
			{
				Stretch = Stretch.UniformToFill,
				ImageSource = new BitmapImage(new Uri(Directory.GetCurrentDirectory().ToString() + @"\Ships\Ship1.png", UriKind.Relative))
			};

			Player = new Rectangle()
			{
				Width = Player_Size,
				Height = Player_Size,
				Stroke = new SolidColorBrush(Colors.White),
				StrokeThickness = 0,
				Fill = IB
			};
		}


		/// <summary>
		/// LEGACY flappy bird mode screen updater
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public void BackgroundUpdaterFlappy(object sender, EventArgs e)
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


			#region Scores

			topScoretext = new TextBlock
			{
				FontWeight = FontWeights.Bold,
				Foreground = new SolidColorBrush(Colors.DarkSlateBlue),
				Background = new SolidColorBrush(Colors.Transparent),
				Margin = new Thickness(5, 5, 0, 0),
				FontSize = 20.0,
				Text = "  " + topScore.ToString() + "  "
			};
			if(topScore == Counter && Counter % 20 < 10)
				topScoretext.Text = "  " + topScore.ToString() + " ! ";


			score = new TextBlock();
			if((Counter - LastMouseCounter) > mouseupthreshold)
			{
				score.Foreground = new SolidColorBrush(Colors.Green);
				score.FontWeight = FontWeights.Bold;
			}

			score.Background = new SolidColorBrush(Colors.Transparent);
			score.Margin = new Thickness(5, 35, 0, 0);
			score.FontSize = 20.0;
			score.Foreground = new SolidColorBrush(Colors.DarkSlateBlue);
			score.Text = "  " + Counter.ToString() + "  ";


			WindowCanvas.Children.Add(score);
			WindowCanvas.Children.Add(topScoretext);

			#endregion


			Player.Fill = IB;

			#region Logic of flappy bird


			if(!spacedown || (spacedown && Counter <= 0))
			{
				Player_Speed += Gravitation;
				Player_LiftPosition -= Player_Speed;
			}

			// for flickering effect
			if((Counter - LastMouseCounter) > mouseupthreshold && Counter % 10 < 5)
				Player.Fill = new SolidColorBrush(Colors.Transparent);

			// counter modification for flapping
			if((Counter - LastMouseCounter) > mouseupthreshold)
				Counter += 5;
			if(spacedown)
				Counter -= 10;



			#endregion

			UpdatePlayerPosition();

			#region Startup flicker

			if(isStartupFlicker)
			{
				if(Counter > 30 || (Counter < 30 && Counter % 5 < 3))
					WindowCanvas.Children.Add(Player);
			}
			else
				WindowCanvas.Children.Add(Player);

			#endregion

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


					// collision detection 
					if(!obstacle.IsHit && IsCollision(Player, obstacle.VisualRect_top) || IsCollision(Player, obstacle.VisualRect_bottom))
					{
						obstacle.IsHit = true;
						isNewGame = false;

						GameOverEvent(this, EventArgs.Empty);

						GameplayTimer.Stop();
						return;
					}
				}


		}




		/// <summary>
		/// Updates players` position on canvas
		/// </summary>
		private void UpdatePlayerPosition()
		{
			Player.SetValue(Canvas.TopProperty, Height - Player_LiftPosition);
			Player.SetValue(Canvas.LeftProperty, Player_ForwardPosition);
		}





		#region Working flappy bird events 

		/// <summary>
		/// Key up
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void KeyUpEventFlappy(object sender, KeyEventArgs e)
		{
			if(isNewGame)
			{
				ResetAll();
				GameplayTimer.Start();
				isNewGame = false;
			}
		}

		/// <summary>
		/// Key down
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void KeyDownEventFlappy(object sender, KeyEventArgs e)
		{
			if(e.Key == Key.Space)
			{
				WindowCanvasMouseDownEventFlappy(null, null);
			}

			KeyUpEventFlappy(sender, null); // place for KeyUpEvent - just for not using same piece of code
		}

		/// <summary>
		/// Mouse down
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void WindowCanvasMouseDownEventFlappy(object sender, MouseButtonEventArgs e)
		{
			KeyUpEventFlappy(sender, null); // place for KeyUpEvent - just for not using same piece of code

			Player_Speed = -5;
			LastMouseCounter = Counter;
		}



		#endregion


		/// <summary>
		/// Resets game
		/// </summary>
		public void ResetAll()
		{
			Counter = 0;

			ObstactleList.Clear();
			for(int i = 0; i < Partitions; i++)
				ObstactleList.Add(new Obstacle() { Height = r.NextDouble(), Left = 500 + (Width + ob_Width) * (i / Partitions), Neg = (r.Next() % 2) * 2 - 1 });

			Player_LiftPosition = 200.0;
			Player_Speed = 0.0;
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
			speed.Text = "SPEED: " + ((int)Player_CurrentSpeed).ToString() + "  ";

			WindowCanvas.Children.Add(speed);

			#endregion

			Player.Fill = IB;

			UpdatePlayerPosition();

			#region Startup flicker

			if(isStartupFlicker)
			{
				if(Counter > 30 || (Counter < 30 && Counter % 5 < 3))
					WindowCanvas.Children.Add(Player);
			}
			else
				WindowCanvas.Children.Add(Player);

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


					// collision detection 
					if(!obstacle.IsHit && IsCollision(Player, obstacle.VisualRect_top) || IsCollision(Player, obstacle.VisualRect_bottom))
					{
						obstacle.IsHit = true;
						isNewGame = false;

						GameOverEvent(this, EventArgs.Empty);

						GameplayTimer.Stop();
						return;
					}
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

		double Player_MaxSpeed = 120;


		private double Player_LiftSpeedBoost = 4.0;
		private double Player_ForwardSpeedBoost = 14.0;
		private double Player_BackwardSpeedBoost = 4.0;


		private double Inertia = 2.0;

		// END TEMPORARY vars


		DispatcherTimer ForewardTimer = new DispatcherTimer() { Interval = TimeSpan.FromSeconds(0.01) };

		DispatcherTimer BackwardTimer = new DispatcherTimer() { Interval = TimeSpan.FromSeconds(0.01) };


		double f = 0;
		double d = 0;
		bool Lock = true;


		public void ForewardTimer_Tick(object sender, EventArgs e)
		{
			Player_ForwardPosition = Player_BasePosition + Player_MaxSpeed - Player_MaxSpeed * Math.Exp(-((f+=0.5)) * 0.1);

			Player_CurrentSpeed = Player_ForwardPosition - Player_DefaultForwardPosition;

			UpdatePlayerPosition();
		}

		

		void ForwardInertia()
		{
			if(Lock)
				return;

			Lock = true;

			Player_BasePosition = Player_ForwardPosition;

			f = 0;
			ForewardTimer.Tick += ForewardTimer_Tick;

			StarSpeedModifier = 3.0;

			SetGameplayTimerToForewardSpeed();

			ForewardTimer.Start();
		}


		void BackwardInertia()
		{
			Player_BasePosition = Player_ForwardPosition;

			d = 0;
			BackwardTimer.Tick += BackwardTimer_Tick;


			BackwardTimer.Start();
		}


		public void BackwardTimer_Tick(object sender, EventArgs e)
		{
			var v = Player_BasePosition * Math.Exp(-(d+=0.5) * 0.01);

			Player_CurrentSpeed = v - Player_DefaultForwardPosition;


			while(GameplayTimer.Interval >= TimeSpan.FromSeconds(0.5))
			{
				GameplayTimer.Interval -= TimeSpan.FromSeconds(0.02);

			}

			if((int)Player_CurrentSpeed <= 0)
			{
				BackwardTimer.Stop();
				Player_CurrentSpeed = 0;
				StarSpeedModifier = 2.0;

				SetGameplayTimerToDefault();

				Lock = false;
				GameplayTimer.Start();
			}

			Player_ForwardPosition = v;

			UpdatePlayerPosition();
		}




		private void Window_KeyDown(object sender, KeyEventArgs e)
		{

			if(e.Key == Key.Up)
				ForwardInertia();

			if(isNewGame)
			{
				ResetAll();
				GameplayTimer.Start();
				isNewGame = false;
			}
		}


		private void Window_KeyUp(object sender, KeyEventArgs e)
		{
			if(e.Key == Key.Up)
			{
				ForewardTimer.Stop();

				BackwardInertia();
			}
		}

		private void Window_MouseDown(object sender, MouseButtonEventArgs e)
		{
			if(isNewGame)
			{
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
