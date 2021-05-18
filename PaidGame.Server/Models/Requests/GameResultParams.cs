using System.ComponentModel.DataAnnotations;

namespace PaidGame.Server.Models.Requests
{
    /// <summary>
    /// Параметры оконченой игровой сессии
    /// </summary>
    public class GameResultParams
    {
        /// <summary>
        /// Счет сессии
        /// </summary>
        [Required]
        public float Score { get; set; }
    }
}