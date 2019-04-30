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
			CoordLeft = x;
			CoordBottom = y;
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
