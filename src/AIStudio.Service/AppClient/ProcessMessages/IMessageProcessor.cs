using Coldairarrow.Util;
using System.Threading.Tasks;

namespace AIStudio.Service.AppClient.ProcessMessages
{
    public interface IMessageProcessor
    {
        Task<AjaxResult<string>> DoResponse();
    }
}
