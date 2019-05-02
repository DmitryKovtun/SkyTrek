using System.Windows.Media;

namespace SkyTrekVisual.GameItems
{
	public interface IGameItem
	{
		double CoordLeft { get; set; }
		double CoordBottom { get; set; }

		ImageBrush LoadImage(int t);

		void GenerateType();
		void GenerateSize();

	}
}
