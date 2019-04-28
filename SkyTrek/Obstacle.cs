using System.Windows.Shapes;

namespace SkyTrek
{
	public class Obstacle
	{
		public bool IsHit { set; get; } = false;

		public Rectangle VisualRect_top { set; get; }
		public Rectangle VisualRect_bottom { set; get; }

		public double Left { set; get; }
		public double Height { set; get; }
		public double Neg { set; get; }


		public Obstacle()
		{

		}

	}
}
