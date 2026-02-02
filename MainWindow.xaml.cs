using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace ChatBotLab
{
    // Главное окно чата
    public partial class MainWindow : Window
    {
        // Экземпляр чат-бота
        private ChatBotBase bot;

        // Конструктор главного окна
        public MainWindow()
        {
            InitializeComponent(); // Инициализирует компоненты окна
            bot = new ChatBot(); // Создаёт экземпляр чат-бота (используется через абстрактный контракт)
            bot.LoadHistory(); // Загружает историю сообщений
            UpdateChatHistory(); // Обновляет отображение истории
        }

        // Обработчик нажатия кнопки отправки
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SendMessage(); // Отправляет сообщение
        }

        // Обработчик нажатия клавиши в поле ввода
        private void txtMessage_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) // Проверяет, нажата ли клавиша Enter
            {
                SendMessage(); // Отправляет сообщение
            }
        }

        // Отправляет сообщение и получает ответ от бота
        private void SendMessage()
        {
            if (!string.IsNullOrEmpty(txtMessage.Text)) // Проверяет, что поле не пустое
            {
                string userMessage = txtMessage.Text; // Сохраняет текст сообщения
                txtMessage.Clear(); // Очищает поле ввода

                bot.AddMessage(App.UserName, userMessage); // Добавляет сообщение пользователя в историю

                string botResponse = bot.ProcessMessage(App.UserName, userMessage); // Получает ответ от бота
                if (!string.IsNullOrEmpty(botResponse)) // Проверяет, что ответ не пустой
                {
                    bot.AddMessage("Бот", botResponse); // Добавляет ответ бота в историю
                }

                UpdateChatHistory(); // Обновляет отображение историии
            }
        }

        // Обновляет отображение истории чата
        private void UpdateChatHistory()
        {
            lstChatHistory.Items.Clear(); // Очищает список сообщений

            foreach (var msg in bot.History) // Перебирает все сообщения в истории
            {
                lstChatHistory.Items.Add($"{msg.Time:HH:mm:ss} [{msg.Author}]: {msg.Text}"); // Добавляет сообщение в список
            }

            // Безопасная прокрутка к последнему сообщению
            if (lstChatHistory.Items.Count > 0)
            {
                lstChatHistory.ScrollIntoView(lstChatHistory.Items[lstChatHistory.Items.Count - 1]);
            }
        }

        // Обработчик закрытия окна
        private void Window_Closing(object sender, CancelEventArgs e)
        {
            bot.SaveHistory(); // Сохраняет историю чата перед закрытием
        }
    }
}