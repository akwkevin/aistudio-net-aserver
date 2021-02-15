using Coldairarrow.Entity.Base_Manage;
using Coldairarrow.Util;
using EFCore.Sharding;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace Coldairarrow.Business.Base_Manage
{
    public class Base_UserRoleBusiness : BaseBusiness<Base_UserRole>, IBase_UserRoleBusiness, ITransientDependency
    {
        public Base_UserRoleBusiness(IDbAccessor db)
            : base(db)
        {
        }
    }
}