using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.IsolatedStorage;
using System.Xml.Serialization;
using System.IO;
using AlienGameSample;
using System.Diagnostics;

namespace attackGame
{
    public class HighScore
    {
        public int Score { get; set; }
        public string Player { get; set; }

        public List<HighScore> SortList(List<HighScore> scores)
        {
            return scores = scores.OrderByDescending(x => x.Score).ToList();
        }

        public bool CheckScores(int score, List<HighScore> scores)
        {
            foreach (HighScore hg in scores)
            {
                if (hg.Score < score)
                {
                    return true;
                }
            }
            return false;
        }

        public List<HighScore> Rewrite(List<HighScore> origin, int score, string player)
        {
            bool finish = false;
            for (int i = 9; i >= 0; i--)
            {
                if ((score < origin[i].Score))
                {
                    origin[i].Score = score;
                    origin[i].Player = player;
                    break;
                }
                if (i == 0)
                {
                    origin[i].Score = score;
                    origin[i].Player = player;
                    finish = true;
                }
                if(!finish)
                {
                    origin[i].Player = origin[i - 1].Player;
                    origin[i].Score = origin[i - 1].Score;
                }
            }
            return origin;
        }

        public List<HighScore> SaveScores(string player, int score, List<HighScore> scores)
        {
            foreach (HighScore hg in scores)
            {
                if (hg.Score < score)
                {
                    scores = Rewrite(scores, score, player);
                    break;
                }
            }
            return scores;
        }

    }
}
