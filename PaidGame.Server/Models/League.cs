using System.ComponentModel.DataAnnotations;

namespace PaidGame.Server.Models
{
    /// <summary>
    /// Модель рейтинговой лиги
    /// </summary>
    public class League
    {
        /// <summary>
        /// Id
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Название
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Требуется очков для вступления
        /// </summary>
        public int ScoreRequirement { get; set; }

        /// <summary>
        /// Множитель денег
        /// </summary>
        public float MoneyMultiplier { get; set; }
    }
}
