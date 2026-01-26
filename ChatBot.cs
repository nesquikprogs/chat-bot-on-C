using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

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
            userMessage = userMessage.ToLower(); // Преобразует сообщение в нижний регистр

            /// Реплики заданного шаблона

            // Приветствия
            if (userMessage.Contains("привет") ||
                userMessage.Contains("здравствуй") ||
                userMessage.Contains("hi") ||
                userMessage.Contains("добрый день"))
            {
                return "Привет, " + userName + "!";
            }

            // Прощания
            if (userMessage.Contains("пока") ||
                userMessage.Contains("до свидания") ||
                userMessage.Contains("до завтра"))
            {
                return "Пока, " + userName + ". До встречи!";
            }

            // Как дела 
            if (userMessage.Contains("как дела") ||
                userMessage.Contains("как ты") ||
                userMessage.Contains("как жизнь") ||
                userMessage.Contains("как настроение"))
            {
                return "У меня всё хорошо, " + userName + "!";
            }

            /// Простые команды

            // Который час
            if (userMessage.Contains("который час?") ||
                userMessage.Contains("сколько время?") ||
                userMessage.Contains("сколько времени?"))
            {
                return "Сейчас " + DateTime.Now.ToString("HH:mm:ss");
            }

            // Какая дата
            if (userMessage.Contains("дата") ||
                userMessage.Contains("число") ||
                userMessage.Contains("сегодня") ||
                userMessage.Contains("какое число") ||
                userMessage.Contains("какая дата") ||
                userMessage.Contains("какое сегодня число"))
            {
                return "Сегодня " + DateTime.Now.ToString("dd MMMM yyyy") + " года";
            }

            // Статистика общения с ботом
            if (userMessage.Contains("статистика"))
            {
                int totalMessages = _history.Count; // Общее количество сообщений
                int userMessages = _history.Count(m => m.Author == userName); // Количество сообщений от пользователя
                int botMessages = totalMessages - userMessages; // Количество сообщений от бота
                return $"Всего сообщений: {totalMessages}. От пользователя: {userMessages}. От бота: {botMessages}.";
            }

            /// Команды с параметрами

            // Умножение
            if (userMessage.StartsWith("умножь") ||
                userMessage.StartsWith("перемножь"))
            {
                var parts = userMessage.Split(' '); // Разделяет сообщение на части
                if (parts.Length >= 4 &&
                    int.TryParse(parts[1], out int a) && // Преобразует первую часть в число
                    int.TryParse(parts[3], out int b)) // Преобразует третью часть в число
                {
                    return "Результат: " + (a * b); // Возвращает результат умножения
                }
                return "Неверный формат. Пример: умножь 12 на 157"; // Возвращает сообщение об ошибке
            }

            // Сложение
            if (userMessage.StartsWith("сложи") ||
                userMessage.StartsWith("плюс") ||
                userMessage.StartsWith("прибавь"))
            {
                var parts = userMessage.Split(' ');
                if (parts.Length >= 4 &&
                    int.TryParse(parts[1], out int a) && // Преобразует первую часть в число
                    int.TryParse(parts[3], out int b)) // Преобразует третью часть в число
                {
                    return "Результат: " + (a + b); // Возвращает результат сложения
                }
                return "Неверный формат. Пример: сложи 45 на 18";
            }

            // Вычитание
            if (userMessage.StartsWith("вычти") ||
                userMessage.StartsWith("минус") ||
                userMessage.StartsWith("отними"))
            {
                var parts = userMessage.Split(' '); // Разделяет сообщение на части
                if (parts.Length >= 4 &&
                    int.TryParse(parts[1], out int a) && // Преобразует первую часть в число
                    int.TryParse(parts[3], out int b)) // Преобразует третью часть в число
                {
                    return "Результат: " + (a - b); // Возвращает результат вычитания
                }
                return "Неверный формат. Пример: вычти 100 на 37";
            }

            // Деление
            if (userMessage.StartsWith("раздели") ||
                userMessage.StartsWith("подели") ||
                userMessage.StartsWith("делить"))
            {
                var parts = userMessage.Split(' '); // Разделяет сообщение на части
                if (parts.Length >= 4 &&
                    int.TryParse(parts[1], out int a) && // Преобразует первую часть в число
                    int.TryParse(parts[3], out int b)) // Преобразует третью часть в число
                {
                    if (b == 0)
                    {
                        return "На ноль делить нельзя!"; // Возвращает сообщение об ошибке
                    }
                    double result = (double)a / b; // Вычисляет результат деления
                    return "Результат: " + result; // Возвращает результат деления
                }
                return "Неверный формат. Пример: раздели 100 на 4";
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