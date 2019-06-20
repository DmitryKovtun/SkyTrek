using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using SkyTrekVisual.GameItems.Helpers;

namespace SkyTrekVisual.GameItems
{
	/// <summary>
	/// Interaction logic for Explosion.xaml
	/// </summary>
	public partial class Explosion : UserControl, IGameItem
	{


		public Explosion()
		{
			InitializeComponent();
		}

		public Explosion(UIElement obj) : this()
		{
			var t = obj as IDestructibleItem;

			if(t == null)
				return;

			CoordBottom = t.CoordBottom-14;
			CoordLeft = t.CoordLeft;

			isActive = true;
		}

		public Explosion(UIElement obj, int type) : this(obj)
		{
			AminationType = type;

		}







		#region  IGameItem



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


		public void GenerateType()
		{
			if(!isActive)
				return;

			if(StartIterator < TextureManager.Explosions[AminationType].Count)
				ItemGrid.Background = TextureManager.Explosions[AminationType][StartIterator++];
			else
				isActive = false;
		}


		public void GenerateSize()
		{
			throw new NotImplementedException();
		}

		public ImageBrush LoadImage(int t)
		{
			throw new NotImplementedException();
		}



		public double Speed { get; set; }

		public void GoBackward()
		{
			throw new NotImplementedException();
		}
		#endregion







		int StartIterator = 0;



		public bool isActive = false;







		public int AminationType = 1;          // min 1, max 10

	







	}
}
