using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyTrek
{
    class GameScore
    {
        public int score { get; set; }
        public double multiplier { get; set; }
        public double multiplier_step { get; set; }

        public GameScore()
        {
            score = 0;
            multiplier = 1;
            multiplier_step = 0.1;
        }

        public void ScoreChanges(int point)
        {
            score += (int)(point * multiplier);
            MultiplierUpdate();
        }

        public void MultiplierUpdate()
        {
            multiplier += multiplier_step;
        }
    }
}
