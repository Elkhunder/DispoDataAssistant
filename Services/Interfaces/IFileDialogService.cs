﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DispoDataAssistant.Services.Interfaces;

public interface IFileDialogService
{
    string OpenFileDialog();
    string SaveFileDialog();
}
