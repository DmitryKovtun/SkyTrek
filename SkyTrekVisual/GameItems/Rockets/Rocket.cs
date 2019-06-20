using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using SkyTrekVisual.GameItems.Helpers;

namespace SkyTrekVisual.GameItems.Rockets
{
	public class Rocket : Control, IGameItem, IDestructibleItem, INotifyPropertyChanged, IDestructive
	{
		//RotateTransform rotateTransform = null;

		private Canvas currentLayout;

		private int currentSpriteCount = 0;
		private int currentExplosionCount = 0;

		public double currentSpeed = 0.5;

		public DispatcherTimer spriteTimer = new DispatcherTimer();
		public DispatcherTimer flyingTimer = new DispatcherTimer();
		public DispatcherTimer explosionTimer = new DispatcherTimer();


		public void Pause()
		{
			spriteTimer.Stop();
			flyingTimer.Stop();
			//explosionTimer.Stop();
		}

		public void Resume()
		{
			//spriteTimer.Start();
			flyingTimer.Start();

		}




		public static readonly DependencyProperty SpriteProperty =
			DependencyProperty.Register("Sprite", typeof(BitmapSource), typeof(Rocket));

		public BitmapSource Sprite
		{
			get { return (BitmapSource)GetValue(SpriteProperty); }
			set { SetValue(SpriteProperty, value); }
		}

		public static readonly DependencyProperty SpriteAngleProperty =
			DependencyProperty.Register("SpriteAngle", typeof(double), typeof(Rocket), new PropertyMetadata(SpriteAngleChanged));

		public double SpriteAngle
		{
			get { return (double)GetValue(SpriteAngleProperty); }
			set { SetValue(SpriteAngleProperty, value); }
		}

		private static void SpriteAngleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			//(d as Rocket).rotateTransform.Angle = (double)e.NewValue;
		}







		public static readonly DependencyProperty CenterXProperty = DependencyProperty.Register("CenterX", typeof(double), typeof(Rocket));

		public double CenterX
		{
			get { return (double)GetValue(CenterXProperty); }
			set
			{
				SetValue(CenterXProperty, value);
				SetValue(Canvas.LeftProperty, value);
			}
		}




		public static readonly DependencyProperty CenterYProperty = DependencyProperty.Register("CenterY", typeof(double), typeof(Rocket));

		public double CenterY
		{
			get { return (double)GetValue(CenterYProperty); }
			set
			{
				SetValue(CenterYProperty, value);
				SetValue(Canvas.BottomProperty, value);
			}
		}





		public enum RocketDirection
		{
			Left, Right
		}



		#region type of rocket

		private Visibility _PlayerRocketVisibility;

		public Visibility PlayerRocketVisibility
		{
			get { return _PlayerRocketVisibility; }
			set { _PlayerRocketVisibility = value; OnPropertyChanged("PlayerRocketVisibility"); }
		}


		private Visibility _PlayerRocket1Visibility;

		public Visibility PlayerRocket1Visibility
		{
			get { return _PlayerRocket1Visibility; }
			set { _PlayerRocket1Visibility = value; OnPropertyChanged("PlayerRocket1Visibility"); }
		}


		private Visibility _PlayerRocket2Visibility;

		public Visibility PlayerRocket2Visibility
		{
			get { return _PlayerRocket2Visibility; }
			set { _PlayerRocket2Visibility = value; OnPropertyChanged("PlayerRocket2Visibility"); }
		}


		private Visibility _PlayerRocket3Visibility;

		public Visibility PlayerRocket3Visibility
		{
			get { return _PlayerRocket3Visibility; }
			set { _PlayerRocket3Visibility = value; OnPropertyChanged("PlayerRocket3Visibility"); }
		}



		private Visibility _PlayerRocket4Visibility;

		public Visibility PlayerRocket4Visibility
		{
			get { return _PlayerRocket4Visibility; }
			set { _PlayerRocket4Visibility = value; OnPropertyChanged("PlayerRocket4Visibility"); }
		}







		private Visibility _EnemyRocketVisibility;

		public Visibility EnemyRocketVisibility
		{
			get { return _EnemyRocketVisibility; }
			set { _EnemyRocketVisibility = value; OnPropertyChanged("EnemyRocketVisibility"); }
		}

		#endregion



		public RocketDirection CurrentDirection = RocketDirection.Left;










		static int COUNT = 0;


		~Rocket()
		{
			Debug.WriteLine("Rocket: " + --COUNT);
		}






		public Rocket()
		{
			Debug.WriteLine("Rocket: " + ++COUNT);


			DefaultStyleKey = typeof(Rocket);

			DataContext = this;

			//Initialization
			SpriteAngle = 0.0;
			// Sprite = TextureManager.Rocket_sprites[currentSpriteCount];



			ChooseStyle(3);

			//Timers

			//Sprite
			spriteTimer.Tick += SpriteTimer_Tick;
			spriteTimer.Interval = TimeSpan.FromSeconds(1.0 / TextureManager.Rocket_sprites.Length);

			spriteTimer.Start();

			//Flying
			flyingTimer.Tick += FlyingTimerLeft_Tick;
			flyingTimer.Interval = TimeSpan.FromMilliseconds(currentSpeed);

			//Explosion
			explosionTimer.Tick += ExplosionTimer_Tick;
			explosionTimer.Interval = TimeSpan.FromSeconds(1.0 / TextureManager.Rocket_explosion.Length);

		}



		public Rocket(Canvas canvas) : this()
		{
			currentLayout = canvas;

		}



		public Rocket(Canvas canvas, double x, double y, double damage) : this(canvas)
		{
			CoordLeft = x;
			CoordBottom = y;

			Damage = damage;

			Fly();
		}


		public Rocket(Canvas canvas, double x, double y, RocketDirection dir) : this(canvas)
		{
			CoordLeft = x;
			CoordBottom = y;

			if((CurrentDirection = dir) == RocketDirection.Right)
			{
				flyingTimer.Tick -= FlyingTimerLeft_Tick;
				flyingTimer.Tick += FlyingTimerRight_Tick;

				PlayerRocketVisibility = Visibility.Hidden;
				EnemyRocketVisibility = Visibility.Visible;

				ChooseStyle(5);
			}

			Fly();
		}




		private void SpriteTimer_Tick(object sender, EventArgs e)
		{
			return;

			Sprite = TextureManager.Rocket_sprites[currentSpriteCount++];

			if(currentSpriteCount == TextureManager.Rocket_sprites.Length)
				currentSpriteCount = 0;
		}


		public int Speed { set; get; } = 3;


		void SelfDestruction()
		{
			//self destruction
			Opacity -= .004;

			if(Opacity <= 0.01)
			{


				Opacity = 1;
				SmallBang();
				currentLayout.Children.Remove(this);
			}

		}

		private void FlyingTimerLeft_Tick(object sender, EventArgs e)
		{
			CoordLeft += Speed;
			SelfDestruction();
		}

		private void FlyingTimerRight_Tick(object sender, EventArgs e)
		{
			CoordLeft -= Speed;
			SelfDestruction();
		}


		private void ExplosionTimer_Tick(object sender, EventArgs e)
		{
			Sprite = TextureManager.Rocket_explosion[currentExplosionCount++];

			if(currentExplosionCount == TextureManager.Rocket_explosion.Length)
			{
				currentExplosionCount = 0;
				explosionTimer.Stop();

				currentLayout.Children.Remove(this);
			}
		}

		public void Bang()
		{
			Destruction();
		}

		public void SmallBang()
		{
			explosionTimer.Stop();
			flyingTimer.Stop();


		}

		public override void OnApplyTemplate()
		{
			//ДАТЬ ИМЯ <RotateTransform>

			//rotateTransform = GetTemplateChild("RotateTransform") as RotateTransform;
			//rotateTransform.Angle = SpriteAngle;

			base.OnApplyTemplate();
		}

		public void Destruction()
		{
			spriteTimer.Stop();
			flyingTimer.Stop();


			Sprite = TextureManager.Rocket_explosion[currentExplosionCount++];

			CenterX = CoordLeft;
			CenterY = CoordBottom - Sprite.Height / 3;

			explosionTimer.Start();

			PlayerRocketVisibility = Visibility.Hidden;
			EnemyRocketVisibility = Visibility.Hidden;
		}



		public void ChooseStyle(int i)
		{
			PlayerRocketVisibility = Visibility.Hidden;
			PlayerRocket1Visibility = Visibility.Hidden;
			PlayerRocket2Visibility = Visibility.Hidden;
			PlayerRocket3Visibility = Visibility.Hidden;
			PlayerRocket4Visibility = Visibility.Hidden;
			EnemyRocketVisibility = Visibility.Hidden;

			switch(i)
			{
				case 0:
					PlayerRocketVisibility = Visibility.Visible;

					break;
				case 1:
					PlayerRocket1Visibility = Visibility.Visible;

					break;

				case 2:
					PlayerRocket2Visibility = Visibility.Visible;

					break;

				case 3:
					PlayerRocket3Visibility = Visibility.Visible;

					break;
				case 4:
					PlayerRocket4Visibility = Visibility.Visible;

					break;
				case 5:
					EnemyRocketVisibility = Visibility.Visible;

					break;
			}
		}


		public void HideAllStyles()
		{
			PlayerRocketVisibility = Visibility.Hidden;
			PlayerRocket1Visibility = Visibility.Hidden;
			PlayerRocket2Visibility = Visibility.Hidden;
			PlayerRocket3Visibility = Visibility.Hidden;
			PlayerRocket4Visibility = Visibility.Hidden;
			EnemyRocketVisibility = Visibility.Hidden;
		}


		public void Fly()
		{
			if(!flyingTimer.IsEnabled)
				flyingTimer.Start();
		}



		#region INotifyPropertyChanged implementation

		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		#endregion



		#region IGameItem

		private double _CoordLeft;

		public double CoordLeft
		{
			get { return _CoordLeft; }
			set { SetValue(Canvas.LeftProperty, _CoordLeft = value); }
		}

		private double _CoordBottom;


		public double CoordBottom
		{
			get { return _CoordBottom; }
			set { SetValue(Canvas.BottomProperty, _CoordBottom = value); }
		}


		public ImageBrush LoadImage(int t)
		{
			throw new NotImplementedException();
		}

		public void GenerateType()
		{
			throw new NotImplementedException();
		}

		public void GenerateSize()
		{
			throw new NotImplementedException();
		}


		double IGameItem.Speed { get; set; }

		public void GoBackward()
		{
			throw new NotImplementedException();
		}
		#endregion



		#region IDestructibleItem


		public int ItemWidth { get { return (int)ActualWidth; } }

		public int ItemHeight { get { return (int)ActualHeight; } }


		public bool IsCollision(IDestructibleItem item) => CollisionDetector.IsCollision(this, item);





		#endregion



		#region IDestructive

		public double Damage { get; set; } = 30;


		public double CurrentDamage => Damage * Opacity;


		#endregion





	}
}
