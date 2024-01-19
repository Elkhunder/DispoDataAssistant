using System.Data.Common;

namespace DispoDataAssistant.Data.Models
{
    public record DbResult(bool WasUpdated, int? RowsUpdatedCount, DbException? DbException);
}
