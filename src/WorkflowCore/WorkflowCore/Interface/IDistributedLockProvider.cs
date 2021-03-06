using System.Threading;
using System.Threading.Tasks;

namespace WorkflowCore.Interface
{
    /// <remarks>
    /// The implemention of this interface will be responsible for
    /// providing a (distributed) locking mechanism to manage in flight workflows    
    /// </remarks>
    public interface IDistributedLockProvider
    {
        Task<bool> AcquireLock(string Id, CancellationToken cancellationToken);

        Task ReleaseLock(string Id);

        Task Start();

        Task Stop();
    }
}
