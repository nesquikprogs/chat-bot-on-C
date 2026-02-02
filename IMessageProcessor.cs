namespace ChatBotLab
{
    public interface IMessageProcessor
    {
        string ProcessMessage(string userName, string userMessage);
    }
}