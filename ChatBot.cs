using System;
using System.Windows;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace ChatBotLab // Пространство имен для чат-бота
{
    /// <summary>
    /// Класс чат-бота, реализующий обработку сообщений и хранение истории. В модели MVC это модель (Model).
    /// </summary>
    public class ChatBot : IMessageHistory, IMessageProcessor 
    {

        #region Поля класса ChatBot

        /// <summary>
        /// Список сообщений в истории чата.
        /// </summary>
        private List<Message> _history = new List<Message>();

        /// <summary>
        /// Свойство для получения истории сообщений в режиме "только для чтения".
        /// </summary>
        public IReadOnlyList<Message> History => _history;

        /// <summary>
        /// Название файла для сохранения истории чата.
        /// </summary>
        private const string FileName = "chat_history.json";

        #endregion

        #region Методы класса ChatBot

        /// <summary>
        /// Метод для добавления сообщения в историю чата.
        /// </summary>
        /// <param name="author">Имя отправителя.</param>
        /// <param name="text">Текст сообщения.</param>
        public void AddMessage(string author, string text)
        {
            _history.Add(new Message(author, text)); 
        }

        /// <summary>
        /// Метод для обработки входящего сообщения и генерации ответа бота.
        /// </summary>
        /// <param name="userName">Имя пользователя, отправившего сообщение.</param>
        /// <param name="userMessage">Сообщение пользователя.</param>
        /// <returns>Ответ чат-бота на сообщение пользователя.</returns>
        public string ProcessMessage(string userName, string userMessage)
        {

            #region Реплики заданного шаблона

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

            #endregion

            #region Простые команды

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
            if (Regex.IsMatch(userMessage, @"\bстатистика|статистику|статистики\b", RegexOptions.IgnoreCase)) // Регулярное выражение для статистики
            {
                int totalMessages = _history.Count; // Общее количество сообщений
                int userMessages = _history.Count(m => m.Author == userName); // Количество сообщений от пользователя
                int botMessages = totalMessages - userMessages; // Количество сообщений от бота
                return $"Всего сообщений: {totalMessages}. От пользователя: {userMessages}. От бота: {botMessages}.";
            }

            #endregion

            #region Команды с параметрами

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
            var addMatch = Regex.Match(userMessage, @"(сложи|плюс|прибавь)\s+(\d+)\s+и\s+(\d+)", RegexOptions.IgnoreCase); // Регулярное выражение для сложения
            if (addMatch.Success)
            {
                if (int.TryParse(addMatch.Groups[2].Value, out int a) && // Пытается преобразовать вторую группу в целое число
                    int.TryParse(addMatch.Groups[3].Value, out int b)) // Пытается преобразовать третую группу в целое число
                {
                    return "Результат: " + (a + b); // Возвращает результат сложения
                }
            }

            // Вычитание
            var subtractMatch = Regex.Match(userMessage, @"(вычти|минус|отними)\s+(\d+)\s+из\s+(\d+)", RegexOptions.IgnoreCase);
            if (subtractMatch.Success)
            {
                if (int.TryParse(subtractMatch.Groups[2].Value, out int smaller) && // Пытается преобразовать вторую группу в целое число
                    int.TryParse(subtractMatch.Groups[3].Value, out int larger)) // Пытается преобразовать третью группу в целое число
                {
                    // Вычитаем меньшее из большего
                    return "Результат: " + (larger - smaller);
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

            // Прямые арифметические выражения
            var mathMatch = Regex.Match(userMessage, @"\s*(\-?\d+)\s*([+\-*/])\s*(\-?\d+)\s*", RegexOptions.IgnoreCase);
            if (mathMatch.Success)
            {
                if (int.TryParse(mathMatch.Groups[1].Value, out int left) &&
                    int.TryParse(mathMatch.Groups[3].Value, out int right))
                {
                    char op = mathMatch.Groups[2].Value[0];

                    if (op == '+')
                    {
                        return "Результат: " + (left + right);
                    }
                    else if (op == '-')
                    {
                        return "Результат: " + (left - right);
                    }
                    else if (op == '*')
                    {
                        return "Результат: " + (left * right);
                    }
                    else if (op == '/')
                    {
                        if (right == 0)
                        {
                            return "На ноль делить нельзя!";
                        }
                        double result = (double)left / right;
                        return "Результат: " + result;
                    }
                }
            }

            #endregion

            // Если ничего не подошло
            return "Не понял, " + userName + ". Попробуй спросить по-другому.";

        }

        /// <summary>
        /// Метод для сохранения истории чата в JSON-файл.
        /// </summary>
        public void SaveHistory() 
        {
            try
            {
                var json = JsonSerializer.Serialize(_history); // Сериализует историю в JSON
                File.WriteAllText(FileName, json); // Записывает историю в файл
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                                "Не удалось загрузить историю чата.\n" +
                                "Чат начнётся заново.\n\n" +
                                "Техническая ошибка: " + ex.Message,
                                "Ошибка загрузки",
                                MessageBoxButton.OK,
                                MessageBoxImage.Warning
                                ); // Выводит сообщение об ошибке
            }
        }

        /// <summary>
        /// Метод для загрузки истории чата из JSON-файла.
        /// </summary>
        public void LoadHistory()
        {
            if (File.Exists(FileName)) // Проверяет, существует ли файл
            {
                try
                {
                    var json = File.ReadAllText(FileName); // Читает историю из файла
                    _history = JsonSerializer.Deserialize<List<Message>>(json) ?? new List<Message>(); // Десериализует историю из JSON
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                                "Не удалось сохранить историю чата.\n" +
                                "История не сохранится после закрытия.\n\n" +
                                "Техническая ошибка: " + ex.Message,
                                "Ошибка сохранения",
                                MessageBoxButton.OK,
                                MessageBoxImage.Warning
                            ); // Выводит сообщение об ошибке
                }
            }
        }

        #endregion

    }
}