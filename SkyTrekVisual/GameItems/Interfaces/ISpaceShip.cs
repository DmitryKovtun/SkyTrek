using System.Windows.Controls;
using System.Windows.Media;
using static SkyTrekVisual.GameItems.Player;

namespace SkyTrekVisual.GameItems
{
	public interface ISpaceShip : IGameItem, IDestructibleItem
	{

		/// <summary>
		/// Indicates what type of ship is current object
		/// </summary>
		ShipType CurrentShipType { get; set; }



		/// <summary>
		/// 
		/// </summary>
		/// <param name="expCanvas"></param>
		void StartShipExplosion(Canvas expCanvas);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ship"></param>
		/// <returns></returns>
		bool IsShipCollision(IDestructibleItem ship);


		/// <summary>
		/// 
		/// </summary>
		/// <param name="brush"></param>
		void Fill(SolidColorBrush brush);

		/// <summary>
		/// Modifier of backward speed
		/// </summary>
		double BackwardSpeedModifier { get; set; }

		/// <summary>
		/// Modifier of forward speed
		/// </summary>
		double ForwardSpeedModifier { get; set; }

		/// <summary>
		/// Returns minimum speed of this ship
		/// </summary>
		int MinimumSpeed { get; }
		
		/// <summary>
		/// Returns maximum speed of this ship
		/// </summary>
		int MaximumSpeed { get; }


		/// <summary>
		/// Returns true if speed is maximum
		/// </summary>
		/// <returns>true of false</returns>
		bool IsSpeedMaximum();

		/// <summary>
		/// Returns true if speed is minimum
		/// </summary>
		/// <returns>true of false</returns>
		bool IsSpeedMinimum();


		/// <summary>
		/// Makes a shot from blaster
		/// </summary>
		/// <param name="canvas"></param>
		void MakeAShot(Canvas canvas);

	}
}
