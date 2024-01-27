using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DispoDataAssistant.Data.Enums;

public enum ServiceNowInstallStatus
{
    Installed = 1,
    Retired = 7,
    InStock = 2, //Assumed
    InMaintenance = 3, //Assumed
    Absent = 4, //Assumed
    OnOrder = 5, //Assumed
    PendingInstall = 6, //Assumed
    PendingRepair = 8, //Assumed
    Stolen = 9 //Assumed
}