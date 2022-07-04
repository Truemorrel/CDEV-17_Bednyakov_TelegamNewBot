using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using UtilityBot.Services;
using UtilityBot.Processes;
using System.Text.RegularExpressions;

namespace UtilityBot.Controllers
{
    public class TextMessageController
    {
        private readonly ITelegramBotClient _telegramClient;
        private readonly IStorage _memoryStorage;
        private readonly INumbers _numberProcessor;
        private readonly ILetters _letterProcessor;

        public TextMessageController(ITelegramBotClient telegramBotClient, IStorage memoryStorage, INumbers numberProcessor, ILetters letterProcessor)
        {
            _telegramClient = telegramBotClient;
            _memoryStorage = memoryStorage;
            _numberProcessor = numberProcessor;
            _letterProcessor = letterProcessor;
        }

        public async Task Handle(Message message, CancellationToken ct)
        {
            
            switch (message.Text)
            {
                case "/start":

                    ///
                    var buttons = new List<InlineKeyboardButton[]>();
                    buttons.Add(new[]
                    {
                       InlineKeyboardButton.WithCallbackData($"Текст", $"txt"),
                       InlineKeyboardButton.WithCallbackData($"Числа", $"dig"),
                       InlineKeyboardButton.WithCallbackData($"Авто", $"auto"),

                    });

                    // передаем кнопки вместе с сообщением (параметр ReplyMarkup)
                    await _telegramClient.SendTextMessageAsync(message.Chat.Id,
                        $"<b> Наш бот читает слова и складывает цифры. </b> {Environment.NewLine}" +
                        $"{Environment.NewLine}В авто режиме можно писать что угодно.{Environment.NewLine}",
                        cancellationToken: ct, parseMode: ParseMode.Html, replyMarkup: new InlineKeyboardMarkup(buttons));
                    break;
                default:
                    string input = message.Text;
                    string result;
                    if (_memoryStorage.GetSession(message.Chat.Id).CommandChoice == "txt")
                    {
                        result = _letterProcessor.Operate(input);
                    }
                    else if (_memoryStorage.GetSession(message.Chat.Id).CommandChoice == "dig")
                    {
                        result = _numberProcessor.Operate(input);
                    }
                    else
                    {
                        if (Regex.IsMatch(input, @"[^0-9 ]+"))
                        {
                            result = _letterProcessor.Operate(input);
                        }
                        else
                        {
                            result = _numberProcessor.Operate(input);
                        };
                    };

                    await _telegramClient.SendTextMessageAsync(message.Chat.Id, result, cancellationToken: ct);

                    break;
            };
        }
    }
}
