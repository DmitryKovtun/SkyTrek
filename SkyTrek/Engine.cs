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
using SkyTrekVisual.GameItems.Helpers;
using SkyTrekVisual.GameItems.Rockets;

namespace SkyTrek
{
	/// <summary>
	/// How it works nobody knows
	/// </summary>
	public class Engine
	{
		#region TODO - place textblocks somewhere outside of engine

		public TextBlock topScoretext = new TextBlock();
		public TextBlock speed = new TextBlock();

		#endregion
		
		#region Obstacles - LEGACY (flappy mode)

		private readonly double ob_GapEnd = 60;
		private readonly double ob_Width = 50.0;
		private readonly double ob_GapBase = 200.0;
		private readonly double ob_Speed = 4.0;

		#endregion

		#region OLD

		/// <summary>
		/// List of obstacles			-- TODO fix?
		/// </summary>
		private List<ObstacleFlapppy> ObstactleList = new List<ObstacleFlapppy>();

		/// <summary>
		/// Defines if there is flicker of player on startup
		/// </summary>
		private bool isStartupFlicker = false;

		/// <summary>
		/// Defines whether to use obstacle generation and updating
		/// </summary>
		private bool isObstacleEnabled = false;
		
		/// <summary>
		/// LEGACY
		/// </summary>
		private double topScore = 0;

		#endregion


		/// <summary>
		/// Do not the red button
		/// </summary>
		private DispatcherTimer GameplayTimer;


		DispatcherTimer ScreensaverTimer;


		private int Counter = 0;

		/// <summary>
		/// Random for all generation things
		/// </summary>
		private readonly Random r = new Random();

		/// <summary>
		/// Defines whether to show startup screen
		/// </summary>
		private bool isNewGame = true;

		/// <summary>
		/// Defines maximum background object size.
		/// Updatable screen area will be expanded by this value
		/// </summary>
		private int MaxObjectSize = 64;

		/// <summary>
		/// Just a player
		/// </summary>
		public Player CurrentPlayer;


		/// <summary>
		/// Is raised when player loses a game
		/// </summary>
		public event EventHandler GameOverEvent;


		#region Background items 

		private static int StarCount = 300;
		private static int PlanetCount = 7;
		private static int AsteriodCount = 17;

		/// <summary>
		/// actually dunno what is this
		/// </summary>
		private int straight_counter = 100;

		/// <summary>
		/// Defines how much background items will change their position every tick
		/// </summary>
		double BackgroundSpeedModifier = .15;     // def 1.5

		#endregion


		#region Canvases

		public Canvas BackdroundCanvas { get; set; }
		public Canvas PlayerCanvas { get; set; }
		public Canvas EnemyCanvas { get; set; }
		public Canvas ExplosionCanvas { get; set; }
		public Canvas ScreensaverCanvas { get; set; }


		/// <summary>
		/// Height of updatable screen area
		/// </summary>
		private int Height;

		/// <summary>
		/// Width of updatable screen area
		/// </summary>
		private int Width;


		#endregion


		/// <summary>
		/// Ctor
		/// </summary>
		/// <param name="window"></param>
		public Engine(MainWindow window)
		{
			//ScreensaverCanvas = window.ScreensaverCanvas;

			BackdroundCanvas = window.BackdroundCanvas;
			PlayerCanvas = window.PlayerCanvas;
			EnemyCanvas = window.EnemyCanvas;
			ExplosionCanvas = window.ExplosionCanvas;

			Height = (int)BackdroundCanvas.ActualHeight;
			Width = (int)(BackdroundCanvas.ActualWidth + MaxObjectSize);

			window.KeyUp += Window_KeyUp;
			window.KeyDown += Window_KeyDown;
			window.MouseDown += Window_MouseDown;


			Explosion.InitializeImages();


			Initialize();

            Player.EnemyCanvas = EnemyCanvas;
			CollisionDetector.CanvasHeight = Height;


			InitializeScreensaver();


			//TextureManager.LoadTextures();

		}





		void InitializeScreensaver()
		{

            return;

			ScreensaverTimer = new DispatcherTimer() { Interval = TimeSpan.FromSeconds(DefaultGameplaySpeed) };
			ScreensaverTimer.Tick += ScreensaverUpdater;

            for (int i = 0; i < StarCount; i++)
                ScreensaverCanvas.Children.Add(new Star(r.Next() % (Width + MaxObjectSize) - MaxObjectSize, r.Next() % Height));

        }




		public void ScreensaverUpdater(object sender, EventArgs e)
		{
			foreach(IGameItem gameplayItem in ScreensaverCanvas.Children)
			{
				if(gameplayItem.CoordLeft < -MaxObjectSize + 1)
				{
					gameplayItem.CoordLeft += Width;
					gameplayItem.CoordBottom = r.Next() % Height;

					gameplayItem.GenerateType();
					gameplayItem.GenerateSize();
				}

				var l = (gameplayItem as UserControl).ActualHeight;
				//gameplayItem.CoordLeft -= (straight_counter * BackgroundSpeedModifier / (gameplayItem as UserControl).ActualHeight) % Width;	// dist



				gameplayItem.CoordLeft -= ((straight_counter * BackgroundSpeedModifier/250* l)) % Width;
			}

		}


		public void RunScreensaver()
		{
			//ScreensaverTimer.Start();
		}


		internal void PauseScreensaver()
		{
			ScreensaverTimer.Stop();
		}






		/// <summary>
		/// Init all variables of engine
		/// </summary>
		public void Initialize()
		{
			GameplayTimer = new DispatcherTimer() { Interval = TimeSpan.FromSeconds(DefaultGameplaySpeed) };
			GameplayTimer.Tick += BackgroundUpdater;
			GameplayTimer.Tick += UserMovement_Tick;

			GameplayTimer.Tick += PlayerShipUpdater_Tick;
			GameplayTimer.Tick += PlayerShootingUpdater_Tick;

			GameplayTimer.Tick += ExplosionUpdater_Tick;
			GameplayTimer.Tick += EnemyUpdater_Tick;

			GameplayTimer.Tick += ItemDisposingUpdater_Tick;

			CurrentPlayer = new Player();

		}


		private void InitializeCanvases()
		{
			for(int i = 0; i < StarCount; i++)
				BackdroundCanvas.Children.Add(new Star(r.Next() % (Width + MaxObjectSize) - MaxObjectSize, r.Next() % Height));

			//for(int i = 0; i < PlanetCount; i++)
			//	BackdroundCanvas.Children.Add(new Planet(r.Next() % (Width + MaxObjectSize) - MaxObjectSize, r.Next() % Height));

			//for(int i = 0; i < AsteriodCount; i++)
			//	BackdroundCanvas.Children.Add(new Asteriod(r.Next() % (Width + MaxObjectSize) - MaxObjectSize, r.Next() % Height));



			PlayerCanvas.Children.Add(CurrentPlayer);
		}








		/// <summary>
		/// Resets game
		/// </summary>
		public void ResetAll()
		{
			//Counter = 0;

			//ObstactleList.Clear();
			//for(int i = 0; i < Partitions; i++)
			//	ObstactleList.Add(new Obstacle() { Height = r.NextDouble(), Left = 500 + (Width + ob_Width) * (i / Partitions), 
			//Neg = (r.Next() % 2) * 2 - 1 });

			BackdroundCanvas.Children.Clear();
			EnemyCanvas.Children.Clear();
			PlayerCanvas.Children.Clear();
			ExplosionCanvas.Children.Clear();

			CurrentPlayer.CoordLeft = Player.Player_DefaultLeftPosition;
			CurrentPlayer.CoordBottom = Player.Player_DefaultBottomPosition;
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
			double r1T = (double)r1.GetValue(Canvas.BottomProperty);
			double r1R = r1L + r1.Width;
			double r1B = r1T + r1.Height;

			double r2L = (double)r2.GetValue(Canvas.LeftProperty);
			double r2T = (double)r2.GetValue(Canvas.BottomProperty);
			double r2R = r2L + r2.Width;
			double r2B = r2T + r2.Height;

			if(r1T < 0)
				return true;
			if(r1B > Height)
				return true;

			return r1R > r2L && r1L < r2R && r1B > r2T && r1T < r2B;
		}











		#region EXPERIMENTAL part - do not touch the RED button

		#region Gameplay control

		public void Resume()
		{
			GameplayTimer.Start();
		}

		public void Pause()
		{
			GameplayTimer.Stop();
		}


		public bool IsActive()
		{
			return GameplayTimer.IsEnabled;
		}

		#endregion


		private double DefaultGameplaySpeed = 0.01;	// 0.5 fow slow

		private bool isMovingUpward = false;
		private bool isMovingDownward = false;
		private bool isMovingForward = false;
		private bool isMovingBackward = false;

		double ForwardIterator = 0;
		double BackwardIterator = 0;
		double UpwardIterator = 0;
		double DownwardIterator = 0;

		public int BulletSpeedModifier { get; private set; } = 1;

		private int BulletRemoveIterator = 0;










		#region Timer updaters for each tick

		/// <summary>
		/// Background canvas updater
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public void BackgroundUpdater(object sender, EventArgs e)
		{
			//-32 ------ Width + 32
			//Width + 32 -------- Width + 40

			foreach(IGameItem gameplayItem in BackdroundCanvas.Children)
			{
				if(gameplayItem.CoordLeft < -MaxObjectSize+1)
				{
					gameplayItem.CoordLeft += Width;
					gameplayItem.CoordBottom = r.Next() % Height;

					gameplayItem.GenerateType();
					gameplayItem.GenerateSize();
				}

				var l = (gameplayItem as UserControl).ActualHeight;
				//gameplayItem.CoordLeft -= (straight_counter * BackgroundSpeedModifier / (gameplayItem as UserControl).ActualHeight) % Width;	// dist
				gameplayItem.CoordLeft -= (straight_counter * BackgroundSpeedModifier/100* l) % Width;
			}

		}


		int iterator = 0;

		private void GameOver()
		{
			GameplayTimer.Stop();
			GameOverEvent.Invoke(null, null);

			isNewGame = true;
		}




		/// <summary>
		/// Updates explosions
		/// Enemy canvas updater
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public void EnemyUpdater_Tick(object sender, EventArgs e)
		{
			foreach(Enemy enemy in EnemyCanvas.Children.OfType<Enemy>())
			{
				if(enemy.IsCollision(CurrentPlayer))
				{
					GameOver();

				}

				enemy.GoBackward();
			}





			if(iterator++ %100 == 0)
				EnemyCanvas.Children.Add(new Enemy(Width, r.Next() % (Height-64) + 20));

		}

		/// <summary>
		/// Updates explosions
		/// Enemy canvas updater
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public void ExplosionUpdater_Tick(object sender, EventArgs e)
		{
			foreach(Explosion exp in ExplosionCanvas.Children)
			{
				exp.GenerateType();
			}

			if(BulletRemoveIterator < ExplosionCanvas.Children.Count)
			{
				if(!(ExplosionCanvas.Children[BulletRemoveIterator] as Explosion).isActive)
					ExplosionCanvas.Children.RemoveAt(BulletRemoveIterator);
			}


		}


		List<Bullet> DisposableBullets = new List<Bullet>();
		List<Enemy> DisposableEnemies = new List<Enemy>();

		List<IDestructibleItem> DisposableItems = new List<IDestructibleItem>();


		public void ItemDisposingUpdater_Tick(object sender, EventArgs e)
		{
			//foreach(var bullet in DisposableBullets)
			//	EnemyCanvas.Children.Remove(bullet);

			//DisposableBullets.Clear();

			//foreach(var enemy in DisposableEnemies)
			//	EnemyCanvas.Children.Remove(enemy);

			//DisposableEnemies.Clear();

			foreach(var enemy in DisposableItems)
				EnemyCanvas.Children.Remove(enemy as UIElement);

			DisposableItems.Clear();
			
		}

		/// <summary>
		/// Updates bullets
		/// Enemy canvas updater
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public void PlayerShootingUpdater_Tick(object sender, EventArgs e)
		{

			foreach(Rocket rocket in EnemyCanvas.Children.OfType<Rocket>())
			{
				if(rocket.CoordLeft > Width)
				{
					DisposableItems.Add(rocket);
				}

				foreach(Enemy enemy in EnemyCanvas.Children.OfType<Enemy>())
				{
					if(rocket.IsCollision(enemy))
					{
						DisposableItems.Add(enemy);
						rocket.Bang();

						//var v = new Explosion(rocket);
						//ExplosionCanvas.Children.Add(v);

						//DisposableItems.Add(rocket);
					}
				}

			}

		}

		/// <summary>
		/// Updates speed TODO : (and UI)
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public void PlayerShipUpdater_Tick(object sender, EventArgs e)
		{
			CurrentPlayer.GenerateType();

			#region Counters

			//straight_counter++;
			Counter++;

			topScore = Counter > topScore ? Counter : topScore;

			#endregion

			#region SPEED

			// TODO view model

			speed.Background = new SolidColorBrush(Colors.Transparent);
			speed.Margin = new Thickness(5, 35, 0, 0);
			speed.FontSize = 20.0;
			speed.Foreground = new SolidColorBrush(Colors.White);
			speed.Text = "SPEED: " + CurrentPlayer.CoordLeft.ToString() + "  ";
	
			#endregion

			#region Startup flicker

			if(isStartupFlicker)
			{
				if(Counter > 30 || (Counter < 30 && Counter % 5 < 3))
					PlayerCanvas.Children.Add(CurrentPlayer);		// fix need to be removed
			}

			#endregion

			#region Obstacle updating

			return;
			if(isObstacleEnabled)
				foreach(ObstacleFlapppy obstacle in ObstactleList)
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
					top.SetValue(Canvas.BottomProperty, 0.0);
					top.SetValue(Canvas.LeftProperty, obstacle.Left);

					Rectangle bottom = new Rectangle()
					{
						Width = ob_Width,
						Height = Height - top_height - ob_gap,
						Stroke = new SolidColorBrush(Colors.White),
						StrokeThickness = 0,
						Fill = new SolidColorBrush(color)
					};
					bottom.SetValue(Canvas.BottomProperty, top_height + ob_gap);
					bottom.SetValue(Canvas.LeftProperty, obstacle.Left);

					obstacle.VisualRect_top = top;
					obstacle.VisualRect_bottom = bottom;
					PlayerCanvas.Children.Add(top);
					PlayerCanvas.Children.Add(bottom);

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

		/// <summary>
		/// Updates movement of player
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public void UserMovement_Tick(object sender, EventArgs e)
		{
			if(isMovingForward && !CurrentPlayer.IsSpeedMaximum())
			{
				if(isMovingBackward)
					isMovingBackward = false;

				int f = (int)(CurrentPlayer.CoordLeft + CurrentPlayer.MaximumSpeed -
					CurrentPlayer.MaximumSpeed * Math.Exp(-((ForwardIterator += 0.5)) * CurrentPlayer.ForwardSpeedModifier));

				if(f < CurrentPlayer.MaximumSpeed)
					CurrentPlayer.CoordLeft = f;
			}

			if(isMovingBackward && !isMovingForward)
			{
				int v = (int)(CurrentPlayer.CoordLeft * Math.Exp(-(BackwardIterator += 0.5) * CurrentPlayer.BackwardSpeedModifier));

				if(CurrentPlayer.IsSpeedMinimum())
				{
					CurrentPlayer.CoordLeft = CurrentPlayer.MinimumSpeed;
					isMovingBackward = false;
				}

				CurrentPlayer.CoordLeft = v;
			}

			if(isMovingUpward)
			{
				int f = (int)(CurrentPlayer.CoordBottom + 8 * Math.Exp(-((UpwardIterator += 0.5)) * 0.3));

				if(f < Height - CurrentPlayer.ActualHeight +20)
					CurrentPlayer.CoordBottom = f;
			}

			if(isMovingDownward)
			{
				int f = (int)(CurrentPlayer.CoordBottom - 2 * Math.Exp(-((DownwardIterator -= 0.5)) * 0.2));

				if(f > -20)
					CurrentPlayer.CoordBottom = f;			
			}
		}


		#endregion





		#region User input event handlers

		/// <summary>
		/// When key is down
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Window_KeyDown(object sender, KeyEventArgs e)
		{
			if(e.Key == Key.Right)
				isMovingForward = true;	

			if(e.Key == Key.Up)
			{
				isMovingUpward = true;
				UpwardIterator = 0;
			}

			if(e.Key == Key.Down)
			{
				isMovingDownward = true;
				DownwardIterator = 0;
			}

			if(e.Key == Key.Space)
				CurrentPlayer.MakeAShot();

			if(isNewGame)
				TryStartNewGame();
		}

		/// <summary>
		/// When key is up
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Window_KeyUp(object sender, KeyEventArgs e)
		{
			if(e.Key == Key.Right)
			{
				isMovingBackward = true;
				isMovingForward = false;
				BackwardIterator = 0;
				ForwardIterator = 0;
			}

			if(e.Key == Key.Up)
				isMovingUpward = false;

			if(e.Key == Key.Down)
				isMovingDownward = false;

            if (e.Key == Key.Space)
                CurrentPlayer.EndAShot();
        }

		/// <summary>
		/// When mouse click
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Window_MouseDown(object sender, MouseButtonEventArgs e)
		{
			TryStartNewGame();
		}

		#endregion








		/// <summary>
		/// Starts new game if isNewGame is true
		/// </summary>
		private void TryStartNewGame()
		{
			if(isNewGame)
			{
				ResetAll();
				InitializeCanvases();
				

				GameplayTimer.Start();  // DONT TOUCH THE	
				
				isNewGame = false;
			}

		}


		#endregion




	}

}
