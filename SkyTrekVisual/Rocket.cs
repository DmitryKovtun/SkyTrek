using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace SkyTrekVisual
{
    public class Rocket : Control
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

        public static readonly DependencyProperty SpriteCenterProperty =
            DependencyProperty.Register("SpriteCenter", typeof(Point), typeof(Rocket), new PropertyMetadata(SpriteCenterChanged));

        private static void SpriteCenterChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as Rocket).SpriteCenter = (Point)e.NewValue;
        }

        public Point SpriteCenter
        {
            get { return (Point)GetValue(SpriteCenterProperty); }
            set { SetValue(SpriteCenterProperty, value); }
        }

        public static readonly DependencyProperty CurrentPositionProperty = DependencyProperty.Register("CurrentPosition", typeof(Point), typeof(Rocket), new PropertyMetadata(CurrentPositionChanged));

        public Point CurrentPosition
        {
            get { return (Point)GetValue(CurrentPositionProperty); }
            set { SetValue(CurrentPositionProperty, value); }
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
            SpriteCenter = new Point(Sprite.Width / 2, Sprite.Height / 2);
            CurrentPosition = new Point(0.0, 0.0);


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

        private void SpriteTimer_Tick(object sender, EventArgs e)
        {
            Sprite = TextureManager.Rocket_sprites[currentSpriteCount++];

            if (currentSpriteCount == TextureManager.Rocket_sprites.Length)
                currentSpriteCount = 0;
        }


        private void FlyingTimer_Tick(object sender, EventArgs e)
        {
            CurrentPosition = new Point(CurrentPosition.X+2, CurrentPosition.Y);
        }


        private void ExplosionTimer_Tick(object sender, EventArgs e)
        {
            Sprite = TextureManager.Rocket_explosion[currentExplosionCount++];
            SpriteCenter = new Point(Sprite.Width / 2, Sprite.Height / 2);

            if (currentExplosionCount == TextureManager.Rocket_explosion.Length)
            {
                currentExplosionCount = 0;
                explosionTimer.Stop();

                currentLayout.Children.Remove(this);
            }
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

            explosionTimer.Start();
        }

        public void Fly()
        {
            if (!flyingTimer.IsEnabled)
                flyingTimer.Start();
        }

    }
}
