using EFCore.Sharding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.DbMigrator
{
    public class SqliteDbContext : GenericDbContext
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbContext"></param>
        public SqliteDbContext(GenericDbContext dbContext)
            : base(dbContext)
        {

        }

    }
}
