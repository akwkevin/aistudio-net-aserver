using System;

namespace Coldairarrow.Util
{
    /// <summary>
    /// 缓存操作接口类
    /// </summary>
    public interface IQuene
    {
        #region 队列
        bool EnQueen(string key, params string[] value);
        string DeQueen(string key);
        string[] DeQueenAll(string key);
        bool EnQueen<T>(string key, params T[] value);
        bool EnQueen<T>(params T[] value);
        T DeQueen<T>(string key);
        T DeQueen<T>();
        T[] DeQueenAll<T>(string key);
        T[] DeQueenAll<T>();

        object[] DeQueenAll(Type type);
        #endregion
    }
}