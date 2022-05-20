using EFCore.Sharding;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Demo.DbMigrator
{
    /// <summary>
    /// BaseDbContextFactory
    /// </summary>
    public class BaseDbContextFactory : IDesignTimeDbContextFactory<BaseDbContext>
    {
        //private static readonly string _connectionString = "Data Source=121.36.12.76;Initial Catalog=Colder.Admin.AntdVue;uid=sa;pwd=aic3600!";
        private static readonly string _connectionString = "Data Source=.;Initial Catalog=Colder.Admin.AntdVue;Integrated Security=True;Pooling=true;";
        //private static readonly string _connectionString = "Data Source=Coldairarrow.Api.db";

        private static readonly DatabaseType _databaseType = DatabaseType.SqlServer;
        //private static readonly DatabaseType _databaseType = DatabaseType.SQLite;
        static BaseDbContextFactory()
        {
            ServiceCollection services = new ServiceCollection();

            services.AddEFCoreSharding(x =>
            {
                //取消建表
                x.CreateShardingTableOnStarting(false);

                //取消外键
                x.MigrationsWithoutForeignKey();

                //使用分表迁移
                x.EnableShardingMigration(true);

                //添加数据源
                x.AddDataSource(_connectionString, ReadWriteType.Read | ReadWriteType.Write, _databaseType);

                x.UseDatabase(_connectionString, _databaseType);
            });
            ServiceProvider = services.BuildServiceProvider();
            new EFCoreShardingBootstrapper(ServiceProvider).StartAsync(default).Wait();
        }

        /// <summary>
        /// ServiceProvider
        /// </summary>
        public static readonly IServiceProvider ServiceProvider;

        /// <summary>
        /// 创建数据库上下文
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public BaseDbContext CreateDbContext(string[] args)
        {
            var db = ServiceProvider
                .GetService<IDbFactory>()
                .GetDbContext(new DbContextParamters
                {
                    ConnectionString = _connectionString,
                    DbType = _databaseType,
                });


            return new BaseDbContext(db);

        }
    }
}
