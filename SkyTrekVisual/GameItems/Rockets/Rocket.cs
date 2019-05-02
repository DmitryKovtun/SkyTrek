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
    public class Rocket : Control, IGameItem,IDestructibleItem, INotifyPropertyChanged
    {
        //RotateTransform rotateTransform = null;

        private Canvas currentLayout;

        private int currentSpriteCount = 0;
        private int currentExplosionCount = 0;

        public double currentSpeed = 0.5;
        
        DispatcherTimer spriteTimer = new DispatcherTimer();
        DispatcherTimer flyingTimer = new DispatcherTimer();
        DispatcherTimer explosionTimer = new DispatcherTimer();


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
			set {
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
	












		private static void CurrentPositionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Canvas.SetLeft((d as Rocket), ((Point)e.NewValue).X);
            Canvas.SetTop((d as Rocket), ((Point)e.NewValue).Y);
        }

        public Rocket()
        {
            DefaultStyleKey = typeof(Rocket);


            //Initialization
            SpriteAngle = 0.0;
            Sprite = TextureManager.Rocket_sprites[currentSpriteCount];



            //Timers

            //Sprite
            spriteTimer.Tick += SpriteTimer_Tick;
            spriteTimer.Interval = TimeSpan.FromSeconds(1.0 / TextureManager.Rocket_sprites.Length);

            spriteTimer.Start();

            //Flying
            flyingTimer.Tick += FlyingTimer_Tick;
            flyingTimer.Interval = TimeSpan.FromMilliseconds(currentSpeed);

            //Explosion
            explosionTimer.Tick += ExplosionTimer_Tick;
            explosionTimer.Interval = TimeSpan.FromSeconds(1.0 / TextureManager.Rocket_explosion.Length);

        }


        public Rocket(Canvas canvas) : this()
        {
            currentLayout = canvas;
        }

		public Rocket(Canvas canvas, double x, double y) : this(canvas)
		{
			CoordLeft = x;
			CoordBottom = y;

			Fly();
		}




		private void SpriteTimer_Tick(object sender, EventArgs e)
        {
            Sprite = TextureManager.Rocket_sprites[currentSpriteCount++];

            if (currentSpriteCount == TextureManager.Rocket_sprites.Length)
                currentSpriteCount = 0;
        }


		int Speed = 2;



        private void FlyingTimer_Tick(object sender, EventArgs e)
        {
			CoordLeft += Speed;

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
			CenterY = CoordBottom - Sprite.Height / 2;

			explosionTimer.Start();
		}



        public void Fly()
        {
            if (!flyingTimer.IsEnabled)
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


		#endregion



		#region 


		public int ItemWidth { get { return (int)ActualWidth; } }

		public int ItemHeight { get { return (int)ActualHeight; } }


		public bool IsCollision(IDestructibleItem item) => CollisionDetector.IsCollision(this, item);

	


		#endregion



	}
}
