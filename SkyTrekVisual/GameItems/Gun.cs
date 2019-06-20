using System;
using System.Windows.Controls;
using System.Windows.Threading;

namespace SkyTrekVisual.GameItems
{
	public class Gun
	{

		public Gun()
		{
			GunReloadTimer = new DispatcherTimer() { Interval = TimeSpan.FromSeconds(ReloadTime) };
			GunReloadTimer.Tick += GunReload_Tick;

		}

		public Gun(double reloadTime,int damage) : this()
		{

			GunReloadTimer.Interval = TimeSpan.FromSeconds(ReloadTime = reloadTime);

			Damage = damage;
		}


		public double Damage = 10;


		public DispatcherTimer GunReloadTimer;

		double ReloadTime = 0.5;

		bool isGunLoaded = true;


		public void Pause()
		{
			GunReloadTimer.Stop();
		}

		public void Resume()
		{
			GunReloadTimer.Start();
		}





		private void GunReload_Tick(object sender, EventArgs e)
		{
			isGunLoaded = true;
		}

		public void MakeAShot(ISpaceShip player)
		{
			if(isGunLoaded)
			{
				GunShot.GenerateBullets(player, Damage);
				isGunLoaded = false;
				GunReloadTimer.Start();
			}

			
		}


		public void MakeAShotRight(Enemy enemy)
		{
			if(isGunLoaded)
			{
				GunShot.GenerateBulletsRight(enemy, Damage);
				isGunLoaded = false;
				GunReloadTimer.Start();
			}

			
		}



	}


}