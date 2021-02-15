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
        private static readonly string _connectionString = "Data Source=localhost;Initial Catalog=Colder.Admin.AntdVue;uid=sa;pwd=aic3600!";
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
                x.AddDataSource(_connectionString, ReadWriteType.Read | ReadWriteType.Write, DatabaseType.SqlServer);

                x.UseDatabase(_connectionString, DatabaseType.SqlServer);
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
                    DbType = DatabaseType.SqlServer,
                });

            return new BaseDbContext(db);
        }
    }
}
