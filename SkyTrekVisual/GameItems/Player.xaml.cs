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
	public partial class Player : UserControl, IGameItem, INotifyPropertyChanged
	{
		public Player()
		{
			InitializeComponent();

			DataContext = this;

			PlayerSize = 64;


			CurrentSpeed = Player_DefaultXPosition;
			CurrentLift = Player_DefaultYPosition;


			for(int i = 0; i < 4; i++)
			{
				ShipStateBrushes.Add(LoadImage(1, i));
			}

			GenerateType();
		}



		public List<ImageBrush> ShipStateBrushes = new List<ImageBrush>();









		/// <summary>
		/// BACKUP Start position in Canvas - horizontal						-- TODO - fix it
		/// </summary>
		public static readonly int Player_DefaultXPosition = 150;

		/// <summary>
		/// BACKUP Start position in Canvas - vertical		//default 200		-- TODO - fix it	
		/// </summary>
		public static readonly int Player_DefaultYPosition = 200;









		/// <summary>
		/// Start position in Canvas - vertical. Defines how high is player on canvas
		/// </summary>
		public int CurrentLift
		{
			get
			{
				return CoordY;
			}
			set
			{
				CoordY = value;
			}

		}




		/// <summary>
		/// Current speed of a shuttle
		/// </summary>
		public int CurrentSpeed
		{
			get
			{
				return CoordX;
			}
			set
			{
				CoordX = value;
			}
		}




		#region INotifyPropertyChanged implementation

		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		#endregion




	

		private int _PlayerSize;
	
		/// <summary>
		/// Size of a ship
		/// </summary>
		public int PlayerSize
		{
			get { return _PlayerSize; }
			set {
				_PlayerSize = value;
				OnPropertyChanged("PlayerSize");
			}
		}





		public void Fill(SolidColorBrush brush)
		{
			ItemGrid.Background = brush;

		}



		public int CoordX { get; set; }
		public int CoordY { get; set; }



		public ImageBrush LoadImage(int ship,int state) => new ImageBrush(new BitmapImage(
			new Uri(DirectoryHelper.CurrentDirectory + @"\Ships\Ship" + ship.ToString() +@"\Ship" + ship.ToString() + "_state" + state.ToString() + ".png", UriKind.Relative)));

		public ImageBrush LoadImage(int t) => new ImageBrush(new BitmapImage(new Uri(DirectoryHelper.CurrentDirectory + @"\Ships\Ship" + t .ToString() + ".png", UriKind.Relative)));




		public void GenerateType()
		{
			var t = new Random().Next() % ShipStateBrushes.Count;

			ItemGrid.Background = ShipStateBrushes[t];
		}

		public void GenerateSize()
		{
			ItemGrid.Height = ItemGrid.Width = new Random().Next(32, 64);
		}


		public double BackwardSpeedModifier { get; set; } = 0.00008;
		public double ForewardSpeedModifier { get; set; } = 0.001;






		public int MinimumSpeed { get; } = Player_DefaultXPosition;
		public int MaximumSpeed { get; set; } = Player_DefaultXPosition + 600;




		public bool IsSpeedMaximum() => CurrentSpeed >= MaximumSpeed;

		public bool IsSpeedMinimum() => CurrentSpeed <= MinimumSpeed;






	

	}
}
