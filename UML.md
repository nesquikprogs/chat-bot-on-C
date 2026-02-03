@startuml
skinparam classAttributeIconSize 0

package "ChatBotLab" {

    interface IMessageHistory {
        + IReadOnlyList<Message> History
        + void AddMessage(author: string, text: string)
        + void LoadHistory()
        + void SaveHistory()
    }

    interface IMessageProcessor {
        + string ProcessMessage(userName: string, userMessage: string)
    }

    class ChatBot  {
        - List<Message> _history
        - const string HistoryFile
        + IReadOnlyList<Message> History
        + void AddMessage(...)
        + string ProcessMessage(...)
        + void LoadHistory()
        + void SaveHistory()
    }

    class Message  {
        + DateTime Time
        + string Author
        + string Text
        + Message(author: string, text: string)
        + Message(time: DateTime, author: string, text: string) <<JsonConstructor>>
    }

    class App  {
        + {static} string UserName
        + void OnStartup(e: StartupEventArgs)
    }

    class LoginWindow  {
        - TextBox txtUserName
        - TextBlock txtError
        + void Button_Click(...)
    }

    class MainWindow  {
        - IMessageHistory history
        - IMessageProcessor processor
        + void SendMessage()
        + void UpdateChatHistory()
        + void Window_Closing(...)
    }

    ChatBot .up.|> IMessageHistory : реализует
    ChatBot .up.|> IMessageProcessor : реализует

    ChatBot o--> "*" Message : содержит историю

    MainWindow --> IMessageHistory : использует
    MainWindow --> IMessageProcessor : использует
    MainWindow --> App : читает UserName

    LoginWindow --> App : устанавливает UserName
    LoginWindow --> MainWindow : создаёт и показывает

    MainWindow O--> СhatBot 

    App --> LoginWindow : запускает первым
}



@enduml