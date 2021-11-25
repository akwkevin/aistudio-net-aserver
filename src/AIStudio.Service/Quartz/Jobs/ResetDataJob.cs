using Coldairarrow.Business.Base_Manage;
using Coldairarrow.Entity;
using Coldairarrow.Entity.Base_Manage;
using Coldairarrow.Util;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIStudio.Service.Quartz
{
    public class ResetDataJob : IJob
    {  
        public async Task Execute(IJobExecutionContext context)
        {         
            
        }
    }
}
