using System.Windows.Controls;
using System.Windows;

namespace DispoDataAssistant.Behaviors
{
    public static class FocusBehavior
    {
        public static readonly DependencyProperty FocusProperty =
        DependencyProperty.RegisterAttached("Focus",
                                            typeof(bool),
                                            typeof(FocusBehavior),
                                            new PropertyMetadata(false, OnFocusPropertyChanged));

        public static bool GetFocus(DependencyObject obj)
        {
            return (bool)obj.GetValue(FocusProperty);
        }

        public static void SetFocus(DependencyObject obj, bool value)
        {
            obj.SetValue(FocusProperty, value);
        }

        private static void OnFocusPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Control control && (bool)e.NewValue)
            {
                control.Focus();
            }
        }
    }
}
