using CommunityToolkit.Mvvm.Messaging.Messages;
using DispoDataAssistant.Data.Models;

namespace DispoDataAssistant.Messages
{
    public class RemoveTabMessage : ValueChangedMessage<AssetTabItem>
    {
        public RemoveTabMessage(AssetTabItem selectedTabItem) : base(selectedTabItem)
        {
        }
    }
}
