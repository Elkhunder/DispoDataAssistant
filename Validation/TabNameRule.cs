using CommunityToolkit.Mvvm.DependencyInjection;
using DispoDataAssistant.UIComponents;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;

namespace DispoDataAssistant.Validation
{
    public class TabNameRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            TabControlEditViewModel tabControlEditViewModel = Ioc.Default.GetRequiredService<TabControlEditViewModel>();
            if (value is null)
            {
                if (tabControlEditViewModel.NewTabNamePanelVisibility == Visibility.Visible)
                {
                    return new ValidationResult(false, "Value can not be empty");
                }
            }
            else
            {
                string? strValue = value.ToString();

                if (!string.IsNullOrEmpty(strValue))
                {
                    char firstCharacter = strValue[0];

                    if (char.IsDigit(firstCharacter))
                    {
                        return new ValidationResult(false, "Value can not start with a number");
                    }
                }
            }
            return ValidationResult.ValidResult;
        }
    }
}
