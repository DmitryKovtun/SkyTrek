using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using SkyTrek.Panels;
using SkyTrekVisual.GameItems;
using SkyTrekVisual.GameItems.Rockets;
using System.Windows.Input;
using System.Diagnostics;
using SkyTrekVisual.GameItems.BonusItems;
using SkyTrekVisual.GameItems.Helpers;

namespace SkyTrek
{
	/// <summary>
	/// How it works nobody knows
	/// </summary>

	[Serializable]
	public class Engine
	{
		#region TODO - place textblocks somewhere outside of engine

		private TextBlock topScoretext = new TextBlock();

		private TextBlock speed = new TextBlock();

		#endregion
		


		#region OLD

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
		/// 
		/// </summary>
		public SpaceShip PlayerShip;


		/// <summary>
		/// Is raised when player loses a game
		/// </summary>
		public event EventHandler GameOverEvent;



		bool isAutoHeal = true;









	#region Canvases

	//public Canvas BackdroundCanvas { get; set; }
	public Canvas PlayerCanvas { get; set; }
		public Canvas EnemyCanvas { get; set; }
		public Canvas ExplosionCanvas { get; set; }

		public Canvas ShotCanvas { get; set; }

		public Canvas LootCanvas { get; set; }



		/// <summary>
		/// Height of updatable screen area
		/// </summary>
		public int Height;

		/// <summary>
		/// Width of updatable screen area
		/// </summary>
		public int Width;


		#endregion



		/// <summary>
		/// Ctor
		/// </summary>
		/// <param name="window"></param>
		public Engine(Player currentPlayer)
		{
			Height = 454 - 48;
			Width = 950 + MaxObjectSize;


			Initialize();

			CollisionDetector.CanvasHeight = Height;

			CurrentPlayer = currentPlayer;
			PlayerShip = currentPlayer.Ship;

		}


		Grid PlayerDamageIndicator;

		Grid PlayerBonusIndicator;

		public void InitCanvases(GameplayPanel gameplayPanel)
        {
			PlayerDamageIndicator = gameplayPanel.PlayerDamageIndicator;
			PlayerDamageIndicator.Opacity = 0;

			PlayerBonusIndicator = gameplayPanel.PlayerBonusIndicator;
			PlayerBonusIndicator.Opacity = 0;

			gameplayPanel.BottomPanel.ShieldImage.Background = TextureManager.Bonuses[BonusType.Shield];

			gameplayPanel.BottomPanel.ReloadGrid.DataContext = CurrentPlayer.Ship.CurrentGun;
			gameplayPanel.BottomPanel.ShieldGrid.DataContext = CurrentPlayer.Ship;



			//BackdroundCanvas = window.Gameplay.BackdroundCanvas;
			PlayerCanvas = gameplayPanel.PlayerCanvas;
            EnemyCanvas = gameplayPanel.EnemyCanvas;
            ExplosionCanvas = gameplayPanel.ExplosionCanvas;
            ShotCanvas = gameplayPanel.ShotCanvas;
			LootCanvas = gameplayPanel.LootCanvas;

			GunShot.DefaultRocketCanvas = ShotCanvas;

			//window.KeyUp += Window_KeyUp;
			//window.KeyDown += Window_KeyDown;
			//window.MouseDown += Window_MouseDown;

		}










		/// <summary>
		/// Init all variables of engine
		/// </summary>
		public void Initialize()
		{
			GameplayTimer = new DispatcherTimer() { Interval = TimeSpan.FromSeconds(DefaultGameplaySpeed) };
			//GameplayTimer.Tick += BackgroundUpdater;
			GameplayTimer.Tick += UserMovement_Tick;

			GameplayTimer.Tick += PlayerShipUpdater_Tick;
			GameplayTimer.Tick += PlayerShootingUpdater_Tick;

			GameplayTimer.Tick += ExplosionUpdater_Tick;
			GameplayTimer.Tick += EnemyUpdater_Tick;

			GameplayTimer.Tick += EnemyItemDisposingUpdater_Tick;


			GameplayTimer.Tick += LootUpdater_Tick;

		}


		private void InitializeCanvases()
		{
			//for(int i = 0; i < StarCount; i++)
			//	BackdroundCanvas.Children.Add(new Star(r.Next() % (Width + MaxObjectSize) - MaxObjectSize, r.Next() % (Height+48)));

			//for(int i = 0; i < PlanetCount; i++)
			//	BackdroundCanvas.Children.Add(new Planet(r.Next() % (Width + MaxObjectSize) - MaxObjectSize, r.Next() % Height));

			//for(int i = 0; i < AsteriodCount; i++)
			//	BackdroundCanvas.Children.Add(new Asteriod(r.Next() % (Width + MaxObjectSize) - MaxObjectSize, r.Next() % Height));

			PlayerCanvas.Children.Add(PlayerShip);
		}




		#region reset

		/// <summary>
		/// Resets game
		/// </summary>
		public void ResetAll()
		{
			//Counter = 0;

			//BackdroundCanvas.Children.Clear();
			isUserInputAvailable = true;

			LootCanvas.Children.Clear();
			EnemyCanvas.Children.Clear();
			PlayerCanvas.Children.Clear();
			ExplosionCanvas.Children.Clear();
			ShotCanvas.Children.Clear();

			GC.Collect();

			PlayerShip.CoordLeft = SpaceShip.Ship_DefaultLeftPosition;
			PlayerShip.CoordBottom = SpaceShip.Ship_DefaultBottomPosition;

			CurrentPlayer.Reset();

			PlayerShip.Visibility = Visibility.Visible;
		}





		#endregion


		public Engine()
		{

		}


	


		#region do not touch the RED button



		#region Gameplay control

		public void Resume()
		{
			GameplayTimer.Start();

			PlayerShip.CurrentGun.Resume();

			foreach(Rocket rocket in ShotCanvas.Children)
				rocket.Resume();

			foreach(Enemy enemy in EnemyCanvas.Children)
				enemy.CurrentGun.Resume();

			PlayerShip.WasHit(0);
		}

		public void Pause()
		{
			GameplayTimer.Stop();

			PlayerShip.CurrentGun.Pause();

			foreach(Rocket rocket in ShotCanvas.Children)
				rocket.Pause();

			foreach(Enemy enemy in EnemyCanvas.Children)
				enemy.CurrentGun.Pause();

			PlayerShip.WasHit(0);
		}


		public bool IsActive()
		{
			return GameplayTimer.IsEnabled;
		}

		#endregion



		#region some engine props

		private double DefaultGameplaySpeed = 0.03; // 0.5 fow slow

		public bool isMovingUpward = false;
		public bool isMovingDownward = false;
		public bool isMovingForward = false;
		public bool isMovingBackward = false;
		public bool isMovingBackwardFloat = false;


		double ForwardIterator = 0;
		double BackwardIterator = 0;
		double UpwardIterator = 0;
		double DownwardIterator = 0;

		public int BulletSpeedModifier { get; private set; } = 1;

		private int BulletRemoveIterator = 0;

	

		#endregion


		#region Timer updaters for each tick


		int iterator = 0;
		int countIII = 0;

		bool isUserInputAvailable = true;


		private void GameOverWithPlayerExplosion()
		{
			//Pause();

			PlayerShip.Visibility = Visibility.Hidden;

			PlayerShip.StartShipExplosion(ExplosionCanvas);

			isUserInputAvailable = false;
			GameplayTimer.Tick += GameOverLastExplosion_Tick;

		}

		private void GameOverLastExplosion_Tick(object sender, EventArgs e)
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

			if(ExplosionCanvas.Children.Count==0)
				GameOver();

		}

		/// <summary>
		/// 
		/// </summary>
		private void GameOver()
		{
			isUserInputAvailable = false;

			Pause();

			PlayerShip.Visibility = Visibility.Hidden;

			GameplayTimer.Tick -= GameOverLastExplosion_Tick;

			GameOverEvent.Invoke(this, null);
			isNewGame = true;
		}






		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void LootUpdater_Tick(object sender, EventArgs e)
		{


			foreach(NewAsteroid asteroid in LootCanvas.Children.OfType<NewAsteroid>())
			{
				if(asteroid.CoordLeft < -18)
				{
					DisposableItems.Add(asteroid);
					return;
				}

				if(asteroid.IsCollision(PlayerShip))
				{
					if(!PlayerShip.IsInvincible)
					{
						PlayerShip.WasHit(asteroid.HitDamage);
						CurrentPlayer.Score.Multiplier = CurrentPlayer.Score.Multiplier / 2;
						PlayerDamageIndicator.Opacity += asteroid.HitDamage * .1;
					}

					ExplosionCanvas.Children.Add(new Explosion(asteroid, 2));
					DisposableItems.Add(asteroid);

					CurrentPlayer.Score.NewShipHit();
				}
				else
					asteroid.GoBackward();
			}


			foreach(BonusItem bonus in LootCanvas.Children.OfType<BonusItem>())
			{
				if(bonus.CoordLeft < -18)
				{
					DisposableItems.Add(bonus);
					return;
				}

				if(bonus.IsCollision(PlayerShip))
				{
					DisposableItems.Add(bonus);

					PlayerBonusIndicator.Opacity += 1;
					CurrentPlayer.GotBonus(bonus);
				}
				else
					bonus.GoBackward();
			}


			foreach(Enemy enemyWithCollision in LootCanvas.Children.OfType<Enemy>())
			{
				if(enemyWithCollision.CoordLeft < -32)
				{
					DisposableCollisionItems.Add(enemyWithCollision);
					return;
				}

				if(enemyWithCollision.IsShipCollision(PlayerShip))
				{
					if(!PlayerShip.IsInvincible)
					{
						PlayerShip.WasHit(enemyWithCollision.HitDamage);
						CurrentPlayer.Score.Multiplier = CurrentPlayer.Score.Multiplier / 2;
						PlayerDamageIndicator.Opacity += enemyWithCollision.HitDamage * .1;
					}

					ExplosionCanvas.Children.Add(new Explosion(enemyWithCollision, 2));


					DisposableCollisionItems.Add(enemyWithCollision);
					CurrentPlayer.Score.NewShipHit();
				}
				else
				{
					enemyWithCollision.GoBackward();

					if(enemyWithCollision.startPointLeft - enemyWithCollision.CoordLeft >=48)
						enemyWithCollision.IsInvincible = false;

				}
			}


		
			if(CurrentPlayer.HealthPoints < 20)
			{
				if(oneTimeHelp)
				{
					LootCanvas.Children.Add(new BonusItem(Width, r.Next() % (Height - 64) + 5,BonusType.Health));
					oneTimeHelp = false;
				}
			}
			else if(CurrentPlayer.HealthPoints > 50)
				oneTimeHelp = true;



			if(iterator % r.Next(90, 120) == 0)
				LootCanvas.Children.Add(new NewAsteroid(Width, r.Next() % (Height - 64) + 5));


			if(r.Next(2, 510) == 90)
				LootCanvas.Children.Add(new BonusItem(Width, r.Next() % (Height - 64) + 5));




		}



		bool oneTimeHelp = true;







		/// <summary>
		/// Updates explosions
		/// Enemy canvas updater
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public void EnemyUpdater_Tick(object sender, EventArgs e)
		{
			foreach(Enemy enemy in EnemyCanvas.Children)
			{
				if(enemy.CoordLeft < -32)
				{

					DisposableItems.Add(enemy);
					return;
				}


				if(enemy.CoordBottom -32 <= PlayerShip.CoordBottom && enemy.CoordBottom + 32 >= PlayerShip.CoordBottom)
				{
					if(r.Next() % 2 == 0)
					{
						enemy.MakeAShot();			
					}
				}

				if(enemy.IsShipCollision(PlayerShip))
				{
					if(!PlayerShip.IsInvincible)
					{
						PlayerShip.WasHit(enemy.HitDamage);
						CurrentPlayer.Score.Multiplier = CurrentPlayer.Score.Multiplier / 2;
						PlayerDamageIndicator.Opacity += enemy.HitDamage * .1;
					}

					ExplosionCanvas.Children.Add(new Explosion(enemy, 7));

					enemy.StartShipExplosion(ExplosionCanvas);

					DisposableItems.Add(enemy);




					CurrentPlayer.Score.NewShipHit();
				}
				else
					enemy.GoBackward();
			}

			if(iterator++ %(r.Next(90,100)) == 0)
				EnemyCanvas.Children.Add(new Enemy(Width, r.Next() % (Height-64)+5));

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


		/// <summary>
		/// 
		/// </summary>
		List<IDestructibleItem> DisposableItems = new List<IDestructibleItem>();

		/// <summary>
		/// 
		/// </summary>
		List<IDestructibleItem> DisposableCollisionItems = new List<IDestructibleItem>();


		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public void EnemyItemDisposingUpdater_Tick(object sender, EventArgs e)
		{

			foreach(var item in DisposableItems.OfType<Enemy>())
				EnemyCanvas.Children.Remove(item as UIElement);

			foreach(var item in DisposableItems.OfType<Rocket>())
				ShotCanvas.Children.Remove(item as UIElement);
			

			foreach(var item in DisposableItems.OfType<NewAsteroid>())
				LootCanvas.Children.Remove(item as UIElement);

			foreach(var item in DisposableCollisionItems.OfType<Enemy>())
				LootCanvas.Children.Remove(item as UIElement);

			foreach(var item in DisposableItems.OfType<BonusItem>())
				LootCanvas.Children.Remove(item as UIElement);


			// clean explosions

			foreach(var item in ExplosionCanvas.Children.OfType<Explosion>())
				if(!item.isActive)
					DisposableItems.Add(item as IDestructibleItem);

			foreach(var item in DisposableItems.OfType<Explosion>())
				ExplosionCanvas.Children.Remove(item as UIElement);

			// end clean explosions



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
			foreach(Rocket rocket in ShotCanvas.Children)
			{
				if(rocket.CoordLeft > Width || rocket.CoordLeft < - MaxObjectSize)
					DisposableItems.Add(rocket);

				foreach(NewAsteroid asteroid in LootCanvas.Children.OfType<NewAsteroid>())
				{
					if(rocket.IsCollision(asteroid))
					{
						DisposableItems.Add(asteroid);

						CurrentPlayer.Score.NewKill();
						rocket.SmallBang();
						ExplosionCanvas.Children.Add(new Explosion(rocket, r.Next() % 10 + 1));

						DisposableItems.Add(rocket);
					}
				}

				foreach(Enemy enemyWithCollision in LootCanvas.Children.OfType<Enemy>())
				{
					if(!enemyWithCollision.IsInvincible)
					{
						if(rocket.IsCollision(enemyWithCollision))
						{
							DisposableCollisionItems.Add(enemyWithCollision);

							CurrentPlayer.Score.NewKill();
							rocket.SmallBang();
							ExplosionCanvas.Children.Add(new Explosion(rocket, r.Next() % 10 + 1));

							DisposableItems.Add(enemyWithCollision);
							DisposableItems.Add(rocket);
						}
					}
				}



				foreach(Enemy enemy in EnemyCanvas.Children)
				{
					if(enemy.CoordBottom - 32 <= rocket.CoordBottom && enemy.CoordBottom + 32 >= rocket.CoordBottom)
					{
						if(enemy.CoordLeft-64<= rocket.CoordLeft && enemy.CoordLeft <= rocket.CoordLeft+32)
						{
							if(enemy.MovementIterator > 8)
								enemy.RenewMovement();

							enemy.ChooseDirectionToRun();

							if(enemy.Direction == MoveDirection.Up)
							{
								//go up 

								int f = (int)(enemy.CoordBottom + 3); 
								//(int)(enemy.CoordBottom + 8 * Math.Exp(-((enemy.MovementIterator += 0.5)) * 0.03));

								if(f < Height - PlayerShip.ActualHeight / 2 - 32)
									enemy.CoordBottom = f;
							}
							else if(enemy.Direction == MoveDirection.Down)
							{
								// go down 

								int f = (int)(enemy.CoordBottom - 4);
								//(int)(enemy.CoordBottom - 2 * Math.Exp(-((enemy.MovementIterator -= 0.5)) * 0.02));

								if(f > 0)
									enemy.CoordBottom = f;
							}
							// else do nothing
						}
					}

					if(rocket.IsCollision(enemy))
					{
						enemy.WasHit(rocket.CurrentDamage);

						if(!enemy.IsAlive())
						{
							LootCanvas.Children.Add(new Enemy(enemy, true));

							DisposableItems.Add(enemy);
							enemy.StartShipExplosion(ExplosionCanvas);

							rocket.Bang();

							CurrentPlayer.Score.NewKill();
						}
						else
						{
							rocket.SmallBang();
							ExplosionCanvas.Children.Add(new Explosion(rocket, r.Next() % 10 + 1));

							DisposableItems.Add(rocket);
							CurrentPlayer.Score.NewHit();
						}
					}
				}

				if(rocket.CurrentDirection == Rocket.RocketDirection.Right && rocket.IsCollision(PlayerShip))
				{
					if(!PlayerShip.IsInvincible)
					{
						PlayerShip.WasHit(rocket.CurrentDamage);
						PlayerDamageIndicator.Opacity += rocket.CurrentDamage * .1;

					}

					//for fun
					CurrentPlayer.Score.Multiplier = CurrentPlayer.Score.Multiplier / 2;

                    rocket.SmallBang();
					ExplosionCanvas.Children.Add(new Explosion(rocket, r.Next() % 10 + 1));

					DisposableItems.Add(rocket);
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
			PlayerShip.GenerateType();

			if(isAutoHeal)
				if(countIII++ % 10 == 0)
					PlayerShip.Heal(0.3);

			if(!PlayerShip.IsAlive())
			{
				ExplosionCanvas.Children.Add(new Explosion(PlayerShip, 7));
				GameOverWithPlayerExplosion();
			}

			if(countIII++ % 100 == 0)
				CurrentPlayer.Score.Multiplier -= CurrentPlayer.Score.MultiplierStep;

			CurrentPlayer.Score.MultiplierStringUpdate();


			if(PlayerDamageIndicator.Opacity>0.001)
				PlayerDamageIndicator.Opacity -= 0.1;


			if(PlayerBonusIndicator.Opacity > 0.001)
				PlayerBonusIndicator.Opacity -= 0.1;




			#region Startup flicker

			//if(isStartupFlicker)
			//{
			//	if(Counter > 30 || (Counter < 30 && Counter % 5 < 3))
			//		PlayerCanvas.Children.Add(CurrentPlayer);       // fix need to be removed
			//}

			#endregion

		}

		/// <summary>
		/// Updates movement of player
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public void UserMovement_Tick(object sender, EventArgs e)
		{
			if(isUserInputAvailable)
			{
				if(isMovingUpward && !isMovingDownward)
				{
					int f = (int)(PlayerShip.CoordBottom + 6);//(int)(PlayerShip.CoordBottom + 8* Math.Exp(-((UpwardIterator += 0.5)) * 0.6));

					if(f < Height - PlayerShip.ActualHeight / 2 - 32 + PlayerShip.CoordTopModifier)
						PlayerShip.CoordBottom = f;
				}

				if(isMovingDownward && !isMovingUpward)
				{
					int f = (int)(PlayerShip.CoordBottom - 9);//(int)(PlayerShip.CoordBottom - 4* Math.Exp(-((DownwardIterator -= 0.5)) * 0.1));

					if(f > 0 + PlayerShip.CoordBottomModifier)
						PlayerShip.CoordBottom = f;
				}

				if(isMovingForward && !PlayerShip.IsSpeedMaximum())
				{
					if(isMovingBackward)
						isMovingBackward = false;

					int f = (int)(PlayerShip.CoordLeft + PlayerShip.MaximumSpeed -
						PlayerShip.MaximumSpeed * Math.Exp(-((ForwardIterator += 0.5)) * PlayerShip.ForwardSpeedModifier));

					if(f < PlayerShip.MaximumSpeed)
						PlayerShip.CoordLeft = f;
				}

				if(isMovingBackwardFloat && !isMovingForward)
				{
					if(PlayerShip.IsSpeedMinimum())
					{
						PlayerShip.CoordLeft = PlayerShip.MinimumSpeed;
						isMovingBackward = false;
						isMovingBackwardFloat = false;
					}
					else
					{
						int v = (int)(PlayerShip.CoordLeft * Math.Exp(-(BackwardIterator += 0.5) * PlayerShip.BackwardSpeedModifier));
						PlayerShip.CoordLeft = v;
					}

				}

				if(isMovingBackward && !isMovingForward)
				{
					if(PlayerShip.IsSpeedMinimum())
					{
						PlayerShip.CoordLeft = PlayerShip.MinimumSpeed;
						isMovingBackward = false;
						isMovingBackwardFloat = false;
					}
					else
					{
						int v = (int)(PlayerShip.CoordLeft * Math.Exp(-(BackwardIterator += 0.5) * PlayerShip.BackwardSpeedModifier * 2));
						PlayerShip.CoordLeft = v;
					}

					if(Keyboard.IsKeyDown(Key.Space))
						PlayerShip.MakeAShot();
				}
			}
		}


		#endregion





		#region User input event handlers

		/// <summary>
		/// When key is down
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public void KeyDown(Key keyDown)
		{
            if (Keyboard.IsKeyDown(Key.Space))
				PlayerShip.MakeAShot();

			if(Keyboard.IsKeyDown(Key.Right) || Keyboard.IsKeyDown(Key.D))
				isMovingForward = true;


			if(Keyboard.IsKeyDown(Key.Left) || Keyboard.IsKeyDown(Key.A))
				isMovingBackward = true;


			if(Keyboard.IsKeyDown(Key.Up) || Keyboard.IsKeyDown(Key.W))
			{
				isMovingUpward = true;
				UpwardIterator = 0;
			}

			if(Keyboard.IsKeyDown(Key.Down) || Keyboard.IsKeyDown(Key.S))
			{
				isMovingDownward = true;
				DownwardIterator = 0;
			}

			StartNewGame();
		}

		/// <summary>
		/// When key is up
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public void KeyUp(Key keyUp)
		{
			if(Keyboard.IsKeyUp(Key.Left) || Keyboard.IsKeyDown(Key.A))
			{
				isMovingBackward = false;
				BackwardIterator = 0;
			}

			if(Keyboard.IsKeyUp(Key.Right) || Keyboard.IsKeyDown(Key.D))
			{
				isMovingForward = false;
				isMovingBackwardFloat = true;
				BackwardIterator = 0;
				ForwardIterator = 0;
			}

			if(Keyboard.IsKeyUp(Key.Up) || Keyboard.IsKeyDown(Key.W))
				isMovingUpward = false;

			if(Keyboard.IsKeyUp(Key.Down) || Keyboard.IsKeyDown(Key.S))
				isMovingDownward = false;
        }

		/// <summary>
		/// When mouse click
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Window_MouseDown(object sender, MouseButtonEventArgs e)
		{
			//TryStartNewGame();
		}

		#endregion





		/// <summary>
		/// Starts new game if isNewGame is true
		/// </summary>
		public void StartNewGame()
		{
			if(isNewGame)
			{
				ResetAll();
				InitializeCanvases();
				
				GameplayTimer.Start();
				
				isNewGame = false;

				PlayerShip.Visibility = Visibility.Visible;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public void ContinueGame()
		{
			//TODO
		}






		#endregion

		//public void Reload()
		//{
		//	isAutoHeal = true;

		//	PlayerCanvas.Children.Clear();
		//	EnemyCanvas.Children.Clear();
		//	ExplosionCanvas.Children.Clear();
		//	ShotCanvas.Children.Clear();
		//	LootCanvas.Children.Clear();


		//}




	}

}
