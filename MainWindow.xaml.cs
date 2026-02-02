using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace ChatBotLab
{
    /// <summary>
    /// Главное окно чата
    /// </summary>
    public partial class MainWindow : Window
    {
        // Интерфейс для работы с историей сообщений
        private readonly IMessageHistory history;

        // Интерфейс для обработки и генерации ответов бота
        private readonly IMessageProcessor processor;

        /// <summary>
        /// Конструктор главного окна
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            // Создаём один объект бота, который реализует оба интерфейса
            var bot = new ChatBot();

            // Присваиваем интерфейсы
            history = bot;
            processor = bot;

            // Загружаем историю через интерфейс
            history.LoadHistory();

            // Показываем начальную историю (или приветствие)
            UpdateChatHistory();
        }

        /// <summary>
        /// Обработчик нажатия кнопки "Отправить"
        /// </summary>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SendMessage();
        }

        /// <summary>
        /// Обработчик нажатия Enter в поле ввода
        /// </summary>
        private void txtMessage_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                e.Handled = true; // Предотвращаем перенос строки
                SendMessage();
            }
        }

        /// <summary>
        /// Отправляет сообщение пользователя и получает ответ от бота
        /// </summary>
        private void SendMessage()
        {
            string userMessage = txtMessage.Text?.Trim();

            if (string.IsNullOrWhiteSpace(userMessage))
                return;

            txtMessage.Clear();

            // Добавляем сообщение пользователя через интерфейс истории
            history.AddMessage(App.UserName, userMessage);

            // Получаем ответ бота через интерфейс процессора
            string botResponse = processor.ProcessMessage(App.UserName, userMessage);

            if (!string.IsNullOrWhiteSpace(botResponse))
            {
                // Добавляем ответ бота в историю
                history.AddMessage("Бот", botResponse);
            }

            UpdateChatHistory();
        }

        /// <summary>
        /// Обновляет список сообщений в ListBox
        /// </summary>
        private void UpdateChatHistory()
        {
            lstChatHistory.Items.Clear();

            foreach (var msg in history.History) // Используем свойство из интерфейса
            {
                lstChatHistory.Items.Add($"{msg.Time:HH:mm:ss} [{msg.Author}]: {msg.Text}");
            }

            // Безопасная прокрутка к последнему сообщению
            if (lstChatHistory.Items.Count > 0)
            {
                lstChatHistory.ScrollIntoView(lstChatHistory.Items[lstChatHistory.Items.Count - 1]);
            }
        }

        /// <summary>
        /// Обработчик закрытия окна — сохраняем историю
        /// </summary>
        private void Window_Closing(object sender, CancelEventArgs e)
        {
            history.SaveHistory(); // Сохраняем через интерфейс
        }
    }
}