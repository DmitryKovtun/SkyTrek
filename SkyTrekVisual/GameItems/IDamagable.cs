using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyTrekVisual.GameItems
{
	public interface IDamagable
	{
		double HealthPoints { get; set; }

		bool IsAlive();

		int HitDamage { get; set; }

		void WasHit(double hitDamage);

	}



}
