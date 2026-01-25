using System.Windows;

namespace ChatBotLab
{
    // Главный класс приложения
    public partial class App : Application
    {
        // Имя текущего пользователя
        public static string UserName { get; set; } = string.Empty;

        // Метод запуска приложения
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e); // Вызывает базовый метод запуска

            // MessageBox.Show("OnStartup сработал!");  // отладочное сообщение 

            LoginWindow login = new LoginWindow(); // Создаёт окно авторизации
            login.Show(); // Отображает окно авторизации
        }
    }
}