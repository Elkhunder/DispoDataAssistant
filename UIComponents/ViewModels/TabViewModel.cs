﻿using CommunityToolkit.Mvvm.Collections;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DispoDataAssistant.Data.Models;
using DispoDataAssistant.Services.Interfaces;
using DispoDataAssistant.UIComponents.BaseViewModels;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DispoDataAssistant.UIComponents.ViewModels;

public partial class TabViewModel : BaseViewModel
{
    public TabViewModel() : this(null!, null!) { }

    [ObservableProperty]
    private TabModel tab;

    public ObservableGroupedCollection<int, ServiceNowAsset> Data { get; set; } = [];

    public TabViewModel(ILogger logger, IWindowService windowService) : base(logger, windowService)
    {
    
    }

    private ObservableGroupedCollection<int, ServiceNowAsset> LoadData(TabModel tab, IEnumerable<ServiceNowAsset> assets)
    {
        this.Tab = tab;

        return new ObservableGroupedCollection<int, ServiceNowAsset>(
            assets
                .GroupBy(assets => assets.TabId)
                .OrderBy(group => group.Key)
        );
    }
}
