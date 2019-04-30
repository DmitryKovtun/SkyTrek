namespace SkyTrekVisual.GameItems
{
	public interface IDestructibleItem
	{



		int ItemHeight { set; get; }
		int ItemWidth { set; get; }

		int CenterX { set; get; }
		int CenterY { set; get; }

		bool IsCollision(IDestructibleItem item);


	}


}
