using System.ComponentModel.DataAnnotations;

namespace PaidGame.Server.Models
{
    /// <summary>
    /// Список социальных сетей
    /// </summary>
    public class SocialNetworksList
    {
        /// <summary>
        /// Уникальный Id списка
        /// </summary>
        [Key]
        public long Id { get; set; }

        /// <summary>
        /// Id чата, которому принадлежит этот список
        /// </summary>
        [Required]
        public long ChatId { get; set; }

        /// <summary>
        /// Одноклассники
        /// </summary>
        public string Odnoklassniki { get; set; }

        /// <summary>
        /// ВКонтакте
        /// </summary>
        public string Vkontakte { get; set; }

        /// <summary>
        /// Фейсбук
        /// </summary>
        public string Facebook { get; set; }

        /// <summary>
        /// Твиттер
        /// </summary>
        public string Twitter { get; set; }

        /// <summary>
        /// Инстаграм
        /// </summary>
        public string Instagram { get; set; }

        /// <summary>
        /// Дискорд
        /// </summary>
        public string Discord { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Telegram { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="chatId">Id чата с аккаунтом</param>
        public SocialNetworksList(long chatId)
        {
            ChatId = chatId;
        }
    }
}
