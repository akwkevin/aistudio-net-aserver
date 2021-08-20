using Coldairarrow.Business.Base_Manage;
using Coldairarrow.Util;
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
        IBase_UserBusiness userBusiness { get { return ServiceLocator.Instance.GetRequiredService<IBase_UserBusiness>(); } }

        public async Task Execute(IJobExecutionContext context)
        {
            var adminUser = await userBusiness.FirstOrDefaultAsync(p => p.UserName == "Admin");
            if (adminUser != null)
            {
                adminUser.Password = "Admin".ToMD5String();
                var result = await userBusiness.UpdateAsync(adminUser);
            }
        }
    }
}
