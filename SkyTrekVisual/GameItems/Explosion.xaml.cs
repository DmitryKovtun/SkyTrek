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
		private UIElement uIElement;

		public Explosion()
		{
			InitializeComponent();
		}

		public Explosion(UIElement bullet, int height) : this()
		{
			var t = bullet as Bullet;

			CoordX = t.CoordX;
			CoordY = t.CoordY;

			SetValue(Canvas.LeftProperty, t.CoordX + 8 +.0);
			SetValue(Canvas.TopProperty, height - t.CoordY -16 + .0);



			LoadImages(1);

			isActive = true;
		}

		public int CoordX { get; set; }
		public int CoordY { get; set; }


		int StartIterator = 0;


		private List<ImageBrush> Images = new List<ImageBrush>();

		public bool isActive = false;

		private void LoadImages(int folder)
		{
			var files = Directory.GetFiles(DirectoryHelper.CurrentDirectory + @"\Explosions\" + folder.ToString());

			foreach(var file in files)
			{
				Images.Add(LoadImage(file));
				Debug.WriteLine(file);
			}


		}

		public ImageBrush LoadImage(string filename) => new ImageBrush(new BitmapImage(new Uri(filename, UriKind.Relative)));



		public void GenerateType()
		{
			if(!isActive)
				return;

			if(StartIterator < Images.Count)
				ItemGrid.Background = Images[StartIterator++];
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




	}
}
