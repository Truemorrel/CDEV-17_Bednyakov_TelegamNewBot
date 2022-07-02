using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Microsoft.Extensions.Hosting;
using UtilityBot.Exceptions;
using System.Text.RegularExpressions;
using UtilityBot.Processes;

namespace UtilityBot
{
    internal class Bot : BackgroundService
    {
        private ITelegramBotClient _telegramClient;
        private INumbers _numberProcessor;
        private ILetters _letterProcessor;

        public Bot(ITelegramBotClient telegramClient, INumbers numberProcessor, ILetters letterProcessor)
        {
            _telegramClient = telegramClient;
            _numberProcessor = numberProcessor;
            _letterProcessor = letterProcessor;

        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _telegramClient.StartReceiving(
                HandleUpdateAsync,
                HandleErrorAsync,
                new ReceiverOptions() { AllowedUpdates = { } }, // Здесь выбираем, какие обновления хотим получать. В данном случае разрешены все
                cancellationToken: stoppingToken);

            Console.WriteLine("Бот запущен");
        }
        async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (update.Type == UpdateType.Message)
            {
                string input = update.Message.Text;
                string result = string.Empty;
                if (Regex.IsMatch(input, @"[^0-9 ]+"))
                {
                    result = _letterProcessor.Operate(input);
                }
                else
                {
                    result = _numberProcessor.Operate(input);
                };
                
                await _telegramClient.SendTextMessageAsync(update.Message.Chat.Id, result, cancellationToken: cancellationToken);
            }
        }

        Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            // Задаем сообщение об ошибке в зависимости от того, какая именно ошибка произошла
            var errorMessage = exception switch
            {
                ApiRequestException apiRequestException
                    => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                    InputException inputException
                    => $"{inputException.Message}",
                _ => exception.ToString()
            };

            // Выводим в консоль информацию об ошибке
            Console.WriteLine(errorMessage);

            // Задержка перед повторным подключением
            Console.WriteLine("Ожидаем 10 секунд перед повторным подключением.");
            Thread.Sleep(10000);

            return Task.CompletedTask;
        }
    }
}
