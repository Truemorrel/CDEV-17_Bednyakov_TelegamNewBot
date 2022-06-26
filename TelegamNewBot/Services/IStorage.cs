using VoiceTexterBot.Models;

namespace VoiceTexterBot.Services
{
    public interface IStorage
    {
        /// <summary>
        /// Получение сессии пользователя по идентификатору
        /// </summary>
        public Session GetSession(long chatId);
    }
}
