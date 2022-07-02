﻿namespace VoiceTexterBot.Configuration
{
    public class AppSettings
    {
        /// <summary>
        /// Формат аудио при конвертации
        /// </summary>
        public string OutputAudioFormat { get; set; }

        /// <summary>
        /// Токен Telegram API
        /// </summary>
        public string BotToken { get; set; }

        /// <summary>
        /// Папка загрузки аудио файлов
        /// </summary>
        public string DownloadsFolder { get; set; }

        /// <summary>
        /// Имя файла при загрузке
        /// </summary>
        public string AudioFileName { get; set; }

        /// <summary>
        /// Формат аудио при загрузке
        /// </summary>
        public string InputAudioFormat { get; set; }

        /// <summary>
        /// Битрейт аудио при загрузке
        /// </summary>
        public float InputAudioBitrate { get; set; }
    }
}
