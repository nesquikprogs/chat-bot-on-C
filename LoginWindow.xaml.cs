using System;
using System.Windows;

namespace ChatBotLab
{
    /// <summary>
    /// Класс окна авторизации. В модели MVC это контроллер (Controler).
    /// </summary>
    public partial class LoginWindow : Window
    {

        #region Конструктор

        /// <summary>
        /// Конструктор окна авторизации.
        /// </summary>
        public LoginWindow()
        {
            InitializeComponent(); 
        }

        #endregion

        //public string UserName; // ?

        #region Обработчики событий

        #region Кнопка "Войти"

        /// <summary>
        /// Метод обработки нажатия кнопки "Войти".
        /// </summary>
        /// <param name="sender">Объект, который вызвал событие.</param>
        /// <param name="e">Дополнительные параметры события.</param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string name = txtUserName.Text?.Trim(); // Получение и обрезка имени пользователя


            if (string.IsNullOrWhiteSpace(name)) // Проверка на пустое имя
            {
                txtError.Text = "Введите имя!";
                txtError.Visibility = Visibility.Visible;
                return;
            }

            txtError.Visibility = Visibility.Collapsed; // Скрытие сообщения об ошибке

            try
            {
               // UserName = name; // Установка имени пользователя в глобальную переменную
                new MainWindow(name).Show(); // Создание и отображение главного окна чата
                Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка");
            }
        }

        #endregion

        #endregion

    }
}