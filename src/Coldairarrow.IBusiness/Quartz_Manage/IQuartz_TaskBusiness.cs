using Coldairarrow.Entity.Quartz_Manage;
using Coldairarrow.IBusiness;
using Coldairarrow.Util;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Business.Quartz_Manage
{
    public interface IQuartz_TaskBusiness : IBaseBusiness<Quartz_Task>
    {
        Task<PageResult<Quartz_Task>> GetDataListAsync(PageInput<Quartz_TaskInputDTO> pagination, string condition, string keyword);
        Task<Quartz_Task> GetTheDataAsync(string id);
        Task AddDataAsync(Quartz_Task data);
        Task UpdateDataAsync(Quartz_Task data);
        Task DeleteDataAsync(List<string> ids);
    }

    public class Quartz_TaskInputDTO
    {
        public string keyword { get; set; }
    }

    [Map(typeof(Quartz_Task))]
    public class Quartz_TaskDTO : Quartz_Task
    {

    }
}