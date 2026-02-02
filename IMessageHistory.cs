using System.Collections.Generic;

namespace ChatBotLab
{
    /// <summary>
    /// Интерфейс для работы с историей сообщений чата.
    /// </summary>
    public interface IMessageHistory
    {
        /// <summary>
        /// Свойство для получения истории сообщений в режиме "только для чтения".
        /// </summary>
        IReadOnlyList<Message> History { get; }

        /// <summary>
        /// Метод(контракт) для добавления сообщения в историю чата.
        /// </summary>
        /// <param name="author">Имя отправителя.</param>
        /// <param name="text">Отправляемое сообщение.</param>
        void AddMessage(string author, string text);

        /// <summary>
        /// Метод(контракт) для загрузки истории сообщений из файла.
        /// </summary>
        void LoadHistory();

        /// <summary>
        /// Метод(контракт) для сохранения истории сообщений в файл.
        /// </summary>
        void SaveHistory();
    }
}