using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace AIStudio.Service.Quartz
{
    public class GlobalJob
    {
        private volatile static GlobalJob instance = null;
        private static readonly object padlock = new object();

        public static GlobalJob Instance
        {
            get
            {
                if (instance == null)
                {
                    // 当第一个线程运行到这里时，此时会对locker对象 "加锁"，
                    // 当第二个线程运行该方法时，首先检测到locker对象为"加锁"状态，  
                    // 该线程就会挂起等待第一个线程解锁     
                    // 第一个线程运行完之后, 会对该对象"解锁"  
                    lock (padlock)
                    {
                        if (instance == null)
                        {
                            instance = new GlobalJob();
                        }
                    }
                }
                return instance;
            }
        }

        private GlobalJob() 
        {
            var assembly = Assembly.GetExecutingAssembly();

            AllTypes = assembly.GetTypes().Where(p => typeof(IJob).IsAssignableFrom(p)).ToList();
        }
       

        readonly List<string> _fxAssemblies =
            new List<string> {
                            "AIStudio.Service",
            };

        /// <summary>
        /// 框架所有自定义类
        /// </summary>
        public readonly List<Type> AllTypes;
    }
}
