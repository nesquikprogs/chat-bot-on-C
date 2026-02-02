using System.Collections.Generic;

namespace ChatBotLab
{
    public interface IMessageHistory
    {
        IReadOnlyList<Message> History { get; }

        void AddMessage(string author, string text);

        void LoadHistory();

        void SaveHistory();
    }
}