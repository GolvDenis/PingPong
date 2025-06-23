using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PingPong.Core.Managers
{
    /// <summary>
    /// Відповідає за підрахунок очок та повідомлення про зміну рахунку.
    /// </summary>
    public class ScoreManager
    {
        public int PlayerScore { get; private set; }
        public int OpponentScore { get; private set; }
        public event System.Action<int, int>? ScoreChanged;

        public void RegisterPoint(bool playerScores)
        {
            if (playerScores) PlayerScore++; else OpponentScore++;
            ScoreChanged?.Invoke(PlayerScore, OpponentScore);
        }

        public void UpdateUI() { }
    }

}
