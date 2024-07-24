using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DispoDataAssistant.UIComponents.Dialogs.AdvancedQuery;

public enum Fields
{
    Building,
    Floor,
    Room,
    ImageMode,
    SupportGroup,
    LifeCycleStage
}

public class ConditionFields()
{
    private static readonly Dictionary<Fields, string> DisplayNames = new()
    {
        {Fields.Building, "Building" },
        {Fields.Floor, "Floor" },
        {Fields.Room, "Room" },
        {Fields.ImageMode, "Core Image Mode" },
        {Fields.SupportGroup, "Support Group" },
        {Fields.LifeCycleStage, "Life Cycle Stage" }
    };

    public static List<string> GetDisplayNames()
    {
        return [.. DisplayNames.Values];
    }

    public static Fields GetFieldFromDisplayName(string displayName)
    {
        return DisplayNames.First(kvp => kvp.Value == displayName).Key;
    }
}

public enum ComparisonOperator
{
    Contains,
    Is,
    IsNot,
    Are,
    StartsWith,
    EndsWith,
    IsEmpty
}

public class ConditionComparisonOperator()
{
    private static readonly Dictionary<ComparisonOperator, string> DisplayNames = new()
    {
        {ComparisonOperator.Contains, "Contains" },
        {ComparisonOperator.Is, "Is" },
        {ComparisonOperator.IsNot, "Is Not" },
        {ComparisonOperator.Are, "Are" },
        {ComparisonOperator.StartsWith, "Starts With" },
        {ComparisonOperator.EndsWith, "Ends With" },
        {ComparisonOperator.IsEmpty, "Is Empty" }
    };

    public static List<string> GetDisplayNames()
    {
        return [.. DisplayNames.Values];
    }

    public static ComparisonOperator GetOperatorFromDisplayName(string displayName)
    {
        return DisplayNames.First(kvp => kvp.Value == displayName).Key;
    }
}

public enum LogicalOperator
{
    And,
    Or,
    None
}

public class ConditionLogicalOperator()
{
    private static readonly Dictionary<LogicalOperator, string> DisplayNames = new()
    {
        // FilterLogicalOperator
        { LogicalOperator.And, "And" },
        { LogicalOperator.Or, "Or" },
        { LogicalOperator.None, "" }
    };

    public static List<string> GetDisplayNames()
    {
        return [.. DisplayNames.Values];
    }

    public static LogicalOperator GetOperatorFromDisplayName(string displayName)
    {
        return DisplayNames.First(kvp => kvp.Value == displayName).Key;
    }
}
public partial class Condition : ObservableObject
{
    public Guid Id { get; } = Guid.NewGuid();
    public Fields Field { get; set; }
    [ObservableProperty]
    private ComparisonOperator comparisonOperator;
    public string? Value { get; set; }
    [ObservableProperty]
    private Visibility addConditionButtonVisibility = Visibility.Visible;
    [ObservableProperty]
    private LogicalOperator logicalOperator;

    partial void OnComparisonOperatorChanged(ComparisonOperator value)
    {
        ComparisonOperator = ConditionComparisonOperator.GetOperatorFromDisplayName(value.ToString());
    }

    partial void OnComparisonOperatorChanging(ComparisonOperator value)
    {
        throw new NotImplementedException();
    }
}
