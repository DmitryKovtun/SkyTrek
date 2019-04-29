using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using SkyTrekVisual.Controls;

namespace SkyTrekVisual.GameItems
{
	/// <summary>
	/// Interaction logic for Player.xaml
	/// </summary>
	public partial class Player : UserControl, IGameItem
	{
		public Player()
		{
			InitializeComponent();



			CurrentSpeed = Player_DefaultXPosition;
			CurrentLift = Player_DefaultYPosition;
		}



		/// <summary>
		/// BACKUP Start position in Canvas - horizontal						-- TODO - fix it
		/// </summary>
		public static readonly int Player_DefaultXPosition = 250;

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









		/// <summary>
		/// Size of a ship
		/// </summary>
		public double Player_Size { get; set; } = 48.0;







		public void Fill(SolidColorBrush brush)
		{
			ItemGrid.Background = brush;

		}



		public int CoordX { get; set; }
		public int CoordY { get; set; }




		public ImageBrush LoadImage(int t) => new ImageBrush(new BitmapImage(new Uri(DirectoryHelper.CurrentDirectory + @"\Ships\Ship" + t .ToString() + ".png", UriKind.Relative))) { Stretch = Stretch.UniformToFill };




		public void GenerateType()
		{
			ItemGrid.Background = LoadImage(2);
		}

		public void GenerateSize()
		{
			ItemGrid.Height = ItemGrid.Width = new Random().Next(32, 64);
		}


		public double BackwardSpeedModifier { get; set; } = 0.00008;
		public double ForewardSpeedModifier { get; set; } = 0.001;






		public int MinimumSpeed { get; } = Player_DefaultXPosition;
		public int MaximumSpeed { get; set; } = Player_DefaultXPosition + 300;




		public bool IsSpeedMaximum() => CurrentSpeed >= MaximumSpeed;

		public bool IsSpeedMinimum() => CurrentSpeed <= MinimumSpeed;






	

	}
}
