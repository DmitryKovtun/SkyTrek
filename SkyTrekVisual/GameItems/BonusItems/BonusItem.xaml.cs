using System;
using System.Windows.Controls;
using System.Windows.Media;
using SkyTrekVisual.GameItems.Helpers;

namespace SkyTrekVisual.GameItems.BonusItems
{


	public enum BonusType
	{
		Health,
		Shield,
		ExtraAmmo,
		ScoreMultiplier
	}




	/// <summary>
	/// Interaction logic for BonusItem.xaml
	/// </summary>
	public partial class BonusItem : UserControl, IGameItem, IDestructibleItem
	{
		public BonusItem()
		{
			InitializeComponent();



		}


		public BonusItem(double x, double y) : this()
		{
			CoordLeft = x;
			CoordBottom = y;

			GenerateType();
			GenerateSize();


		}

		public BonusItem(double x, double y, BonusType type) : this()
		{
			CoordLeft = x;
			CoordBottom = y;

			LoadImage((int)type);
			GenerateSize();
		}


		public BonusType Type;


		public static Random rnd = new Random();

		








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
			Type = (BonusType)t;

			ItemGrid.Background = TextureManager.Bonuses[Type];
			return null;
		}

		public void GenerateType()
		{
			var type = rnd.Next(0, 4);
			Type = (BonusType)type;

			ItemGrid.Background = TextureManager.Bonuses[Type];
		}

		public void GenerateSize()
		{

			ItemGrid.Height = ItemGrid.Width = 24;


		}




		public double Speed { get; set; } = 3;

		public void GoBackward()
		{
			CoordLeft -= Speed;
		}






		#region IDestructibleItem

		public int ItemWidth { get { return (int)ActualWidth; } }

		public int ItemHeight { get { return (int)ActualHeight; } }

		public bool IsCollision(IDestructibleItem item) => CollisionDetector.IsCollision(this, item);


		#endregion




		#endregion





	}
}
