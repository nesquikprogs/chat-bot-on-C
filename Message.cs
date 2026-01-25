using System;
using System.Text.Json.Serialization;

namespace ChatBotLab
{
    // Класс сообщения в чате
    public class Message
    {
        // Время отправки сообщения
        public DateTime Time { get; private set; }

        // Автор сообщения 
        public string Author { get; private set; }

        // Текст сообщения
        public string Text { get; private set; }

        // Конструктор для создания нового сообщения с текущим временем
        public Message(string author, string text)
        {
            Time = DateTime.Now; // Устанавливает текущее время
            Author = author ?? string.Empty; // Устанавливает автора или пустую строку
            Text = text ?? string.Empty; // Устанавливает текст или пустую строку
        }

        // Конструктор для десериализации из JSON с указанным временем
        [JsonConstructor]
        public Message(DateTime time, string author, string text)
        {
            Time = time; // Устанавливает переданное время
            Author = author ?? string.Empty; // Устанавливает автора или пустую строку
            Text = text ?? string.Empty; // Устанавливает текст или пустую строку
        }
    }
}