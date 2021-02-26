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
        public long Score { get; set; }
    }
}
