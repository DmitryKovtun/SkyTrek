namespace SkyTrekVisual.GameItems
{
	public interface IDamagable
	{
		double HealthPoints { get; set; }

		bool IsAlive();

		int HitDamage { get; set; }

		void WasHit(double hitDamage);

		bool IsInvincible { get; set; }

	}



}
