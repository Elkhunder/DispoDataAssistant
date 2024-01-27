using DispoDataAssistant.Services.Interfaces;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DispoDataAssistant.Services.Implementations;

public class FileDialogService : IFileDialogService
{
    public string OpenFileDialog()
    {
        OpenFileDialog openFileDialog = new OpenFileDialog();
        if (openFileDialog.ShowDialog() == true)
        {
            return openFileDialog.FileName;
        }

        return string.Empty;
    }

    public string SaveFileDialog()
    {
        SaveFileDialog saveFileDialog = new SaveFileDialog();
        if (saveFileDialog.ShowDialog() == true)
        {
            return saveFileDialog.FileName;
        }

        return string.Empty;
    }
}
