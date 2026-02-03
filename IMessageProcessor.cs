namespace ChatBotLab
{
    /// <summary>
    /// Интерфейс для обработки и генерации ответов чат-бота. В модели MVC это модель (Model).
    /// </summary>
    public interface IMessageProcessor
    {
        /// <summary>
        /// Метод(контракт) для обработки входящего сообщения пользователя и генерации ответа бота.
        /// </summary>
        /// <param name="userName">Имя отправителя.</param>
        /// <param name="userMessage">Текст сообщения.</param>
        /// <returns>Ответ бота на сообщение пользователя.</returns>
        string ProcessMessage(string userName, string userMessage);
    }
}