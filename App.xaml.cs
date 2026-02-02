using System.Windows;

namespace ChatBotLab
{
    /// <summary>
    /// Главный класс приложения.
    /// </summary>
    public partial class App : Application
    {

        #region Свойства класса App

        /// <summary>
        /// Свойство для хранения имени пользователя.
        /// </summary>
        public static string UserName { get; set; } = string.Empty;

        #endregion

        #region Методы 

        /// <summary>
        /// Метод, вызываемый при запуске приложения.
        /// </summary>
        /// <param name="e">Аргументы запуска приложения.</param>
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e); // Вызывает базовый метод класса Application - запускает приложение

            LoginWindow login = new LoginWindow(); // Создаёт окно авторизации
            login.Show(); // Отображает окно авторизации
        }

        #endregion

    }
}