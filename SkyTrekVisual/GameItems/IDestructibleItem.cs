namespace SkyTrekVisual.GameItems
{
	public interface IDestructibleItem : IGameItem
	{



		int ItemHeight { get; }
		int ItemWidth { get; }


		bool IsCollision(IDestructibleItem item);


	}


}
