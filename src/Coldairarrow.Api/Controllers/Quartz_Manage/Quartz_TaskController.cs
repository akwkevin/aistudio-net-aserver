using Coldairarrow.Business.Quartz_Manage;
using Coldairarrow.Util;
using Microsoft.AspNetCore.Mvc;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AIStudio.Service.Quartz;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Coldairarrow.Api.Controllers.Quartz_Manage
{
    [Route("/Quartz_Manage/[controller]/[action]")]
    public class Quartz_TaskController : BaseApiController
    {
        private readonly ISchedulerFactory _schedulerFactory;
        public Quartz_TaskController(ISchedulerFactory schedulerFactory)
        {
            this._schedulerFactory = schedulerFactory;
        }

        /// <summary>
        /// 获取所有的作业
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<PageResult<Quartz_TaskDTO>> GetDataList(PageInput<Quartz_TaskInputDTO> input)
        {
            var jobs = await _schedulerFactory.GetJobs();
            if (!string.IsNullOrEmpty(input.Search.keyword))
            {
                jobs = jobs.Where(p => p.TaskName.Contains(input.Search.keyword)).ToList();
            }
            var total = jobs.Count;
            var list = jobs.Skip((input.PageIndex - 1) * input.PageRows).Take(input.PageRows).ToList();
           
            return new PageResult<Quartz_TaskDTO>() { Total = total, Data = list };
        }

        [HttpPost]
        public async Task<Quartz_TaskDTO> GetTheData(IdInputDTO input)
        {
            return (await _schedulerFactory.GetJobs()).FirstOrDefault(p => p.Id == input.id);
        }

        [HttpPost]
        public async Task SaveData(Quartz_TaskDTO theData)
        {
            if (theData.Id.IsNullOrEmpty())
            {
                InitEntity(theData);
                theData.CreateTime = DateTime.MinValue;
                await theData.AddJob(_schedulerFactory);
            }
            else
            {
                UpdateEntity(theData);
                await _schedulerFactory.Update(theData);
            }
           
        }

        [HttpPost]
        public async Task DeleteData(List<string> ids)
        {
            var jobs = await _schedulerFactory.GetJobs();

            foreach (var id in ids)
            {
                Quartz_TaskDTO theData = jobs.FirstOrDefault(p => p.Id == id);
                if (theData != null)
                {
                    await _schedulerFactory.Remove(theData);
                }
            }             
        }

        [HttpPost]
        public async Task PauseData(List<string> ids)
        {
            var jobs = await _schedulerFactory.GetJobs();

            foreach (var id in ids)
            {
                Quartz_TaskDTO theData = jobs.FirstOrDefault(p => p.Id == id);
                if (theData != null && theData.Status== (int)TriggerState.Normal)
                {
                    await _schedulerFactory.Pause(theData);
                }
            }
        }

        [HttpPost]
        public async Task StartData(List<string> ids)
        {
            var jobs = await _schedulerFactory.GetJobs();

            foreach (var id in ids)
            {
                Quartz_TaskDTO theData = jobs.FirstOrDefault(p => p.Id == id);
                if (theData != null && theData.Status != (int)TriggerState.Normal)
                {
                    await _schedulerFactory.Start(theData);
                }
            }
        }

        [HttpPost]
        public async Task TodoData(List<string> ids)
        {
            var jobs = await _schedulerFactory.GetJobs();

            foreach (var id in ids)
            {
                Quartz_TaskDTO theData = jobs.FirstOrDefault(p => p.Id == id);
                if (theData != null)
                {
                    await _schedulerFactory.Run(theData);
                }
            }
        }
    }
}
