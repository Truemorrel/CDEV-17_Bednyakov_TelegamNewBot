﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using VoiceTexterBot.Configuration;
using VoiceTexterBot.Controllers;
using VoiceTexterBot.Services;

namespace VoiceTexterBot
{
    public class Program
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
            AppSettings appSettings = BuildAppSettings();
            services.AddSingleton(BuildAppSettings);
            services.AddSingleton<IFileHandler, AudioFileHandler>();
            services.AddTransient<DefaultMessageController>();
            services.AddTransient<VoiceMessageController>();
            services.AddTransient<TextMessageController>();
            services.AddSingleton<IStorage, MemoryStorage>();
            services.AddTransient<InlineKeyboardController>();

            // Регистрируем объект TelegramBotClient c токеном подключения
            services.AddSingleton<ITelegramBotClient>(provider => new TelegramBotClient(appSettings.BotToken));
            // Регистрируем постоянно активный сервис бота
            services.AddHostedService<Bot>();
        }

        static AppSettings BuildAppSettings()
        {
            return new AppSettings()
            {
                DownloadsFolder = "D:\\Repos\\Skillfactory",
                BotToken = "5411416941:AAE7PHjxc6mBOUIzK6toLWuCLw3g10WpwJo",
                AudioFileName = "audio",
                InputAudioFormat = "ogg"
            };
        }
    }
}
