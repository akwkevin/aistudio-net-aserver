using AIStudio.Service.WebSocketEx;
using Coldairarrow.Api;
using Coldairarrow.Util;
using Colder.Logging.Serilog;
using EFCore.Sharding;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Quartz;
using Quartz.Impl;
using System;
using System.Reflection;
using System.Windows;

namespace AIStudio.WpfApi
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override async void OnStartup(StartupEventArgs args)
        {
            base.OnStartup(args);

            await Host.CreateDefaultBuilder(args.Args)
                  .ConfigureLogging((context, logger) =>
                    logger.AddColorConsoleLogger(configuration =>
                    {
                        configuration.LogLevels.Add(
                            LogLevel.Critical, ConsoleColor.DarkRed);
                    }))
                   .UseIdHelper()
                   .UseCache()
                   .ConfigureServices((hostContext, services) =>
                   {
                       services.AddFxServices();
                       services.AddAutoMapper();
                       services.AddEFCoreSharding(config =>
                       {
                           var dbOptions = hostContext.Configuration.GetSection("Database:BaseDb").Get<DatabaseOptions>();

                           config.UseDatabase(dbOptions.ConnectionString, dbOptions.DatabaseType);

                           //以下为分表
                           config.AddDataSource(dbOptions.ConnectionString, ReadWriteType.Read | ReadWriteType.Write, dbOptions.DatabaseType);
                           DateTime startTime = DateTime.Now.AddMinutes(-5);
                           GlobalData.MonthTableTypes.ForEach(
                               p =>
                               {
                                   //config.SetDateSharding<D_UserMessage>(nameof(D_UserMessage.CreateTime), ExpandByDateMode.PerMonth, startTime);
                                   MethodInfo method = config.GetType().GetMethod("SetDateSharding", new Type[] { typeof(string), typeof(ExpandByDateMode), typeof(DateTime), typeof(string) });
                                   MethodInfo curMethod = method.MakeGenericMethod(p);
                                   curMethod.Invoke(config, new Object[] { "CreateTime", p.GetCustomAttribute<ExpandByDateModeTypeAttribute>().Mode, startTime, ShardingConstant.DefaultSource });

                               });
                       });

                       if (hostContext.Configuration.GetSection("UseWebSocket").Get<bool>() == true)
                       {
                           services.AddSingleton<ICustomWebSocketFactory, CustomWebSocketFactory>();
                           services.AddSingleton<ICustomWebSocketMessageHandler, CustomWebSocketMessageHandler>();
                       }

                       if (hostContext.Configuration.GetSection("UseQuartz").Get<bool>() == true)
                       {
                           services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();
                       }

                       if (hostContext.Configuration.GetSection("UseWorkflow").Get<bool>() == true)
                       {
                           var dbOptions = hostContext.Configuration.GetSection("Database:BaseDb").Get<DatabaseOptions>();

                           switch (dbOptions.DatabaseType)
                           {
                               case DatabaseType.SqlServer: services.AddWorkflow(x => x.UseSqlServer(dbOptions.ConnectionString, false, true)); break;
                               case DatabaseType.MySql: services.AddWorkflow(x => x.UseMySQL(dbOptions.ConnectionString, false, true)); break;
                               case DatabaseType.PostgreSql: services.AddWorkflow(x => x.UsePostgreSQL(dbOptions.ConnectionString, false, true)); break;
                               case DatabaseType.SQLite: services.AddWorkflow(x => x.UseSqlite(dbOptions.ConnectionString, true)); break;
                               default: throw new Exception("暂不支持该数据库！");
                           }
                           services.AddWorkflowDSL();
                       }
                   })
                   .ConfigureWebHostDefaults(webBuilder =>
                   {
                       webBuilder.UseStartup<Startup>();
                   }).UseDefaultServiceProvider(options =>
                   {
                       options.ValidateScopes = false;
                   })
                   .Build()
                   .RunAsync();
        }
    }
}
