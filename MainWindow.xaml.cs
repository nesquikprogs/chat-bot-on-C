using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace ChatBotLab
{
    /// <summary>
    /// Класс главного окна приложения.
    /// </summary>
    public partial class MainWindow : Window
    {

        #region Поля класса MainWindow

        /// <summary>
        /// Интерфейс для работы с историей сообщений чата.
        /// </summary>
        private readonly IMessageHistory history;

        /// <summary>
        /// Интерфейс для обработки сообщений чат-ботом.
        /// </summary>
        private readonly IMessageProcessor processor;

        #endregion

        #region Конструктор

        /// <summary>
        /// Конструктор главного окна приложения.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent(); // Инициализация компонентов окна

            var bot = new ChatBot(); // Создание экземпляра чат-бота

            // Присваиваем интерфейсы
            history = bot;
            processor = bot;

            history.LoadHistory(); // Загружаем историю через интерфейс

            UpdateChatHistory(); // Показываем начальную историю 
        }

        #endregion

        #region Обработчики событий

        #region Кнопка "Отправить"

        /// <summary>
        /// Обработчик нажатия кнопки "Отправить".
        /// </summary>
        /// <param name="sender">Объект, который вызвал событие(сама кнопка).</param>
        /// <param name="e">Дополнительные аргументы события.</param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SendMessage(); // Отправка сообщения
        }

        #endregion

        #region Нажатие Enter в поле ввода

        /// <summary>
        /// Обработчик нажатия клавиши Enter в поле ввода сообщения.
        /// </summary>
        /// <param name="sender">Объект, который вызвал событие(поле ввода).</param>
        /// <param name="e">Аргументы события клавиатуры.</param>
        private void txtMessage_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) // Проверка нажатия клавиши Enter
            {
                e.Handled = true; // Предотвращаем перенос строки
                SendMessage(); // Отправка сообщения
            }
        }

        #endregion

        #region Закрытие окна

        /// <summary>
        /// Обработчик события закрытия окна.
        /// </summary>
        /// <param name="sender">Объект, который вызвал событие(само окно).</param>
        /// <param name="e">Специальный аргумент события.</param>
        private void Window_Closing(object sender, CancelEventArgs e)
        {
            history.SaveHistory(); // Сохраняем историю 
        }

        #endregion

        #endregion

        #region Методы класса MainWindow

        /// <summary>
        /// Метод для отправки сообщения пользователем и получения ответа от бота.
        /// </summary>
        private void SendMessage()
        {
            string userMessage = txtMessage.Text?.Trim(); // Получаем и обрезаем сообщение пользователя

            if (string.IsNullOrWhiteSpace(userMessage)) 
                return; // Выходим, если сообщение пустое

            txtMessage.Clear(); // Очищаем поле ввода

            history.AddMessage(App.UserName, userMessage); // Добавляем сообщение пользователя в историю

            string botResponse = processor.ProcessMessage(App.UserName, userMessage); // Получаем ответ бота

            if (!string.IsNullOrWhiteSpace(botResponse))
            {
                history.AddMessage("Бот", botResponse); // Добавляем ответ бота в историю если он не пустой
            }

            UpdateChatHistory(); // Обновляем отображение истории чата
        }

        /// <summary>
        /// Метод для обновления отображения истории чата в интерфейсе.
        /// </summary>
        private void UpdateChatHistory()
        {
            lstChatHistory.Items.Clear(); // Очищаем текущее отображение истории

            // Если история пустая — показываем приветствие от бота
            if (history.History.Count == 0)
            {
                string welcome = $"Привет, {App.UserName}! Я твой чат-бот. Напиши что-нибудь, чтобы начать!";
                lstChatHistory.Items.Add($"{DateTime.Now:HH:mm:ss} [Бот]: {welcome}");
            }
            else
            {
                foreach (var msg in history.History) // Проходим по всем сообщениям в истории
                {
                    lstChatHistory.Items.Add($"{msg.Time:HH:mm:ss} [{msg.Author}]: {msg.Text}"); // Добавляем форматированное сообщение в список
                }
            }

            if (lstChatHistory.Items.Count > 0) // Если есть хотябы одно сообщение в списке
            {
                lstChatHistory.ScrollIntoView(lstChatHistory.Items[lstChatHistory.Items.Count - 1]); // Проскролливаем к последнему сообщению
            }
        }


        #endregion
    }
}