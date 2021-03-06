using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WorkflowCore.QueueProviders.SqlServer.Interfaces
{
    public interface ISqlServerQueueProviderMigrator
    {
        Task MigrateDbAsync();
        Task CreateDbAsync();
    }
}
