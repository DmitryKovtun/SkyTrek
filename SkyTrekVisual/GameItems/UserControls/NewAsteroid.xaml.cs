using System;
using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using SkyTrekVisual.Controls;

namespace SkyTrekVisual.GameItems
{
	/// <summary>
	/// Interaction logic for Asteriod.xaml
	/// </summary>
	public partial class NewAsteroid : UserControl, IGameItem, IDestructibleItem, IDamagable
	{

		public static int MaxSize = 32;
		public static int MinSize = 5;




		public NewAsteroid()
		{

			InitializeComponent();

			GenerateType();
			GenerateSize();

		}

		public NewAsteroid(int x, int y) : this()
		{
			CoordLeft = x;
			CoordBottom = y;

			SetValue(Panel.ZIndexProperty, 1000);

			Speed = ItemGrid.Height/4;
			HitDamage =  (int)ItemGrid.Height*4 / HitDamage;

		}


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


		public ImageBrush LoadImage(int t) => new ImageBrush(new BitmapImage(new Uri(DirectoryHelper.CurrentDirectory + @"\Asteroids\Asteroid" + t.ToString() + ".png", UriKind.Relative)));

		public void GenerateType()
		{
			ItemGrid.Background = LoadImage(new Random().Next() % 4 + 1);
		}


		public void GenerateSize()
		{
			ItemGrid.Height = ItemGrid.Width = new Random().Next(MinSize, MaxSize);

		}









		#region IDestructibleItem

		public int ItemWidth { get { return (int)ActualWidth; } }

		public int ItemHeight { get { return (int)ActualHeight; } }

		public bool IsCollision(IDestructibleItem item) => CollisionDetector.IsCollision(this, item);


		#endregion



		public double Speed { get; set; } = 3;

		public void GoBackward()
		{
			CoordLeft -= Speed;
		}





		#region IDamagable


		public double HealthPoints { get; set; } = 20;

		public int HitDamage { get; set; } = 10;

		public bool IsAlive() => HealthPoints > 0;

		public void WasHit(double hitDamage)
		{
			HealthPoints -= hitDamage;
		}

		public bool IsInvincible { get; set; } = false;




		#endregion




	}
}
