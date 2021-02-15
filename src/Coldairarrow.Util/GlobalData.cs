using EFCore.Sharding;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Coldairarrow.Util
{
    public static class GlobalData
    {
        static GlobalData()
        {
            string rootPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            AllFxAssemblies = Directory.GetFiles(rootPath, "*.dll")
                .Where(x => new FileInfo(x).Name.Contains(FXASSEMBLY_PATTERN))
                .Select(x => Assembly.LoadFrom(x))
                .Where(x => !x.IsDynamic)
                .ToList();

            AllFxAssemblies.ForEach(aAssembly =>
            {
                try
                {
                    AllFxTypes.AddRange(aAssembly.GetTypes());                   
                }
                catch
                {

                }
            });

            PhysicDeleteTypes.AddRange(AllFxTypes.Where(p => p.GetCustomAttributes<PhysicDeleteTypeAttribute>().Count() > 0));
            CachTypes.AddRange(AllFxTypes.Where(p => p.GetCustomAttributes<CachTypeAttribute>().Count() > 0));
            MonthTableTypes.AddRange(AllFxTypes.Where(p => p.GetCustomAttributes<ExpandByDateModeTypeAttribute>().Count(p => p.Mode == ExpandByDateMode.PerMonth) > 0));
            PushMessageTypes.AddRange(AllFxTypes.Where(p => p.GetCustomAttributes<PushMessageTypeAttribute>().Count() > 0));
            BatchSaveTypes.AddRange(AllFxTypes.Where(p => p.GetCustomAttributes<BatchSaveTypeAttribute>().Count() > 0));
        }

        /// <summary>
        /// 解决方案程序集匹配名
        /// </summary>
        public const string FXASSEMBLY_PATTERN = "Coldairarrow";

        /// <summary>
        /// 解决方案所有程序集
        /// </summary>
        public static readonly List<Assembly> AllFxAssemblies;

        /// <summary>
        /// 解决方案所有自定义类
        /// </summary>
        public static readonly List<Type> AllFxTypes = new List<Type>();

        /// <summary>
        /// 框架物理删除的类
        /// </summary>
        public static readonly List<Type> PhysicDeleteTypes = new List<Type>();

        /// <summary>
        /// 框架缓存数据的类
        /// </summary>
        public static readonly List<Type> CachTypes = new List<Type>();

        /// <summary>
        /// 框架按月分表的类
        /// </summary>
        public static readonly List<Type> MonthTableTypes = new List<Type>();

        /// <summary>
        /// 推送给websocket的消息类
        /// </summary>
        public static readonly List<Type> PushMessageTypes = new List<Type>();

        /// <summary>
        /// 批量存储的类
        /// </summary>
        public static readonly List<Type> BatchSaveTypes = new List<Type>();

        /// <summary>
        /// 超级管理员UserIId
        /// </summary>
        public const string ADMINID = "Admin";
    }
}
