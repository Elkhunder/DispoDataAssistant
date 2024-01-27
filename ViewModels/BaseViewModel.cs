using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
