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
		}



		/// <summary>
		/// BACKUP Start position in Canvas - horizontal						-- TODO - fix it
		/// </summary>
		public static readonly double Player_DefaultForwardPosition = 250.0;

		/// <summary>
		/// BACKUP Start position in Canvas - vertical		//default 200		-- TODO - fix it	
		/// </summary>
		public static readonly double Player_DefaultLiftPosition = 200.0;






		/// <summary>
		/// Start position in Canvas - horizontal
		/// </summary>
		public double Player_ForwardPosition { get; set; } = Player_DefaultForwardPosition;

		/// <summary>
		/// Start position in Canvas - vertical. Defines how high is player on canvas
		/// </summary>
		public double Player_LiftPosition { get; set; } = Player_DefaultLiftPosition;



		/// <summary>
		/// Size of a ship
		/// </summary>
		public double Player_Size { get; set; } = 48.0;


		/// <summary>
		/// ????
		/// </summary>
		public double Player_Speed { get; set; } = 0.0;


		/// <summary>
		/// ????
		/// </summary>
		public double Player_CurrentSpeed { get; set; } = 0.0;



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


		public static double BackwardSpeedModifier { get; set; } = 0.0008;
		public static double ForewardSpeedModifier { get; set; } = 0.05;



		public double MinimumSpeed { get; set; } = Player_DefaultForwardPosition;
		public  double MaximumSpeed { get; set; } = Player_DefaultForwardPosition + 120;


	}
}
