using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using VoiceTexterBot.Configuration;
using VoiceTexterBot.Services;
using VoiceTexterBot.Controllers;

namespace VoiceTexterBot.Controllers
{
    public class VoiceMessageController
    {
        private readonly ITelegramBotClient _telegramClient;
        private readonly IFileHandler _audioFileHandler;
        private readonly IStorage _memoryStorage;

        public VoiceMessageController(ITelegramBotClient telegramBotClient, IFileHandler audioFileHandler, IStorage memoryStorage)
        {
            _telegramClient = telegramBotClient;
            _audioFileHandler = audioFileHandler;
            _memoryStorage = memoryStorage;
        }

        public async Task Handle(Message message, CancellationToken ct)
        {
            var fileId = message.Voice?.FileId;
            if (fileId == null)
                return;

            await _audioFileHandler.Download(fileId, ct);

            // Здесь получим язык из сессии пользователя
            string userLanguageCode = _memoryStorage.GetSession(message.Chat.Id).LanguageCode;

            // Запустим обработку
            var result = _audioFileHandler.Process(userLanguageCode);

            await _telegramClient.SendTextMessageAsync(
                message.Chat.Id,
               (result == "") ? "<<<Empty>>>" : result,
                cancellationToken: ct);
        }
    }
}
