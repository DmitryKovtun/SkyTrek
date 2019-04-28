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

/// <summary>
/// How it works nobody knows
/// </summary>
namespace SkyTrek
{
	public class Engine
	{
		public TextBlock topScoretext;
		public TextBlock score;


		private int straight_counter = 0;

		private double topScore = 0;

		private bool spacedown = false;

		

		private List<IBackgroundGameItem> BackgroundItems = new List<IBackgroundGameItem>();


		private static int StarCount = 300;
		private static int PlanetCount = 7;
		private static int AsteriodCount = 17;






		private readonly int mouseupthreshold = 30;

		private readonly double ob_GapEnd = 60;
		private readonly double ob_Width = 50.0;
		private readonly double ob_GapBase = 200.0;
		private readonly double ob_Speed = 4.0;

		private double Partitions = 3.0;



		private static readonly double Player_DefaultForwardPosition = 246.0;
		private static readonly double Player_DefaultLiftPosition = 200.0;

		private double Player_ForwardPosition = Player_DefaultForwardPosition;
		private double Player_LiftPosition = Player_DefaultLiftPosition; //def 200				// defines how high is player on canvas

		private double Player_Size = 48.0;
		private double Player_Speed = 0.0;

		private double Player_LiftSpeedBoost = 4.0;
		private double Player_ForwardSpeedBoost = 14.0;
		private double Player_BackwardSpeedBoost = 4.0;


		private double Player_CurrentSpeed = 0.0;



		private double Gravitation = 0.3; // less == longer fly
		private double Inertia = 2.0;



		private DispatcherTimer GameplayTimer;
		private int Counter = 0;
		private int LastMouseCounter = 0;

		private readonly Random r = new Random();

		private List<Obstacle> ObstactleList = new List<Obstacle>();


		private bool isNewGame = true;

		private int Height;
		private int Width;
		private bool isObstacleEnabled = false;


		private int MaxObjectSize = 64;


		double StarSpeedModifier = 3.0;     // def 1.5





		private bool isFlapping = false;			// defines behaviour of flappy bird
		private bool isStartupFlicker = false;		// defines if there is flicker of player on startup





		ImageBrush IB;
		Rectangle Player;





		private double DefaultGameplaySpeed = 0.5;
		private double ForewardGameplaySpeed = 0.01;








		public Engine(MainWindow window)
		{
			WindowCanvas = window.WindowCanvas;

			Height = (int)(WindowCanvas.ActualHeight + MaxObjectSize);
			Width = (int)(WindowCanvas.ActualWidth + MaxObjectSize);


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





		int User_KeyPressedTime = 0;


		double Player_BasePosition;



		double Player_MaxSpeed = 120;









		private void UpdatePlayerPosition()
		{
			Player.SetValue(Canvas.TopProperty, Height - Player_LiftPosition);
			Player.SetValue(Canvas.LeftProperty, Player_ForwardPosition);
		}




		double f = 0;


		public void ForewardTimer_Tick(object sender, EventArgs e)
		{
			Player_ForwardPosition = Player_BasePosition + Player_MaxSpeed - Player_MaxSpeed * Math.Exp(-((f+=0.5)) * 0.1);

			Player_CurrentSpeed = Player_ForwardPosition - Player_DefaultForwardPosition;

			UpdatePlayerPosition();
		}






		DispatcherTimer ForewardTimer = new DispatcherTimer() { Interval = TimeSpan.FromSeconds(0.01) };

		

		void SetGameplayTimerToDefault()
		{
			GameplayTimer.Interval = TimeSpan.FromSeconds(DefaultGameplaySpeed);

		}
		void SetGameplayTimerToForewardSpeed()
		{
			GameplayTimer.Interval = TimeSpan.FromSeconds(ForewardGameplaySpeed);

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



		double d = 0;




		void BackwardInertia()
		{
			Player_BasePosition = Player_ForwardPosition;

			d = 0;
			BackwardTimer.Tick += BackwardTimer_Tick;


			BackwardTimer.Start();
		}




		DispatcherTimer BackwardTimer = new DispatcherTimer() { Interval = TimeSpan.FromSeconds(0.01) };

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










		bool Lock = true;


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

















		void KeyUpEventFlappy(object sender, KeyEventArgs e)
		{
			if(isNewGame)
			{
				ResetAll();
				GameplayTimer.Start();
				isNewGame = false;
			}
		}

		void KeyDownEventFlappy(object sender, KeyEventArgs e)
		{
			if(e.Key == Key.Space)
			{
				WindowCanvasMouseDownEventFlappy(null, null);
			}

			KeyUpEventFlappy(sender, null); // place for KeyUpEvent - just for not using same piece of code
		}

		void WindowCanvasMouseDownEventFlappy(object sender, MouseButtonEventArgs e)
		{
			KeyUpEventFlappy(sender, null); // place for KeyUpEvent - just for not using same piece of code

			Player_Speed = -5;
			LastMouseCounter = Counter;
		}




		public Canvas WindowCanvas { get; set; }





		

		public void Initialize()
		{
			for(int i = 0; i < StarCount; i++)
				BackgroundItems.Add(new Star(r.Next() % (Width+MaxObjectSize) - MaxObjectSize, r.Next() % Height));

			for(int i = 0; i < PlanetCount; i++)
				BackgroundItems.Add(new Planet(r.Next() % (Width + MaxObjectSize) - MaxObjectSize, r.Next() % Height));

			for(int i = 0; i < AsteriodCount; i++)
				BackgroundItems.Add(new Asteriod(r.Next() % (Width + MaxObjectSize) - MaxObjectSize, r.Next() % Height));



			GameplayTimer = new DispatcherTimer() { Interval = TimeSpan.FromSeconds(DefaultGameplaySpeed) };
			GameplayTimer.Tick += Timer_Tick;


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




		public void Timer_Tick(object sender, EventArgs e)
		{
			#region Counters

			straight_counter++;
			Counter++;

			topScore = Counter > topScore ? Counter : topScore;

			#endregion

			WindowCanvas.Children.Clear();


			#region Stars updating


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


			//foreach(var planet in Planets)
			//{
			//	if(planet.CoordX - straight_counter * StarSpeedModifier < -MaxObjectSize)
			//	{
			//		planet.CoordX += Width;
			//		planet.CoordY = r.Next() % Height;

			//		planet.GenerateType();
			//	}

			//	planet.SetValue(Canvas.TopProperty, (double)planet.CoordY);
			//	planet.SetValue(Canvas.LeftProperty, (planet.CoordX - straight_counter * StarSpeedModifier) % Width);

			//	WindowCanvas.Children.Add(planet);
			//}



			#endregion


			#region Scores

			if(isFlapping)
			{
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
			}
			else
			{
				var speed = new TextBlock();
				speed.Background = new SolidColorBrush(Colors.Transparent);
				speed.Margin = new Thickness(5, 35, 0, 0);
				speed.FontSize = 20.0;
				speed.Foreground = new SolidColorBrush(Colors.White);
				speed.Text = "SPEED: " + Player_CurrentSpeed.ToString() + "  ";

				WindowCanvas.Children.Add(speed);
			}

			#endregion


			Player.Fill = IB;

			#region Logic of flappy bird

			if(isFlapping)
			{
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
			}
			else
			{
				Counter += 5;
			}

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
					if(!obstacle.IsHit && isCollision(Player, obstacle.VisualRect_top) || isCollision(Player, obstacle.VisualRect_bottom))
					{
						obstacle.IsHit = true;
						isNewGame = false;

						GameOverEvent(this, EventArgs.Empty);

						GameplayTimer.Stop();
						return;
					}
				}


		}

		
		public event EventHandler GameOverEvent;





		public void ResetAll()
		{
			Counter = 0;

			ObstactleList.Clear();
			for(int i = 0; i < Partitions; i++)
				ObstactleList.Add(new Obstacle() { Height = r.NextDouble(), Left = 500 + (Width + ob_Width) * (i / Partitions), Neg = (r.Next() % 2) * 2 - 1 });


			Player_LiftPosition = 200.0;
			Player_Speed = 0.0;

		}


		bool isCollision(Rectangle r1, Rectangle r2)
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


	}

}
