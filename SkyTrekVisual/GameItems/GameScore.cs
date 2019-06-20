using System;
using System.ComponentModel;
using System.Diagnostics;

namespace SkyTrekVisual.GameItems
{
	public class GameScore : INotifyPropertyChanged
    {
		public int Score { get; set; } = 10;


		private double _Multiplier = 1;


		public double Multiplier
		{
			get { return _Multiplier ; }
			set { if(value>1) _Multiplier = value; OnPropertyChanged("MultiplierString"); }
		}



		public double MultiplierStep { get; set; } = 0.1;



//		public double Multiplier { get; set; } = 1;




		public string ScoreString
		{
			get { return Score.ToString(); }
		}

		public string MultiplierString
		{
			get { return Math.Round(Multiplier,2).ToString() + "x"; }
		}








		public GameScore()
		{

		}

		public void Clear()
		{
			Score = 0;
			Multiplier = 0;
			OnPropertyChanged("ScoreString");
			OnPropertyChanged("MultiplierString");

		}




		public void ScoreChanges(int point)
        {
            Score += (int)(point * Multiplier);
			OnPropertyChanged("ScoreString");


			MultiplierUpdate();
		}


		public void MultiplierStringUpdate()
		{
			OnPropertyChanged("MultiplierString");
		}


		public void MultiplierUpdate()
        {
			if(Score%20000 == 0)
			{
				GC.Collect();
			}

            Multiplier += MultiplierStep;
			OnPropertyChanged("MultiplierString");
		}

		public void NewShipHit()
		{
			MultiplierUpdate();

			ScoreChanges(140);
		}

		public void NewHit()
		{
			OnPropertyChanged("MultiplierString");
			ScoreChanges(20);
		}


		public void NewKill()
		{
			MultiplierUpdate();

			ScoreChanges(110);
		}

	



		#region INotifyPropertyChanged implementation

		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		#endregion







	}
}
