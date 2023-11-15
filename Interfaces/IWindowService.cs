using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DispoDataAssistant.Interfaces
{
    public interface IWindowService
    {
        void CloseWindow();
        void MaximizeWindow();
        void MinimizeWindow();
        void RestoreWindow();
    }
}
