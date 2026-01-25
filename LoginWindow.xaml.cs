using System;
using System.Windows;

namespace ChatBotLab
{
    // Окно авторизации пользователя
    public partial class LoginWindow : Window
    {
        // Конструктор окна авторизации
        public LoginWindow()
        {
            InitializeComponent(); // Инициализирует компоненты окна
        }

        // Обработчик нажатия кнопки входа
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string name = txtUserName.Text?.Trim(); // Получает и очищает имя пользователя

            if (string.IsNullOrWhiteSpace(name)) // Проверяет, что имя не пустое
            {
                MessageBox.Show("Введите имя!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                App.UserName = name; // Сохраняет имя пользователя в приложении
                new MainWindow().Show(); // Открывает главное окно чата
                Close(); // Закрывает окно авторизации
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при запуске чата:\n{ex.Message}", "Ошибка");
            }
        }
    }
}