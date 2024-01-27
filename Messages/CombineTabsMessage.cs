using CommunityToolkit.Mvvm.Messaging.Messages;
using DispoDataAssistant.Data.Models;

namespace DispoDataAssistant.Messages
{
    public class CombineTabsMessage : ValueChangedMessage<AssetTabItem>
    {
        public CombineTabsMessage(AssetTabItem selectedTabItem) : base(selectedTabItem)
        {
        }
    }
}
