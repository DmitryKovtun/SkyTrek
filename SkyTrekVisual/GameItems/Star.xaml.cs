using System;
using System.Windows.Controls;
using System.Windows.Media;

namespace SkyTrekVisual.GameItems
{
	/// <summary>
	/// Interaction logic for Star.xaml
	/// </summary>
	public partial class Star : UserControl, IGameItem
	{
		public Star()
		{
			InitializeComponent();
		}


		public Star(int x, int y) : this()
		{
			CoordX = x;
			CoordY = y;
		}

		public int CoordX { get; set; }
		public int CoordY { get; set; }





		public ImageBrush LoadImage(int t)
		{
			return null;

		}

		public void GenerateType()
		{
			

		}

		public void GenerateSize()
		{

		}

	}
}
