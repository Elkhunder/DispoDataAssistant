using CommunityToolkit.Mvvm.Messaging.Messages;
using DispoDataAssistant.Data.Models;

namespace DispoDataAssistant.Messages
{
    public class ClearTabMessage : ValueChangedMessage<AssetTabItem>
    {
        public ClearTabMessage(AssetTabItem selectedTabItem) : base(selectedTabItem)
        {
        }
    }
}
