using System.Collections;
using System.Windows.Controls;
using System.Windows;
using DispoDataAssistant.Data.Models;

namespace DispoDataAssistant.Extensions
{
    public class ControlExtensions
    {

    }

    public static class DataGridExtensions
    {
        public static readonly DependencyProperty SelectedItemsProperty = DependencyProperty.RegisterAttached("SelectedItems", typeof(IList), typeof(DataGridExtensions), new PropertyMetadata(null, SelectedItemsChanged));

        public static void SetSelectedItems(DependencyObject element, IList value)
        {
            element.SetValue(SelectedItemsProperty, value);
        }

        public static IList GetSelectedItems(DependencyObject element)
        {
            return (IList)element.GetValue(SelectedItemsProperty);
        }

        private static void SelectedItemsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is DataGrid dataGrid)
            {
                dataGrid.SelectionChanged -= DataGrid_SelectionChanged;
                dataGrid.SelectionChanged += DataGrid_SelectionChanged;
            }
        }

        private static void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var dataGrid = (DataGrid)sender;
            var selectedItems = GetSelectedItems(dataGrid);
            selectedItems?.Clear();
            if (dataGrid.SelectedItems != null)
            {
                foreach (var item in dataGrid.SelectedItems)
                {
                    if (item is ServiceNowAsset)
                    {
                        selectedItems?.Add(item);
                    }
                    else
                    {
                        return;
                    }
                }
                    
            }
        }
    }
}
