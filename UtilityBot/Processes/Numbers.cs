using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using UtilityBot.Exceptions;
using UtilityBot.Processes;

namespace UtilityBot.Processes
{
    public class Numbers : INumbers
    {
        private ITelegramBotClient _telegramBotClient;

        public Numbers(ITelegramBotClient botClient)
        {
            _telegramBotClient = botClient;
        }
        public string Operate(string givenString)
        {
            string[] substrings = givenString.Split(' ');
            int result = 0;
            try
            {
                for (int i = 0; i < substrings.Length; i++)
                {
                    result += Convert.ToInt32(substrings[i]);
                }

            }
            catch (Exception ex) when (ex is FormatException)
            {
                InputException exception = new InputException("Ошибка ввода");
                throw exception;
            }
            return $"Сумма равна {result}";
        }
    }
}
