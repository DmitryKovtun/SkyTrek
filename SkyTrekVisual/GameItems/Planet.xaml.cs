using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;





namespace SkyTrekVisual.GameItems
{
	/// <summary>
	/// Interaction logic for Planet.xaml
	/// </summary>
	public partial class Planet : UserControl, IBackgroundGameItem
	{
		public int CoordX { get; set; }
		public int CoordY { get; set; }


		public Planet()
		{
			InitializeComponent();

			GenerateType();
			GenerateSize();
		}


		public Planet(int x, int y) : this()
		{
			CoordX = x;
			CoordY = y;
		}


		public ImageBrush LoadImage(int t) => new ImageBrush(new BitmapImage(new Uri(Directory.GetCurrentDirectory().ToString() + 
				@"\Planets\Planet" + t.ToString() + ".png", UriKind.Relative)));

		public void GenerateType()
		{
		    ItemGrid.Background = LoadImage(new Random().Next() % 13 + 1);
		}

		public void GenerateSize()
		{
			ItemGrid.Height = ItemGrid.Width = new Random().Next(32, 64);
		}


	}
}
