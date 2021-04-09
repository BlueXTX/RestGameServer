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
        /// Логин обладателя
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// Количество игр, на которые действует бустер
        /// </summary>
        public int GamesCount { get; set; }

        /// <summary>
        /// Множитель опыта
        /// </summary>
        public float ScoreMultiplier { get; set; }

        public Booster(string login, int gamesCount, float scoreMultiplier)
        {
            Login = login;
            GamesCount = gamesCount;
            ScoreMultiplier = scoreMultiplier;
        }
    }
}
