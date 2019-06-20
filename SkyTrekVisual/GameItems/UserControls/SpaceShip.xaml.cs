using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.ComponentModel;
using SkyTrekVisual.Controls;
using System.Windows.Threading;
using System.Diagnostics;

namespace SkyTrekVisual.GameItems
{
	/// <summary>
	/// 
	/// </summary>
	public enum ShipType
	{
		Ship1,
		Ship2,
		Ship3,
		Ship4,
		Ship5,
		Ship6
	}



	/// <summary>
	/// Interaction logic for SpaceShip.xaml
	/// </summary>
	public partial class SpaceShip : UserControl, IGameItem, ISpaceShip, IDestructibleItem, INotifyPropertyChanged, IDamagable
	{





		public SpaceShip()
		{
			InitializeComponent();

			DataContext = this;



			CoordLeft = Ship_DefaultLeftPosition;
			CoordBottom = Ship_DefaultBottomPosition;

			//CurrentShipType = (ShipType)1;

			//CurrentGun = new Gun(0.6, 10);

			//GenerateType();





			ShieldLifeTimer = new DispatcherTimer() { Interval = TimeSpan.FromSeconds(0.01) };
			ShieldLifeTimer.Tick += ShieldLifeTimer_Tick;

		

		





		}





		#region shield

		

		public double ShieldReloadTime { set; get; } = 0;




		public string ShieldReloadTimeString
		{
			get { return ShieldReloadTime.ToString(); }
			set { OnPropertyChanged(""); }
		}



		DispatcherTimer ShieldLifeTimer;


		public void EnableShield()
		{
			GC.Collect();

			IsInvincible = true;
			ShieldReloadTime = 100;
			ShieldLifeTimer.Start();
			ShieldReloadTimeString = "";
		}

		private void ShieldLifeTimer_Tick(object sender, EventArgs e)
		{
			ShieldReloadTimeString = "";
			if((ShieldReloadTime-=.1) < 0)
			{
				ShieldReloadTime = 0;
				ShieldLifeTimer.Stop();
				IsInvincible = false;
			}
		}

		#endregion












		public SpaceShip(int type, double gunReload, int damage) : this()
		{
			CurrentGun = new Gun(gunReload, damage + 50);


			CurrentShipType = (ShipType)type;

			GenerateType();
		}





		public int CoordBottomModifier = 0;
		public int CoordTopModifier = 0;



		public static double ShipScale { get; set; } = 0.3;



		#region IDamagable

		public double HealthPoints { get; set; } = 100;

		public bool IsAlive() => HealthPoints > 0;

		public int HitDamage { get; set; } = 60;

		//public void WasHit(int hitDamage) => HealthPoints -= hitDamage;
		public void WasHit(double hitDamage)
		{
			//Debug.WriteLine("Player was hit: " + hitDamage.ToString());

			if(!IsInvincible)
			{
				HealthPoints -= hitDamage;


				OnHealthChange.Invoke(this, null);
			}
		}

		public bool IsInvincible { get; set; } = false;







		public void Heal(double howMuch)
		{
			if((HealthPoints += howMuch) > 100)
				HealthPoints = 100;

			OnHealthChange.Invoke(this, null);
		}


		#endregion


		public Gun CurrentGun;







		private void UpdateShipView()
		{
			int images = 4; // 4
							//int images = 1; // 4
			for(int i = 0; i < images; i++)
			{
				ShipStateBrushes.Add(LoadImage((int)CurrentShipType + 1, i));
				//ShipStateBrushes.Add(LoadImage((int)CurrentShipType + 1));
			}

			SizeSwitcher();     // sets ship size
		}




		public event EventHandler OnHealthChange;








		private void SizeSwitcher()
		{
			switch(CurrentShipType)
			{
				case ShipType.Ship1:
					Height = 150 * ShipScale;
					Width = 344 * ShipScale;
					CoordTopModifier = 10;
					break;

				case ShipType.Ship2:
					Height = 271 * ShipScale;
					Width = 236 * ShipScale;
					CoordTopModifier = -15;
					CoordBottomModifier = 4;
					break;

				case ShipType.Ship3:
					Height = 230 * ShipScale;       //was 301
					Width = 344 * ShipScale;
					CoordTopModifier = -4;
					CoordBottomModifier = 0;
					break;

				case ShipType.Ship4:
					Height = 59.22;
					Width = 110;
					CoordTopModifier = 5;
					break;

				case ShipType.Ship5:

					Height = 342 * ShipScale;
					Width = 194 * ShipScale;
					CoordTopModifier = -15;
					break;

				case ShipType.Ship6:
					Height = 252 * ShipScale;
					Width = 263 * ShipScale;
					CoordTopModifier = -4;
					CoordBottomModifier = 1;
					break;

				default:
					break;
			}




		}



		public List<ImageBrush> ShipStateBrushes = new List<ImageBrush>();


		#region IDestructibleItem

		public int ItemWidth { get { return (int)ActualWidth; } }

		public int ItemHeight { get { return (int)ActualHeight; } }

		public bool IsCollision(IDestructibleItem item) => CollisionDetector.IsCollision(this, item);

		#endregion




		#region Default position



		/// <summary>
		/// BACKUP Start position in Canvas - horizontal						-- TODO - fix it
		/// </summary>
		public static readonly int Ship_DefaultLeftPosition = 100;         //150;

		/// <summary>
		/// BACKUP Start position in Canvas - vertical		//default 200		-- TODO - fix it	
		/// </summary>
		public static readonly int Ship_DefaultBottomPosition = 100;      //100;

		#endregion




		#region INotifyPropertyChanged implementation


		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		#endregion



		#region ISpaceShip




		/// <summary>
		/// 
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public bool IsShipCollision(IDestructibleItem item) => CollisionDetector.IsShipCollision(this, item);



		private ShipType _CurrentShipType;

		public ShipType CurrentShipType
		{
			get { return _CurrentShipType; }
			set
			{
				_CurrentShipType = value;
				UpdateShipView();

			}
		}








		public void Fill(SolidColorBrush brush)
		{
			ItemGrid.Background = brush;

		}





		public double BackwardSpeedModifier { get; set; } = 0.0001;
		public double ForwardSpeedModifier { get; set; } = 0.004;






		public int MinimumSpeed { get; } = Ship_DefaultLeftPosition;
		public int MaximumSpeed { get; set; } = Ship_DefaultLeftPosition + 600;




		public bool IsSpeedMaximum() => CoordLeft >= MaximumSpeed;

		public bool IsSpeedMinimum() => CoordLeft <= MinimumSpeed;








		public void MakeAShot()
		{
			CurrentGun.MakeAShot(this);



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



		public ImageBrush LoadImage(int ship, int state) => new ImageBrush(new BitmapImage(
			new Uri(DirectoryHelper.CurrentDirectory + @"\Ships\Ship" + ship.ToString() + @"\Ship" + ship.ToString() + "_state" + state.ToString() + ".png", UriKind.Relative)))
		{ Stretch = Stretch.Uniform };

		public ImageBrush LoadImage(int t) => new ImageBrush(new BitmapImage(new Uri(DirectoryHelper.CurrentDirectory + @"\Ships\Ship" + t.ToString() + ".png", UriKind.Relative))) { Stretch = Stretch.Uniform };





		public void GenerateType()
		{
			var t = new Random().Next() % ShipStateBrushes.Count;

			ItemGrid.Background = ShipStateBrushes[t];

		}

		public void GenerateSize()
		{
			ItemGrid.Height = ItemGrid.Width = new Random().Next(32, 64);
		}


		public double Speed { get; set; }

		public void GoBackward()
		{
			throw new NotImplementedException();
		}














		public void MakeAShot(Canvas canvas)
		{
			throw new NotImplementedException();
		}

		public void StartShipExplosion(Canvas explosionCanvas)
		{
			var rnd = new Random();
			for(int i = 0; i < 10; i++)
			{

				var e = new Explosion();

				e.CoordBottom = CoordBottom + rnd.Next(0, 48) - 14;
				e.CoordLeft = CoordLeft + rnd.Next(0, 48);

				e.isActive = true;
				e.AminationType = rnd.Next(1, 10);

				explosionCanvas.Children.Add(e);
			}
		}










		#endregion














	}
}
