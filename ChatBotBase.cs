using System.Collections.Generic;

namespace ChatBotLab
{

    // Абстрактный класс чат-бота
    public abstract class ChatBotBase
    {
        // История сообщений 
        public abstract IReadOnlyList<Message> History { get; }

        // Добавляет сообщение в историю.
        public abstract void AddMessage(string author, string text);

        // Обрабатывает сообщение пользователя и возвращает ответ бота.
        public abstract string ProcessMessage(string userName, string userMessage);

        // Загружает историю чата из хранилища.
        public abstract void LoadHistory();

        // Сохраняет историю чата в хранилище.
        public abstract void SaveHistory();
    }
}

