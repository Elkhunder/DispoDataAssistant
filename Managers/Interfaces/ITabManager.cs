using DispoDataAssistant.Data.Models;
using DispoDataAssistant.Messages;
using System.Threading.Tasks;

namespace DispoDataAssistant.Managers.Interfaces
{
    public interface ITabManager
    {
        ResultObject HandleCreateNewTab(CreateNewTabMessage msg);
        Task<ResultObject> HandleRenameTab(RenameTabMessage msg);
        void HandleRemoveTab(RemoveTabMessage msg);
        void HandleClearTab(ClearTabMessage msg);
        void HandlerCombineTabs(CombineTabsMessage msg);
        void HandleSaveTabs(SaveTabsMessage msg);
        void HandleDownloadTabToFile(DownloadTabToFileMessage msg);
        void HandleUploadFileToTab(UploadFileToTabMessage msg);
    }
}
