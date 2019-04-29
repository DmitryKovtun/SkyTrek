using System;
using System.Windows.Controls;
using System.Windows.Media;

namespace SkyTrekVisual.GameItems
{
	/// <summary>
	/// Interaction logic for Bullet.xaml
	/// </summary>
	public partial class Bullet : UserControl, IGameItem
	{
		public static int LimitWidth;

		public int Speed = 5;


		private Player CurrentPlayer;

		public Bullet()
		{
			InitializeComponent();
		}



		public Bullet(Player currentPlayer) : this()
		{
			CurrentPlayer = currentPlayer;

			CoordX = CurrentPlayer.CoordX+CurrentPlayer.PlayerSize;
			CoordY = CurrentPlayer.CoordY-CurrentPlayer.PlayerSize/2;

		}




		public void GoForward()
		{	
			SetValue(Canvas.LeftProperty, (double)(CoordX += Speed));
		}




		public int CoordX { get; set; }
		public int CoordY { get; set; }






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

	


	




	}
}
