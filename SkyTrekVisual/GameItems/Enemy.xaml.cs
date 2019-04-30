using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SkyTrekVisual.Controls;
using static SkyTrekVisual.GameItems.Player;

namespace SkyTrekVisual.GameItems
{
	/// <summary>
	/// Interaction logic for Enemy.xaml
	/// </summary>
	public partial class Enemy : UserControl, IGameItem, ISpaceShip, IDestructibleItem
	{
		


		public Enemy()
		{
			InitializeComponent();



		}

		public Enemy(int x, int y,int height) : this()
		{
			CoordX = x;
			CoordY = y;

			GenerateType();

			SetValue(Canvas.LeftProperty, CoordX + .0);
			SetValue(Canvas.TopProperty, (height - CoordY) + .0);



		}



		#region IDestructibleItem

		public int ItemWidth { get { return (int)ActualWidth; } }

		public int ItemHeight { get { return (int)ActualHeight; } }




		public bool IsCollision(IDestructibleItem item) => CollisionDetector.IsCollision(this, item);


		#endregion








		#region IGameItem


		public int CoordX { get; set; }
		public int CoordY { get; set; }






		public ImageBrush LoadImage(int t) => new ImageBrush(new BitmapImage(new Uri(DirectoryHelper.CurrentDirectory + @"\Enemies\enemyBlack" + t.ToString() + ".png", UriKind.Relative))) { Stretch = Stretch.Uniform };



		public void GenerateType()
		{
			ItemGrid.Background = LoadImage(2);


		}

		public void GenerateSize()
		{
			throw new NotImplementedException();
		}

		#endregion









		#region ISpaceShip

		public ShipType CurrentShipType { get; set; }
		public int CurrentLift { get; set; }
		public int CurrentSpeed { get; set; }
		public int ShipSize { get; set; } = 32;

		public void Fill(SolidColorBrush brush)
		{
			throw new NotImplementedException();
		}

		public double BackwardSpeedModifier { get; set; }
		public double ForwardSpeedModifier { get; set; }
		public int MinimumSpeed { get; }
		public int MaximumSpeed { get; }

		public bool IsSpeedMaximum()
		{
			throw new NotImplementedException();
		}

		public bool IsSpeedMinimum()
		{
			throw new NotImplementedException();
		}

		public void MakeAShot(Canvas canvas)
		{
			throw new NotImplementedException();
		}




		#endregion







	}

}
