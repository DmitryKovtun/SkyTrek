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

		public Gun(double reloadTime) : this()
		{
			ReloadTime = reloadTime;

		}



		DispatcherTimer GunReloadTimer;

		double ReloadTime = 0.5;

		bool isGunLoaded = true;



		private void GunReload_Tick(object sender, EventArgs e)
		{
			isGunLoaded = true;
		}

		public void MakeAShot(ISpaceShip player)
		{
			if(isGunLoaded)
			{
				PlayerShot.GenerateBullets(player);
				isGunLoaded = false;
			}

			GunReloadTimer.Start();
		}


		public void MakeAShotRight(Enemy enemy)
		{
			if(isGunLoaded)
			{
				PlayerShot.GenerateBulletsRight(enemy);
				isGunLoaded = false;
			}

			GunReloadTimer.Start();
		}



	}


}