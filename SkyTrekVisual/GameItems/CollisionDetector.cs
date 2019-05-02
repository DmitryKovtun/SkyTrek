namespace SkyTrekVisual.GameItems
{
	public static class CollisionDetector
	{
		public static int CanvasHeight { get; set; } = 0;

		public static bool IsCollision(IDestructibleItem bullet, IDestructibleItem enemy)
		{
			return bullet.CoordLeft >= enemy.CoordLeft &&
					enemy.CoordLeft + enemy.ItemWidth >= bullet.CoordLeft &&
					enemy.CoordBottom + enemy.ItemHeight >= bullet.CoordBottom &&
					bullet.CoordBottom >= enemy.CoordBottom;
		}

	}
}
