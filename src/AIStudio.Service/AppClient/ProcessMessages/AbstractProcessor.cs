using Coldairarrow.Util;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace AIStudio.Service.AppClient.ProcessMessages
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class AbstractProcessor : IMessageProcessor
    {
        static AbstractProcessor()
        {
            Assembly masterAssembly = Assembly.Load("Coldairarrow.Entity");
            Type[] types = masterAssembly.GetTypes();
            foreach (Type type in types)
            {
                bool flag = AbstractProcessor.IsUserTable(type);
                if (flag)
                {
                    AbstractProcessor.Types.Add(type.Name, type);
                }
            }

            masterAssembly = Assembly.Load("Coldairarrow.Business");
            types = masterAssembly.GetTypes();
            foreach (Type type in types)
            {
                bool flag = AbstractProcessor.IsUserIBusiness(type);
                if (flag)
                {
                    AbstractProcessor.IBusinessTypes.Add(type.Name, type);
                }
            }
        }

        private static bool IsUserTable(Type type)
        {
            return type.FullName != null && type.FullName.StartsWith("Coldairarrow.Entity") && type.IsClass && !type.IsAbstract && !type.IsSealed;
        }

        private static bool IsUserIBusiness(Type type)
        {
            return type.FullName != null && type.FullName.StartsWith("Coldairarrow.Business") && type.IsInterface && type.Name != "IBusiness";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableType"></param>
        protected static void CheckReload(string tableType)
        {
               
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public abstract Task<AjaxResult<string>> DoResponse();

        /// <summary>
        /// 
        /// </summary>

        protected static readonly Dictionary<string, Type> Types = new Dictionary<string, Type>();

        /// <summary>
        /// 
        /// </summary>

        protected static readonly Dictionary<string, Type> IBusinessTypes = new Dictionary<string, Type>();
    }
}
