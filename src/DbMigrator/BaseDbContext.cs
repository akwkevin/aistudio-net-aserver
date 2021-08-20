using EFCore.Sharding;
using Microsoft.EntityFrameworkCore;
using System;
using System.Reflection;

namespace Demo.DbMigrator
{
    /// <summary>
    /// BaseDbContext
    /// </summary>
    public class BaseDbContext : GenericDbContext
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbContext"></param>
        public BaseDbContext(GenericDbContext dbContext)
            : base(dbContext)
        {

        }

    }
}
