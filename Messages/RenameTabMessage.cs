using CommunityToolkit.Mvvm.Messaging.Messages;
using DispoDataAssistant.Data.Models;

namespace DispoDataAssistant.Messages;

public class RenameTabMessage(AssetTabItem selectedTabItem) : ValueChangedMessage<AssetTabItem>(selectedTabItem)
{
}
