using CommunityToolkit.Mvvm.Messaging.Messages;
using DispoDataAssistant.Data.Models;

namespace DispoDataAssistant.Messages
{
    public class RenameTabMessage : ValueChangedMessage<AssetTabItem>
    {
        public RenameTabMessage(AssetTabItem selectedTabItem) : base(selectedTabItem)
        {
        }
    }
}
