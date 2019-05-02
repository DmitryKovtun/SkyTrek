using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using SkyTrekVisual.Controls;

namespace SkyTrekVisual.GameItems
{
	/// <summary>
	/// Interaction logic for Asteriod.xaml
	/// </summary>
	public partial class Asteriod : UserControl, IGameItem
	{

		public static int MaxSize = 10;
		public static int MinSize = 5;


		public Asteriod()
		{
			InitializeComponent();

			GenerateType();
			GenerateSize();
		}

		public Asteriod(int x, int y) : this()
		{
			CoordLeft = x;
			CoordBottom = y;

			SetValue(Panel.ZIndexProperty, 1000);
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
			ItemGrid.Background = LoadImage(new Random().Next() % 4+1);
		}


		public void GenerateSize()
		{
			ItemGrid.Height = ItemGrid.Width = new Random().Next(MinSize,MaxSize);
		}


	}
}
