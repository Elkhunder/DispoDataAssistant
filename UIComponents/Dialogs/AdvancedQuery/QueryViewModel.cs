using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Printing;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace DispoDataAssistant.UIComponents.Dialogs.AdvancedQuery;

public partial class QueryViewModel : ObservableObject
{
    [ObservableProperty]
    private Condition condition;
    
    [ObservableProperty]
    private List<string> fields;
    [ObservableProperty]
    private List<string> comparisonOperators;
    [ObservableProperty]
    private List<string> logicalOperators;
    [ObservableProperty]
    private bool isOrLogicalOperator;

    [ObservableProperty]
    private string selectedField = String.Empty;
    [ObservableProperty]
    private string selectedComparisonOperator = String.Empty;
    [ObservableProperty]
    private string selectedLogicalOperator = String.Empty;

    public QueryViewModel()
    {
        Condition = new Condition();

        Fields = ConditionFields.GetDisplayNames();
        ComparisonOperators = ConditionComparisonOperator.GetDisplayNames();
        LogicalOperators = ConditionLogicalOperator.GetDisplayNames();

        
    }
    partial void OnSelectedFieldChanging(string value)
    {
        Condition.Field = ConditionFields.GetFieldFromDisplayName(value);
    }
    partial void OnSelectedComparisonOperatorChanging(string value)
    {
        Condition.ComparisonOperator = ConditionComparisonOperator.GetOperatorFromDisplayName(value);
    }
    partial void OnSelectedLogicalOperatorChanging(string value)
    {
        Condition.LogicalOperator = ConditionLogicalOperator.GetOperatorFromDisplayName(value);
    }
}
