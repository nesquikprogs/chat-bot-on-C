using System;
using System.Text.Json.Serialization;

namespace ChatBotLab
{
    /// <summary>
    /// Класс сообщения в чате. В модели MVC это модель (Model).
    /// </summary>
    public class Message
    {

        #region Свойства

        /// <summary>
        /// Свойство для времени отправки сообщения.
        /// </summary>
        public DateTime Time { get; private set; }

        /// <summary>
        /// Свойство для автора сообщения.
        /// </summary>
        public string Author { get; private set; }

        /// <summary>
        /// Свойство для текста сообщения.
        /// </summary>
        public string Text { get; private set; }

        #endregion

        #region Конструкторы

        /// <summary>
        /// Конструктор для создания нового сообщения с текущим временем.
        /// </summary>
        /// <param name="author">Имя отправителя.</param>
        /// <param name="text">Текст отправленного сообщения.</param>
        public Message(string author, string text)
        {
            Time = DateTime.Now; // Устанавливает текущее время
            Author = author ?? string.Empty; // Устанавливает автора или пустую строку
            Text = text ?? string.Empty; // Устанавливает текст или пустую строку
        }

        /// <summary>
        /// Констурктор для десериализации из JSON с указанным временем.
        /// </summary>
        /// <param name="time">Текущее время.</param>
        /// <param name="author">Имя отправителя.</param>
        /// <param name="text">Текст отправленного сообщения.</param>
        [JsonConstructor]
        public Message(DateTime time, string author, string text)
        {
            Time = time; // Устанавливает переданное время
            Author = author ?? string.Empty; // Устанавливает автора или пустую строку
            Text = text ?? string.Empty; // Устанавливает текст или пустую строку
        }

        #endregion

    }
}