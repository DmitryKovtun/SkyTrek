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
	/// Interaction logic for Planet.xaml
	/// </summary>
	public partial class Planet : UserControl, IGameItem
	{

		public static int MaxSize = 32;
		public static int MinSize = 16;



		public Planet()
		{
			InitializeComponent();

			GenerateType();
			GenerateSize();

			SetValue(Panel.ZIndexProperty, 1000);
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


		public Planet(int x, int y) : this()
		{
			CoordLeft = x;
			CoordBottom = y;
		}


		public ImageBrush LoadImage(int t) => new ImageBrush(new BitmapImage(new Uri(DirectoryHelper.CurrentDirectory + 
				@"\Planets\Planet" + t.ToString() + ".png", UriKind.Relative)));

		public void GenerateType()
		{
		    ItemGrid.Background = LoadImage(new Random().Next() % 13 + 1);
		}

		public void GenerateSize()
		{
			ItemGrid.Height = ItemGrid.Width = new Random().Next(MinSize, MaxSize);
		}


		#endregion



	}
}
