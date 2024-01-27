using CommunityToolkit.Mvvm.Messaging.Messages;

namespace DispoDataAssistant.Messages
{
    public class RequestChangeTabNameMessage : ValueChangedMessage<string>
    {
        public RequestChangeTabNameMessage(string value) : base(value)
        {
        }
    }
}
