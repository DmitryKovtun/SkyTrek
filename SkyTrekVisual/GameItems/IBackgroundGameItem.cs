using System.Windows.Media;

namespace SkyTrekVisual.GameItems
{
	public interface IBackgroundGameItem
	{
		int CoordX { get; set; }
		int CoordY { get; set; }

		ImageBrush LoadImage(int t);

		void GenerateType();
		void GenerateSize();
	}
}
