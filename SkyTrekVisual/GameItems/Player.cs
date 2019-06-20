using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkyTrekVisual.GameItems.BonusItems;

namespace SkyTrekVisual.GameItems
{
	public class Player
	{
		public Player()
		{


			Ship = new SpaceShip();
		}


		public SpaceShip Ship;




		public void GotBonus(BonusItem bonus)
		{
			switch(bonus.Type)
			{
				case BonusType.ExtraAmmo:

					break;
				case BonusType.Health:
					Ship.Heal(HealthPoints * 2);
					break;
				case BonusType.ScoreMultiplier:
					Score.Multiplier *= 20;
					break;
				case BonusType.Shield:

					break;

				default:
					break;
			}




		}








		public GameScore Score { get; set; } = new GameScore();



		public void Reset()
		{
			Ship.HealthPoints = 100;

			Score.Clear();

		}


		public string UserName { get; set; }


		public double HealthPoints { get { return Ship.HealthPoints; }  set { Ship.HealthPoints = value; } }





	}


}



