using System.Collections.Generic;

namespace AIStudio.Service.AppClient.HttpClients
{
    public interface IAppHeader
    {
        Dictionary<string, string> SetHeader();
    }
}
