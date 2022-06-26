using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VoiceTexterBot.Configuration;
using Telegram.Bot;
using System.IO;

namespace VoiceTexterBot.Services
{
    internal class AudioFileHandler : IFileHandler
    {
        private readonly AppSettings _appSettings;
        private readonly ITelegramBotClient _telegramBotClient;

        public AudioFileHandler(AppSettings appSettings, ITelegramBotClient telegramBotClient)
        {
            _appSettings = appSettings;
            _telegramBotClient = telegramBotClient;
        }
        public async Task Download(string fileId, CancellationToken ct)
        {
            // Генерируем полный путь файла из конфигурации
            string inputAudioFilePath = Path.Combine(
                _appSettings.DownloadsFolder,
                $"{_appSettings.AudioFileName}.{_appSettings.InputAudioFormat}");

            using (FileStream destinationStream = File.Create(inputAudioFilePath))
            {
                // Загружаем информацию о файле
                var file = await _telegramBotClient.GetFileAsync(fileId, ct);
                if (file.FilePath == null)
                    return;

                // Скачиваем файл
                await _telegramBotClient.DownloadFileAsync(file.FilePath, destinationStream, ct);
            }
        }

        public string Process(string param)
        {
            throw new NotImplementedException();
        }
    }
}
