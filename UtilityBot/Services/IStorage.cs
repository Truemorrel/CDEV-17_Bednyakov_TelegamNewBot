using UtilityBot.Models;

namespace UtilityBot.Services
{
    public interface IStorage
    {
        /// <summary>
        /// Получение сессии пользователя по идентификатору
        /// </summary>
        public Session GetSession(long chatId);
    }
}
