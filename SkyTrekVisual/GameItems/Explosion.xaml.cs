using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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

		public Explosion(UIElement bullet) : this()
		{
			var t = bullet as IDestructibleItem;

			if(t == null)
				return;

			CoordBottom = t.CoordBottom-14;
			CoordLeft = t.CoordLeft;

			isActive = true;
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

			if(StartIterator < Images.Count)
				ItemGrid.Background = Images[AminationType][StartIterator++];
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

		#endregion







		int StartIterator = 0;


		private static Dictionary<int,List<ImageBrush>> Images = new Dictionary<int, List<ImageBrush>>();

		public bool isActive = false;







		public static void InitializeImages()
		{
			var files = Directory.GetDirectories(DirectoryHelper.CurrentDirectory + @"\Explosions\");

			int i = 1;
			foreach(var dir in files.ToList())
			{
				var l = new List<ImageBrush>();

				foreach(var image in Directory.GetFiles(dir))
					l.Add(LoadImage(image));

				Images.Add(i++, l);
			}

		}

		public static ImageBrush LoadImage(string filename) => new ImageBrush(new BitmapImage(new Uri(filename, UriKind.Relative)));


		int AminationType = 1;			// min 1, max 10



		




	}
}
