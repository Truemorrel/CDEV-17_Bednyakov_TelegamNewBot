using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using UtilityBot.Processes;

namespace UtilityBot.Processes
{
    internal class Letters : ILetters
    {
        private ITelegramBotClient _telegramBotClient;
        public Letters(ITelegramBotClient botClient)
        {
            _telegramBotClient = botClient;
        }
    public string Operate(string givenString)
        {
            return $"В стоке содержится {givenString.Length} символов";
        }
    }
}
