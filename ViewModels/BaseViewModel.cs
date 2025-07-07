using CommunityToolkit.Mvvm.ComponentModel;
using System;

namespace DispoDataAssistant.ViewModels
{
    public class BaseViewModel : ObservableObject
    {
        public BaseViewModel()
        {
            PropertyChanged += (s, e) =>
            {
                Console.WriteLine($"Property changed: {e.PropertyName}");
            };
        }
    }
}
