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
            string name = txtUserName.Text?.Trim();

            // Показываем/скрываем ошибку
            if (string.IsNullOrWhiteSpace(name))
            {
                txtError.Text = "Введите имя!";
                txtError.Visibility = Visibility.Visible;
                return;
            }

            txtError.Visibility = Visibility.Collapsed; // скрываем, если всё ок

            try
            {
                App.UserName = name;
                new MainWindow().Show();
                Close();
            }
            catch (Exception ex)
            {
                // Оставляем обработку реальных ошибок 
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка");
            }
        }
    }
}