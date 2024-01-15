using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DispoDataAssistant.Data.Models
{
    public record DbResult(bool WasUpdated, int? RowsUpdatedCount, DbException? DbException);
}
