using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SkyTrekVisual.Controls;
using static SkyTrekVisual.GameItems.Player;

namespace SkyTrekVisual.GameItems
{


	public enum MoveDirection { None, Up, Down }



	/// <summary>
	/// Interaction logic for Enemy.xaml
	/// </summary>
	public partial class Enemy : UserControl, IGameItem, ISpaceShip, IDestructibleItem, IDamagable
	{

		public Gun CurrentGun = new Gun(1, 20);

		public Enemy()
		{
			InitializeComponent();



		}


		public Enemy(int x, int y) : this()
		{
			CoordBottom = y;
			CoordLeft = x;

			GenerateType();

			CurrentGun = new Gun(0.8,20);

		}






		#region direction to run




		public MoveDirection Direction { get; set; } = MoveDirection.None;



		public void ChooseDirectionToRun()
		{
			if(Direction == MoveDirection.None)
			{
				Direction = (MoveDirection)(new Random().Next(0,3));
			}
		}


		public double MovementIterator { get; set; } = 0;


		public void RenewMovement()
		{
			MovementIterator = 0;
			Direction = MoveDirection.None;

		}
		



		#endregion












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










		public ImageBrush LoadImage(int t) => new ImageBrush(new BitmapImage(new Uri(DirectoryHelper.CurrentDirectory + @"\Enemies\enemyBlack" + t.ToString() + ".png", UriKind.Relative))) { Stretch = Stretch.Uniform };



		public void GenerateType()
		{
			ItemGrid.Background = LoadImage(new Random().Next()%5+1);


		}

		public void GenerateSize()
		{
			throw new NotImplementedException();
		}

		#endregion




		public void MakeAShot()
		{
			CurrentGun.MakeAShotRight(this);



		}




		#region ISpaceShip


		/// <summary>
		/// 
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public bool IsShipCollision(IDestructibleItem item) => CollisionDetector.IsShipCollision(this, item);






		/// <summary>
		/// 
		/// </summary>
		/// <param name="explosionCanvas"></param>
		public void StartShipExplosion(Canvas explosionCanvas)
		{
			var rnd = new Random();
			for(int i = 0; i < 3; i++)
			{
				var e = new Explosion();

				e.CoordBottom = CoordBottom + rnd.Next(0, 32) - 14;
				e.CoordLeft = CoordLeft - rnd.Next(0, 40);

				e.isActive = true;
				e.AminationType = rnd.Next(1, 10);

				explosionCanvas.Children.Add(e);
			}
		}


		public ShipType CurrentShipType { get; set; }

		public int ShipSize { get; set; } = 32;

		public void Fill(SolidColorBrush brush)
		{
			throw new NotImplementedException();
		}

		public double BackwardSpeedModifier { get; set; }
		public double ForwardSpeedModifier { get; set; }
		public int MinimumSpeed { get; }
		public int MaximumSpeed { get; }
		

		public bool IsSpeedMaximum()
		{
			throw new NotImplementedException();
		}

		public bool IsSpeedMinimum()
		{
			throw new NotImplementedException();
		}

		public void MakeAShot(Canvas canvas)
		{
			throw new NotImplementedException();
		}





		#endregion



		#region IDamagable


		public double HealthPoints { get; set; } = 100;

		public int HitDamage { get; set; } = 20;

		public bool IsAlive() => HealthPoints > 0;

		public void WasHit(double hitDamage)
		{
			Debug.WriteLine("enemy was hit: " + hitDamage.ToString());


			HealthPoints -= hitDamage;
			var t = HealthPoints * 46 / 100;
			HealthIndicator.Width =  t > 0 ? t : 0;
			
			if(t>23 & t<80)
				HealthIndicator.Background = new BrushConverter().ConvertFromString("#F9AA33") as SolidColorBrush;
			else
				HealthIndicator.Background = new BrushConverter().ConvertFromString("#df4e56") as SolidColorBrush;


		}

	



		#endregion


	}

}
