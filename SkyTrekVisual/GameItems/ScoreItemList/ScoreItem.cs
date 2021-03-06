﻿namespace SkyTrekVisual.GameItems.ScoreItemList
{
	public class ScoreItem : NotifyPropertyChanged
	{
		private string _Name;

		public string Name
		{
			get { return _Name; }
			set { _Name = value; OnPropertyChanged("Name"); }

		}

		private int _Score;

		public int Score
		{
			get { return _Score; }
			set { _Score = value; OnPropertyChanged("Score"); }


		}


		private string _Date;

		public string Date
		{
			get { return _Date; }
			set { _Date = value; OnPropertyChanged("Date"); }

		}

		public ScoreItem()
		{

		}


		public ScoreItem(string name, int score, string date)
		{
			if((Name = name) == "")
				Name = "Unknown player";

			Score = score;
			Date = date;
		}


	}
}
