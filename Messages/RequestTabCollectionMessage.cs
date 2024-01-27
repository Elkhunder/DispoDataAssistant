using CommunityToolkit.Mvvm.Messaging.Messages;
using DispoDataAssistant.Data.Models;
using System.Collections.ObjectModel;

namespace DispoDataAssistant.Messages
{
    public class RequestTabCollectionMessage : CollectionRequestMessage<ObservableCollection<AssetTabItem>>
    {
    }
}
