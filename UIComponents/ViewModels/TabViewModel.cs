using CommunityToolkit.Mvvm.Collections;
using CommunityToolkit.Mvvm.ComponentModel;
using DispoDataAssistant.Data.Contexts;
using DispoDataAssistant.Data.Models;
using DispoDataAssistant.Services.Interfaces;
using DispoDataAssistant.UIComponents.BaseViewModels;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace DispoDataAssistant.UIComponents.ViewModels;

public partial class TabViewModel : BaseViewModel
{
    public TabViewModel() : this(null!, null!, null!) { }

    private AssetContext _AssetContext;

    public ObservableCollection<TabModel> Data { get; set; } = [];

    public TabViewModel(ILogger logger, IWindowService windowService, AssetContext AssetContext) : base(logger, windowService)
    {
        AssetContext.Tabs.Load<TabModel>();
        _AssetContext = AssetContext;

        Data = AssetContext.Tabs.Local.ToObservableCollection();
    }

    private ObservableGroupedCollection<int, TabModel> LoadData()
    {
        _AssetContext.Tabs.ToList<TabModel>().ForEach(tab => { });

        return new ObservableGroupedCollection<int, TabModel>(
            _AssetContext.Tabs
                .GroupBy(tabs => tabs.Id)
                .OrderBy(group => group.Key)
        );
    }
}
