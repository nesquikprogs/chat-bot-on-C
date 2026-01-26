using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace ChatBotLab
{
    // Класс чат-бота для обработки сообщений и ведения истории переписки
    public class ChatBot : ChatBotBase
    {
        // История сообщений чата
        private List<Message> _history = new List<Message>();

        // Публичная история 
        public override IReadOnlyList<Message> History => _history;

        // Имя файла для сохранения истории
        private const string HistoryFile = "chat_history.json";

        // Добавляет сообщение в историю
        public override void AddMessage(string author, string text)
        {
            _history.Add(new Message(author, text)); // Добавляет сообщение в историю
        }

        // Обрабатывает сообщение пользователя и возвращает ответ бота
        public override string ProcessMessage(string userName, string userMessage)
        {
            userName = userName ?? string.Empty;

            /// Реплики заданного шаблона

            // Приветствия
            if (Regex.IsMatch(userMessage, @"\b(привет|здравствуй|hi|добрый\s+день)\b", RegexOptions.IgnoreCase)) // Регулярное выражение для приветствий
            {
                return "Привет, " + userName + "!";
            }

            // Прощания
            if (Regex.IsMatch(userMessage, @"\b(пока|до\s+свидания|до\s+завтра)\b", RegexOptions.IgnoreCase)) // Регулярное выражение для прощаний
            {
                return "Пока, " + userName + ". До встречи!";
            }

            // Как дела 
            if (Regex.IsMatch(userMessage, @"\b(как\s+дела|как\s+ты|как\s+жизнь|как\s+настроение)\b", RegexOptions.IgnoreCase)) // Регулярное выражение для как дела
            {
                return "У меня всё хорошо, " + userName + "!";
            }

            /// Простые команды

            // Который час
            if (Regex.IsMatch(userMessage, @"(который\s+час|сколько\s+времени?)\??", RegexOptions.IgnoreCase)) // Регулярное выражение для который час
            {
                return "Сейчас " + DateTime.Now.ToString("HH:mm:ss");
            }

            // Какая дата
            if (Regex.IsMatch(userMessage, @"\b(дата|число|сегодня|какое\s+число|какая\s+дата|какое\s+сегодня\s+число)\b", RegexOptions.IgnoreCase)) // Регулярное выражение для какая дата
            {
                return "Сегодня " + DateTime.Now.ToString("dd MMMM yyyy") + " года";
            }

            // Статистика общения с ботом
            if (Regex.IsMatch(userMessage, @"\bстатистика\b", RegexOptions.IgnoreCase)) // Регулярное выражение для статистики
            {
                int totalMessages = _history.Count; // Общее количество сообщений
                int userMessages = _history.Count(m => m.Author == userName); // Количество сообщений от пользователя
                int botMessages = totalMessages - userMessages; // Количество сообщений от бота
                return $"Всего сообщений: {totalMessages}. От пользователя: {userMessages}. От бота: {botMessages}.";
            }

            /// Команды с параметрами

            // Умножение
            var multiplyMatch = Regex.Match(userMessage, @"(умножь|перемножь)\s+(\d+)\s+на\s+(\d+)", RegexOptions.IgnoreCase); // Регулярное выражение для умножения
            if (multiplyMatch.Success)
            {
                if (int.TryParse(multiplyMatch.Groups[2].Value, out int a) && // Пытается преобразовать вторую группу в целое число
                    int.TryParse(multiplyMatch.Groups[3].Value, out int b)) // Пытается преобразовать третую группу в целое число
                {
                    return "Результат: " + (a * b); // Возвращает результат умножения
                }
            }

            // Сложение
            var addMatch = Regex.Match(userMessage, @"(сложи|плюс|прибавь)\s+(\d+)\s+на\s+(\d+)", RegexOptions.IgnoreCase); // Регулярное выражение для сложения
            if (addMatch.Success)
            {
                if (int.TryParse(addMatch.Groups[2].Value, out int a) && // Пытается преобразовать вторую группу в целое число
                    int.TryParse(addMatch.Groups[3].Value, out int b)) // Пытается преобразовать третую группу в целое число
                {
                    return "Результат: " + (a + b); // Возвращает результат сложения
                }
            }

            // Вычитание
            var subtractMatch = Regex.Match(userMessage, @"(вычти|минус|отними)\s+(\d+)\s+на\s+(\d+)", RegexOptions.IgnoreCase); // Регулярное выражение для вычитания
            if (subtractMatch.Success)
            {
                if (int.TryParse(subtractMatch.Groups[2].Value, out int a) && // Пытается преобразовать вторую группу в целое число
                    int.TryParse(subtractMatch.Groups[3].Value, out int b)) // Пытается преобразовать третую группу в целое число
                {
                    return "Результат: " + (a - b); // Возвращает результат вычитания
                }
            }

            // Деление
            var divideMatch = Regex.Match(userMessage, @"(раздели|подели|делить)\s+(\d+)\s+на\s+(\d+)", RegexOptions.IgnoreCase); // Регулярное выражение для деления
            if (divideMatch.Success)
            {
                if (int.TryParse(divideMatch.Groups[2].Value, out int a) && // Пытается преобразовать вторую группу в целое число
                    int.TryParse(divideMatch.Groups[3].Value, out int b)) // Пытается преобразовать третую группу в целое число 
                {
                    if (b == 0)
                    {
                        return "На ноль делить нельзя!"; // Возвращает сообщение об ошибке
                    }
                    double result = (double)a / b; // Вычисляет результат деления
                    return "Результат: " + result; // Возвращает результат деления
                }
            }

            // Если ничего не подошло
            return "Не понял, " + userName + ". Попробуй спросить по-другому.";

        }

        // Сохраняет историю чата в JSON-файл
        public override void SaveHistory() // Сохраняет историю чата в JSON-файл
        {
            try
            {
                var json = JsonSerializer.Serialize(_history); // Сериализует историю в JSON
                File.WriteAllText(HistoryFile, json); // Записывает историю в файл
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка сохранения: " + ex.Message); // Выводит сообщение об ошибке
            }
        }

        // Загружает историю чата из JSON-файла
        public override void LoadHistory()
        {
            if (File.Exists(HistoryFile)) // Проверяет, существует ли файл
            {
                try
                {
                    var json = File.ReadAllText(HistoryFile); // Читает историю из файла
                    _history = JsonSerializer.Deserialize<List<Message>>(json) ?? new List<Message>(); // Десериализует историю из JSON
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Ошибка загрузки истории: " + ex.Message); // Выводит сообщение об ошибке
                }
            }
        }
    }
}