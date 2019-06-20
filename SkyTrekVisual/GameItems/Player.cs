using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyTrekVisual.GameItems
{
	public class Player
	{
		public Player()
		{


			Ship = new SpaceShip();
		}


		public SpaceShip Ship;



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



