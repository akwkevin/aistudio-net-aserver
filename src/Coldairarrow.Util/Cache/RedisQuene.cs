using System;
using System.Collections.Generic;

namespace Coldairarrow.Util
{
    /// <summary>
    /// Redis缓存队列
    /// </summary>
    public class RedisQuene : IQuene
    {

        public bool EnQueen(string key, params string[] value)
        {
            try
            {
                //从头部插入 
                RedisHelper.LPush(key, value);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public string DeQueen(string key)
        {
            string result = "";
            try
            {
                //从尾部取值
                result = RedisHelper.RPop(key);
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
                long len = RedisHelper.LLen(key);

                //取出指定数量数据
                result = RedisHelper.LRange(key, 0, len - 1);
                //删除指定数据
                bool res = RedisHelper.LTrim(key, len, -1);

                return result;
            }
            catch (Exception)
            {
                return result;
            }
        }

        public bool EnQueen<T>(string key, params T[] value)
        {
            try
            {
                //从头部插入 
                long len = RedisHelper.LPush(key, value);
                if (len > 0)
                    return true;
                else
                    return false;
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

        public T DeQueen<T>(string key)
        {
            T result = default(T);
            try
            {
                //从尾部取值
                result = RedisHelper.RPop<T>(key);
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
                long len = RedisHelper.LLen(key);

                //取出指定数量数据
                result = RedisHelper.LRange<T>(key, 0, len - 1);
                //删除指定数据
                bool res = RedisHelper.LTrim(key, len, -1);

                return result;
            }
            catch (Exception)
            {
                return result;
            }
        }

        public T[] DeQueenAll<T>()
        {
            string key = typeof(T).Name;
            return DeQueenAll<T>(key);
        }

        /// <summary>
        /// 设置hash值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool SetHash(string key, string field, string value)
        {
            try
            {
                RedisHelper.HSet(key, field, value);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 根据表名，键名，获取hash值
        /// </summary>
        /// <param name="key">表名</param>
        /// <param name="field">键名</param>
        /// <returns></returns>
        public string GetHash(string key, string field)
        {
            string result = "";
            try
            {

                result = RedisHelper.HGet(key, field);
                return result;
            }
            catch (Exception)
            {
                return result;
            }
        }

        /// <summary>
        /// 获取指定key中所有字段
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetHashAll(string key)
        {
            try
            {
                var result = RedisHelper.HGetAll(key);
                return result;
            }
            catch (Exception)
            {
                return new Dictionary<string, string>();
            }
        }

        /// <summary>
        /// 根据表名，键名，删除hash值
        /// </summary>
        /// <param name="key">表名</param>
        /// <param name="field">键名</param>
        /// <returns></returns>
        public long DeleteHash(string key, string field)
        {
            long result = 0;
            try
            {
                result = RedisHelper.HDel(key, field);
                return result;
            }
            catch (Exception)
            {
                return result;
            }
        }

        public object[] DeQueenAll(Type type)
        {
            throw new NotImplementedException();
        }
    }
}
