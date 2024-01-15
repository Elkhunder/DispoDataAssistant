using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using DispoDataAssistant.Data.Models;
using DispoDataAssistant.Managers.Interfaces;
using DispoDataAssistant.Messages;
using DispoDataAssistant.Services.Implementations;
using DispoDataAssistant.UIComponents;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace DispoDataAssistant.Managers.Implementations
{
    public partial class TabManager : ObservableRecipient, ITabManager
    {
        private readonly ILogger _logger;
        private readonly WeakReferenceMessenger _messenger;

        public TabManager() : this(null!) { }

        public TabManager(ILogger<TabManager> logger)
        {
            _logger = logger;
            _messenger = WeakReferenceMessenger.Default;

            _messenger.Register<TabManager, CreateNewTabMessage>(this, (r, msg) => r.HandleCreateNewTab(msg));
            _messenger.Register<TabManager, RenameTabMessage>(this, async (r, msg) => await r.HandleRenameTab(msg));
            _messenger.Register<TabManager, RemoveTabMessage>(this, (r, msg) => r.HandleRemoveTab(msg));
            _messenger.Register<TabManager, ClearTabMessage>(this, (r, msg) => r.HandleClearTab(msg));
            _messenger.Register<TabManager, CombineTabsMessage>(this, (r, msg) => r.HandlerCombineTabs(msg));
            _messenger.Register<TabManager, SaveTabsMessage>(this, (r, msg) => r.HandleSaveTabs(msg));
            _messenger.Register<TabManager, DownloadTabToFileMessage>(this, (r, msg) => r.HandleDownloadTabToFile(msg));
            _messenger.Register<TabManager, UploadFileToTabMessage>(this, (r, msg) => r.HandleUploadFileToTab(msg));
        }

        public ResultObject HandleCreateNewTab(CreateNewTabMessage msg)
        {
            var newTabName = _messenger.Send<RequestNewTabNameMessage>();

            if (newTabName != null)
            {

            }
            else
            {
                return new ResultObject(false, "new tab name null");
            }

            return new ResultObject(true, "tab name changed", newTabName);
        }

        public async Task<ResultObject> HandleRenameTab(RenameTabMessage msg)
        {
            var selectedTab = msg.Value;
            var response = _messenger.Send<RequestNewTabNameMessage>();
            var newTabName = response.Response;

            if (string.IsNullOrEmpty(newTabName))
            {
                _logger.LogError("New tab name was null or empty");
                return new ResultObject(false, "New tab name was null or empty");
            }
            if (selectedTab is null)
            {
                _logger.LogError("Selected tab was null");
                return new ResultObject(false, "Selected tab was null", null);
            }
            if (selectedTab.Header is null)
            {
                _logger.LogError("Selected tab header was null");
                return new ResultObject(false, "Select tab header was null");
            }
            var result = await DbService.RenameTable(selectedTab.Header, newTabName);

            if (result is not null && result.WasUpdated is true)
            {
                _logger.LogInformation("Rename was successful");
                _messenger.Send(new RefreshTabsMessage());
                return new ResultObject(true, "Rename Successful", result);

            }
            else if (result is not null && result.WasUpdated is false && result.DbException is not null)
            {
                _logger.LogError($"{result.DbException.Message}");
                MessageBox.Show(result.DbException.Message);
                return new ResultObject(false, result.DbException.Message, result.DbException);
            }
            else
            {
                _logger.LogError("Tab name was not updated and no exception was thrown");
                return new ResultObject(false, "Tab name was not updated and no exception was thrown");
            }
        }

        public void HandleRemoveTab(RemoveTabMessage msg)
        {
            var tab = msg.Value;
        }

        public void HandleClearTab(ClearTabMessage msg)
        {
            var tab = msg.Value;
        }

        public void HandlerCombineTabs(CombineTabsMessage msg)
        {
            var firstTab = msg.Value;
            var secondTab = _messenger.Send<RequestSecondTabMessage>();
            var newTabName = _messenger.Send<RequestFileNameMessage>();
        }

        public void HandleSaveTabs(SaveTabsMessage msg)
        {
            var tabs = _messenger.Send<RequestTabCollectionMessage>();
        }

        public void HandleDownloadTabToFile(DownloadTabToFileMessage msg)
        {
            var filePath = _messenger.Send<RequestFilePathMessage>();
            var fileName = _messenger.Send<RequestFileNameMessage>();
        }

        public void HandleUploadFileToTab(UploadFileToTabMessage msg)
        {
            var filePath = _messenger.Send<RequestFilePathMessage>();
        }
    }
}
