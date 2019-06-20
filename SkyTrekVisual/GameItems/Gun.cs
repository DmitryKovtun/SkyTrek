using System;
using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Threading;

namespace SkyTrekVisual.GameItems
{
	public class Gun : NotifyPropertyChanged
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

			GunReloadTimer.Start();
		}


		public double Damage = 10;


		public DispatcherTimer GunReloadTimer;

		double ReloadTime = 0.5;

		bool isGunLoaded = true;













		private int _reloadValue;

		public int ReloadValue
		{
			get { return _reloadValue; }
			set {
				_reloadValue = value;
				OnPropertyChanged("ReloadValue");
			}
		}







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
			ReloadValue += 100;

			//Debug.WriteLine(ReloadValue);
			//isGunLoaded = true;
		}


		public void MakeAShot(ISpaceShip player)
		{
			if(ReloadValue > 100)
			//if(isGunLoaded)
			{
				GunShot.GenerateBullets(player, Damage);
				isGunLoaded = false;
				//GunReloadTimer.Start();
				ReloadValue -= 100;
			}
			
		}


		public void MakeAShotRight(Enemy enemy)
		{
			if(ReloadValue==100)
			//if(isGunLoaded)
			{
				GunShot.GenerateBulletsRight(enemy, Damage);
				isGunLoaded = false;
				//GunReloadTimer.Start();
				ReloadValue = 0;
			}

			
		}



	}


}