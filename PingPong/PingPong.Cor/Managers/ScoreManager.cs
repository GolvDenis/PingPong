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

        /// <summary>
        /// Викидається щоразу, коли змінюється рахунок.
        /// Підписуйтесь у UI, щоб оновлювати відображення.
        /// Параметри: (PlayerScore, OpponentScore)
        /// </summary>
        public event Action<int, int>? ScoreChanged;

        /// <summary>
        /// Реєструє набране очко (true — гравець, false — суперник).
        /// </summary>
        public void RegisterPoint(bool playerScores)
        {
            if (playerScores)
                PlayerScore++;
            else
                OpponentScore++;

            ScoreChanged?.Invoke(PlayerScore, OpponentScore);
        }

        /// <summary>
        /// Метод-«ноу-оп» для виклику в Game.Update.
        /// Якщо ви оновлюєте UI через ScoreChanged — можете залишити порожнім.
        /// </summary>
        public void UpdateUI()
        {
            // Якщо потрібно, можна тут робити додаткові дії при кожному кадрі.
        }
    }

}
