using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Coldairarrow.Util
{
    /// <summary>
    /// 系统缓存队列
    /// </summary>
    public class SystemQuene : IQuene
    {
        private ConcurrentDictionary<string, BlockingCollection<object>> queen = new ConcurrentDictionary<string, BlockingCollection<object>>();

        public bool EnQueen(string key, params string[] value)
        {
            try
            {
                BlockingCollection<object> blocking = null;
                if (queen.ContainsKey(key))
                {
                    if (queen.TryGetValue(key, out blocking) == false)
                        return false;
                }
                if (blocking == null)
                {
                    blocking = new BlockingCollection<object>();
                    queen[key] = blocking;
                }
                value.ForEach(p => blocking.TryAdd(p, 100));                

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool EnQueen<T>(string key, params T[] value)
        {
            try
            {
                BlockingCollection<object> blocking = null;
                if (queen.ContainsKey(key))
                {
                    if (queen.TryGetValue(key, out blocking) == false)
                        return false;
                }
                if (blocking == null)
                {
                    blocking = new BlockingCollection<object>();
                    queen[key] = blocking;
                }
                value.ForEach(p => blocking.TryAdd(p, 100));               

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool EnQueen<T>(params T[] value)
        {
            string key = typeof(T).Name;
            return EnQueen<T>(key, value);
        }


        public string DeQueen(string key)
        {
            string result = "";
            try
            {
                BlockingCollection<object> blocking = null;
                //从尾部取值
                if (queen.ContainsKey(key))
                {
                    if (queen.TryGetValue(key, out blocking) == false)
                        return result;

                    if (blocking != null)
                    {
                        object item;
                        if (blocking.TryTake(out item, 100) == true)
                        {
                            result = item.ToString();
                        }
                    }
                }
                return result;
            }
            catch (Exception)
            {
                return result;
            }
        }

        public string[] DeQueenAll(string key)
        {
            string[] result = { };
            try
            {
                BlockingCollection<object> blocking = null;
                //从尾部取值
                if (queen.ContainsKey(key))
                {
                    if (queen.TryGetValue(key, out blocking) == false)
                        return result;

                    if (blocking != null)
                    {
                        int count = blocking.Count;
                        List<string> list = new List<string>();
                        for (int i = 0; i < count; i++)
                        {
                            object item;
                            if (blocking.TryTake(out item, 100) == true)
                            {
                                list.Add(item.ToString());
                            }
                        }
                        result = list.ToArray();
                    }
                }
                return result;
            }
            catch (Exception)
            {
                return result;
            }
        }    

        public T DeQueen<T>(string key)
        {
            T result = default(T);
            try
            {
                BlockingCollection<object> blocking = null;
                //从尾部取值
                if (queen.ContainsKey(key))
                {
                    if (queen.TryGetValue(key, out blocking) == false)
                        return result;

                    if (blocking != null)
                    {
                        object item;
                        if (blocking.TryTake(out item, 100) == true)
                        {
                            result = (T)item;
                        }
                    }
                }
                return result;
            }
            catch (Exception)
            {
                return result;
            }
        }

        public T DeQueen<T>()
        {
            string key = typeof(T).Name;
            return DeQueen<T>(key);
        }
        public T[] DeQueenAll<T>(string key)
        {
            T[] result = { };
            try
            {
                BlockingCollection<object> blocking = null;
                //从尾部取值
                if (queen.ContainsKey(key))
                {
                    if (queen.TryGetValue(key, out blocking) == false)
                        return result;

                    if (blocking != null)
                    {
                        int count = blocking.Count;
                        List<T> list = new List<T>();
                        for (int i = 0; i < count; i++)
                        {
                            object item;
                            if (blocking.TryTake(out item, 100) == true)
                            {
                                list.Add((T)item);
                            }
                        }
                        result = list.ToArray();
                    }
                }
                return result;
            }
            catch 
            {
                return result;
            }
        }

        public T[] DeQueenAll<T>()
        {
            string key = typeof(T).Name;
            return DeQueenAll<T>(key);
        }

        public object[] DeQueenAll(Type type)
        {
            string key = type.Name;

            #region 反射获取数据方法
            MethodInfo deQueenAll = this.GetType().GetMethod("DeQueenAll",1, new Type[] { typeof(string) }).MakeGenericMethod(new Type[] { type });
            var result = deQueenAll.Invoke(this, new Object[] { key });
            return result as object[];
            #endregion
        }
    }
}