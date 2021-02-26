using System.ComponentModel.DataAnnotations;

namespace PaidGame.Server.Models
{
    /// <summary>
    /// Модель бустера очков
    /// </summary>
    public class Booster
    {
        /// <summary>
        /// Id
        /// </summary>
        [Key]
        public long Id { get; set; }

        /// <summary>
        /// Количество игр, на которые действует бустер
        /// </summary>
        public int GamesCount { get; set; }

        /// <summary>
        /// Множитель опыта
        /// </summary>
        public int ScoreMultiplier { get; set; }

        public Booster(int gamesCount, int scoreMultiplier)
        {
            GamesCount = gamesCount;
            ScoreMultiplier = scoreMultiplier;
        }
    }
}
