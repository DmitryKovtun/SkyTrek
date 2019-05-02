using System;
using System.Windows.Controls;
using System.Windows.Media;

namespace SkyTrekVisual.GameItems
{
	/// <summary>
	/// Interaction logic for Bullet.xaml
	/// </summary>
	public partial class LaserBeam : UserControl, IGameItem, IDestructibleItem,IDestructive
	{


		public int Speed = 5;


		public LaserBeam()
		{
			InitializeComponent();
		}



		public LaserBeam(double left, double bottom) : this()
		{		
			CoordLeft = left;
			CoordBottom = bottom;

		}


		public void GoForward()
		{	
			CoordLeft += Speed;
		}


		#region IDestructive

		public int Damage { get; set; } = 10;

		#endregion



		#region IDestructibleItem

		public int ItemWidth { get { return (int)ActualWidth; } }

		public int ItemHeight { get { return (int)ActualHeight; } }

		public bool IsCollision(IDestructibleItem item) => CollisionDetector.IsCollision(this, item);

		#endregion








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



	

		public ImageBrush LoadImage(int t)
		{
			throw new NotImplementedException();
		}

		public void GenerateType()
		{
			throw new NotImplementedException();
		}

		public void GenerateSize()
		{
			throw new NotImplementedException();
		}





		#endregion




	}
}
