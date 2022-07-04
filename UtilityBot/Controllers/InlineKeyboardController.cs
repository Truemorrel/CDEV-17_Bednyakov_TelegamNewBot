using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using UtilityBot.Services;

namespace UtilityBot.Controllers
{
    public class InlineKeyboardController
    {
        private readonly IStorage _memoryStorage;
        private readonly ITelegramBotClient _telegramClient;

        public InlineKeyboardController(IStorage memoryStorage,
            ITelegramBotClient telegramBotClient)
        {
            _telegramClient = telegramBotClient;
            _memoryStorage = memoryStorage;
        }

        public async Task Handle(CallbackQuery callbackQuery, CancellationToken ct)
        {
            if (callbackQuery?.Data == null)
                return;

            // Обновление пользовательской сессии новыми данными
            _memoryStorage.GetSession(callbackQuery.From.Id).CommandChoice = callbackQuery.Data;

            // Генерим информационное сообщение
            string inputCode = callbackQuery.Data switch
            {
                "txt" => " подсчет количества символов ",
                "dig" => " суммирование чисел ",
                _ => string.Empty
            };

            // Отправляем в ответ уведомление о выборе
            await _telegramClient.SendTextMessageAsync(callbackQuery.From.Id,
                $"<b>Выполняется - {inputCode}.{Environment.NewLine}</b>" +
                $"{Environment.NewLine}Можно поменять в главном меню.", cancellationToken: ct, parseMode: ParseMode.Html);
        }
    }
}
