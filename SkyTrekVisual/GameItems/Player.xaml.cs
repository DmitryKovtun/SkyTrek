using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using SkyTrekVisual.Controls;



namespace SkyTrekVisual.GameItems
{




	/// <summary>
	/// Interaction logic for Player.xaml
	/// </summary>
	[Serializable]
	public partial class Player : UserControl, IGameItem, ISpaceShip, IDestructibleItem, INotifyPropertyChanged, IDamagable
	{



		public string UserName { get; set; }




		public enum ShipType
		{
			Ship1,
			Ship2,
			Ship3,
			Ship4,
			Ship5,
			Ship6
		}


		public static double ShipScale { get; set; } = 0.5;



		#region IDamagable

		public double HealthPoints { get; set; } = 100;

		public bool IsAlive() => HealthPoints > 0;

		public int HitDamage { get; set; } = 60;

		//public void WasHit(int hitDamage) => HealthPoints -= hitDamage;
		public void WasHit(double hitDamage)
		{
			Debug.WriteLine("Player was hit: " + hitDamage.ToString());


			HealthPoints -= hitDamage;


			OnPlayerHealthChange.Invoke(this,null);

		}

		public void Heal(double howMuch)
		{
			if((HealthPoints+=howMuch) >100)
				HealthPoints = 100;

			OnPlayerHealthChange.Invoke(this, null);
		}


		#endregion


		public Gun CurrentGun;




		public void Reset()
		{
			HealthPoints = 100;
			Score.Clear();

		}



		private void UpdateShipView()
		{
			int images = 4; // 4
			//int images = 1; // 4
			for(int i = 0; i < images; i++)
			{
				ShipStateBrushes.Add(LoadImage((int)CurrentShipType + 1 , i));
				//ShipStateBrushes.Add(LoadImage((int)CurrentShipType + 1));
			}

			SizeSwitcher();     // sets ship size
		}




		public event EventHandler OnPlayerHealthChange;




		public Player()
		{
			InitializeComponent();

			DataContext = this;

			

			CoordLeft = Player_DefaultLeftPosition;
			CoordBottom = Player_DefaultBottomPosition;

			CurrentShipType = (ShipType)0;

			CurrentGun = new Gun(0.4);

			GenerateType();
		}




		private void SizeSwitcher()
		{
			switch(CurrentShipType)
			{
				case ShipType.Ship1:
					Height = 59.22;
					Width = 110;
					break;

				case ShipType.Ship2:
					Height = 271 * ShipScale;
					Width = 236 * ShipScale;
					break;

				case ShipType.Ship3:
					Height = 342 * ShipScale;
					Width = 194 * ShipScale;
					break;

				case ShipType.Ship4:
					Height = 150 * ShipScale;
					Width = 344 * ShipScale;
					break;

				case ShipType.Ship5:
					Height = 252 * ShipScale;
					Width = 263 * ShipScale;
					break;

				case ShipType.Ship6:
					Height = 301 * ShipScale;
					Width = 344 * ShipScale;
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




		/// <summary>
		/// BACKUP Start position in Canvas - horizontal						-- TODO - fix it
		/// </summary>
		public static readonly int Player_DefaultLeftPosition = 50;			//150;

		/// <summary>
		/// BACKUP Start position in Canvas - vertical		//default 200		-- TODO - fix it	
		/// </summary>
		public static readonly int Player_DefaultBottomPosition = 100;		//100;






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





		public double BackwardSpeedModifier { get; set; } = 0.00008;
		public double ForwardSpeedModifier { get; set; } = 0.001;






		public int MinimumSpeed { get; } = Player_DefaultLeftPosition;
		public int MaximumSpeed { get; set; } = Player_DefaultLeftPosition + 600;




		public bool IsSpeedMaximum() => CoordLeft >= MaximumSpeed;

		public bool IsSpeedMinimum() => CoordLeft <= MinimumSpeed;








        public void MakeAShot()
		{
			CurrentGun.MakeAShot(this);

			

		}



		#endregion





		public GameScore Score { get; set; } = new GameScore();





		
		






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

		public void WasHit(object currentDamage)
		{
			throw new NotImplementedException();
		}






		#endregion








	}
}
