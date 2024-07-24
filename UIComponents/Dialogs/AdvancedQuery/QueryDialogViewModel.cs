using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DispoDataAssistant.Data.Models;
using DispoDataAssistant.Extensions;
using DispoDataAssistant.Interfaces;
using DispoDataAssistant.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Windows;

namespace DispoDataAssistant.UIComponents.Dialogs.AdvancedQuery;

public partial class QueryDialogViewModel : ObservableObject
{
    [ObservableProperty]
    private ObservableCollection<QueryViewModel> queries;
    [ObservableProperty]
    private ObservableCollection<ServiceNowAsset> assets;

    private QueryViewModel newQuery;
    private readonly IServiceProvider serviceProvider;

    

    public QueryDialogViewModel(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
        newQuery = serviceProvider.GetRequiredService<QueryViewModel>();
        newQuery.Condition.LogicalOperator = LogicalOperator.None;
        Queries = [];
        Assets = [];

        Queries.Add(newQuery);
    }

    [RelayCommand]
    private void OnAddCondition()
    {
        foreach (var query in Queries)
        {
            query.Condition.AddConditionButtonVisibility = System.Windows.Visibility.Collapsed;
        }
        newQuery = serviceProvider.GetRequiredService<QueryViewModel>();
        Queries.Add(newQuery);
    }

    [RelayCommand]
    private void OnAddOrCondition()
    {
        foreach (var query in Queries)
        {
            query.Condition.AddConditionButtonVisibility = System.Windows.Visibility.Collapsed;
        }
        newQuery = serviceProvider.GetRequiredService<QueryViewModel>();
        newQuery.IsOrLogicalOperator = true;
        newQuery.Condition.LogicalOperator = LogicalOperator.Or;

        Queries.Add(newQuery);
        //Queries.Sort(c => c.Condition.LogicalOperator.ToString());
        //Queries.Sort(c => c.Condition.Field.ToString());

        newQuery = Queries.SingleOrDefault(c => c.Condition.Id == newQuery.Condition.Id)!;
        var lastQuery = Queries.LastOrDefault();

        if(newQuery is not null && lastQuery is not null)
        {
            var isLastQuery = Queries.Any() && lastQuery.Condition.Id == newQuery.Condition.Id;
            if (!isLastQuery)
            {
                lastQuery.Condition.AddConditionButtonVisibility = Visibility.Visible;
                newQuery.Condition.AddConditionButtonVisibility = Visibility.Collapsed;
            }
        }
        
    }

    [RelayCommand]
    private void OnAddAndCondition()
    {
        foreach (var query in Queries)
        {
            query.Condition.AddConditionButtonVisibility = System.Windows.Visibility.Collapsed;
        }
        newQuery = serviceProvider.GetRequiredService<QueryViewModel>();
        newQuery.IsOrLogicalOperator = false;
        newQuery.Condition.LogicalOperator = LogicalOperator.And;

        Queries.Add(newQuery);

        //Queries.Sort(c => c.Condition.LogicalOperator.ToString());
        //Queries.Sort(c => c.Condition.Field.ToString());

        newQuery = Queries.SingleOrDefault(c => c.Condition.Id == newQuery.Condition.Id)!;
        var lastQuery = Queries.LastOrDefault();

        if (newQuery is not null && lastQuery is not null)
        {
            var isLastQuery = Queries.Any() && lastQuery.Condition.Id == newQuery.Condition.Id;
            if (!isLastQuery)
            {
                lastQuery.Condition.AddConditionButtonVisibility = Visibility.Visible;
                newQuery.Condition.AddConditionButtonVisibility = Visibility.Collapsed;
            }
        }
    }

    [RelayCommand]
    private void OnConditionFieldChanged()
    {
        Queries.Sort(c => c.Condition.LogicalOperator.ToString());
        Queries.Sort(c => c.Condition.Field.ToString());
        newQuery = Queries.SingleOrDefault(c => c.Condition.Id == newQuery.Condition.Id)!;
        var lastQuery = Queries.LastOrDefault();

        if (newQuery is not null && lastQuery is not null)
        {
            var isLastQuery = Queries.Any() && lastQuery.Condition.Id == newQuery.Condition.Id;
            if (!isLastQuery)
            {
                lastQuery.Condition.AddConditionButtonVisibility = Visibility.Visible;
                newQuery.Condition.AddConditionButtonVisibility = Visibility.Collapsed;
            }
        }
    }

    [RelayCommand]
    private void OnQueryExecuted()
    {
        var conditions = new List<Condition>();
        var buildingConditions = Queries.Where(q => q.Condition.Field == Fields.Building);
        var floorConditions = Queries.Where(q => q.Condition?.Field == Fields.Floor);
        var roomConditions = Queries.Where(q => q.Condition.Field == Fields.Room);
        var imageModeConditions = Queries.Where(q => q.Condition.Field == Fields.ImageMode);
        var supportGroupConditions = Queries.Where(q => q.Condition.Field == Fields.SupportGroup);
        var lifeCycleConditions = Queries.Where(q => q.Condition.Field == Fields.LifeCycleStage);

        //foreach(var query in Queries)
        //{
        //    var condition = query.Condition;
        //    var comparison = query.Condition.ComparisonOperator;
        //    var logicalOperator = query.Condition.LogicalOperator;
        //    var value = query.Condition.Value;

        //    conditions.Add(condition);
        //    //Build query string for service now
        //    //Example Query
        //    //cmdb_ci.life_cycle_stage = End of Life^ORcmdb_ci.life_cycle_stage = Missing ^ ORcmdb_ci.life_cycle_stage = Defectivecmdb_ci.life_cycle_stage = End of Life^ORcmdb_ci.life_cycle_stage = Missing ^ ORcmdb_ci.life_cycle_stage = Defective ^ cmdb_ci.u_buildingSTARTSWITHNorthville cmdb_ci.life_cycle_stage = End of Life^ORcmdb_ci.life_cycle_stage = Missing ^ ORcmdb_ci.life_cycle_stage = Defective ^ cmdb_ci.u_buildingSTARTSWITHNorthville ^ assignment_group = 61cc8268db50c7406431572e5e961929 ^ ORassignment_group = 6e7d4e28db50c7406431572e5e9619f6
            
        //}
        var client = serviceProvider.GetRequiredService<IServiceNowApiClient>();

        var assets = client.GetAssetsByQueryAsync(Queries);
    }

    [RelayCommand]
    private void OnRemoveCondition(Guid Id)
    {
        if (Queries.Count <= 1) { return; }
        var query = Queries.SingleOrDefault(f => f.Condition.Id == Id);
        if (query is not null)
        {
            if(Queries.Last().Condition.Id == Id)
            {
                Queries.Remove(query);
                Queries.Last().Condition.AddConditionButtonVisibility = System.Windows.Visibility.Visible;
            }
            else
            {
                Queries.Remove(query);
            }
            
        }
    }
}
