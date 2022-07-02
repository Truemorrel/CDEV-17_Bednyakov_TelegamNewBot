using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;
using System.Text;
using UtilityBot.Settings;
using UtilityBot.Exceptions;
using UtilityBot.Processes;

namespace UtilityBot
{
    internal class Program
    {
        public static async Task Main(string[] args)
        {
            Console.OutputEncoding = Encoding.Unicode;

            // Объект, отвечающий за постоянный жизненный цикл приложения
            var host = new HostBuilder()
                .ConfigureServices((hostContext, services) => ConfigureServices(services)) // Задаем конфигурацию
                .UseConsoleLifetime() // Позволяет поддерживать приложение активным в консоли
                .Build(); // Собираем

            Console.WriteLine("Сервис запущен");
            // Запускаем сервис
            await host.RunAsync();
            Console.WriteLine("Сервис остановлен");
        }

        static void ConfigureServices(IServiceCollection services)
        {
            AppConfig appSettings = MakeAppSettings();
            // Регистрируем объект TelegramBotClient c токеном подключения
            services.AddSingleton<ITelegramBotClient>(provider => new TelegramBotClient(appSettings.BotToken));
            services.AddTransient<INumbers, Numbers>();
            services.AddTransient<ILetters, Letters>();
            services.AddSingleton<InputException>();
            // Регистрируем постоянно активный сервис бота
            services.AddHostedService<Bot>();
        }

        static AppConfig MakeAppSettings ()
        {
            return new AppConfig()
            {
                BotToken = "5252471202:AAHUiXi3gVYqPSSeHHN4saiZNC1XFhu48_k"
            };
        }
    }
}
