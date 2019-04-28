using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SkyTrekVisual.GameItems
{
	/// <summary>
	/// Interaction logic for Asteriod.xaml
	/// </summary>
	public partial class Asteriod : UserControl, IBackgroundGameItem
	{
		public Asteriod()
		{
			InitializeComponent();

			GenerateType();
			GenerateSize();
		}

		public Asteriod(int x, int y) : this()
		{
			CoordX = x;
			CoordY = y;
		}


		public int CoordX { get; set; }
		public int CoordY { get; set; }


		public ImageBrush LoadImage(int t) => new ImageBrush(new BitmapImage(new Uri(Directory.GetCurrentDirectory().ToString() + @"\Asteroid" + t.ToString() + ".png", UriKind.Relative)));

		public void GenerateType()
		{
			ItemGrid.Background = LoadImage(new Random().Next() % 4+1);
		}


		public void GenerateSize()
		{
			ItemGrid.Height = ItemGrid.Width = new Random().Next(14,32);
		}


	}
}
