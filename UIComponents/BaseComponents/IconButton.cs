using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace DispoDataAssistant.UIComponents.BaseComponents;

public class IconButton : Button
{
    public static readonly DependencyProperty IconColorProperty = DependencyProperty.Register(nameof(IconColor), typeof(Brush), typeof(IconButton), new UIPropertyMetadata(new SolidColorBrush(Colors.Black)));

    public Brush IconColor
    {
        get { return (Brush)GetValue(IconColorProperty); }
        set { SetValue(IconColorProperty, value); }
    }

    public static readonly DependencyProperty IconNameProperty = DependencyProperty.Register(nameof(IconName), typeof(string), typeof(IconButton), new UIPropertyMetadata(string.Empty));

    public string IconName
    {
        get { return (string)GetValue(IconNameProperty); }
        set { SetValue(IconNameProperty, value); }
    }
}
