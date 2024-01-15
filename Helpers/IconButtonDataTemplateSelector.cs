using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace DispoDataAssistant.Helpers
{
    public class IconButtonDataTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate? SelectTemplate(object item, DependencyObject container)
        {
            FrameworkElement? element = container as FrameworkElement;
            if (element != null && item != null && item is string iconName)
            {
                return element.FindResource($"{iconName}DataTemplate") as DataTemplate;
            }
            return null;
        }
    }
}
